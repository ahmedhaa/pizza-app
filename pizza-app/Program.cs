using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using pizza_app.Data;
using pizza_app.Entities.UserModel;
using pizza_app.Interfaces;
using pizza_app.Services;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
  .AddJsonOptions(options =>
   {
       options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
   });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:51952")  // Autoriser les requêtes depuis Angular
              .AllowAnyHeader()  // Permettre tous les en-têtes
              .AllowAnyMethod(); // Permettre toutes les méthodes HTTP (GET, POST, PUT, DELETE, etc.)
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddLogging();
builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Logs dans la console
    .WriteTo.File("logs/Pizzaapp.txt", rollingInterval: RollingInterval.Day)  // Logs dans un fichier avec un nouveau fichier chaque jour
    .CreateLogger();


builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Veuillez entrer le jeton JWT sous la forme 'Bearer votre_token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API de Gestion des Commandes de pizzas",
        Version = "v1",
        Description = "Une API pour gérer les commandes de Pizzas.",
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ajoutez Identity 
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Enregistrer le service avec l'interface
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPizzaService, PizzaService>();
builder.Services.AddScoped<ICommandeService, CommandeService>();





// Ajouter l'authentification JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", // Spécifiez la claim de rôle
            NameClaimType = "sub"

        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});
var app = builder.Build();
//service initializer pour initialiser les roles et les utilisateurs

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    await Initializer.Initialize(scope.ServiceProvider, userManager, roleManager);
}
//gestion erreur
app.Use(async (context, next) =>
{
    try
    {
        await next();

        if (context.Response.HasStarted)
        {
            return;
        }

        var errorMessages = new Dictionary<int, string>
        {
            { StatusCodes.Status401Unauthorized, "Non autorisé : vous devez être authentifié pour accéder à cette ressource." },
            { StatusCodes.Status402PaymentRequired, "Paiement requis pour accéder à cette ressource." },
            { StatusCodes.Status403Forbidden, "Accès interdit : vous n'avez pas les permissions nécessaires." },
            { StatusCodes.Status422UnprocessableEntity, "Données envoyées invalides ou incomplètes." },
            { StatusCodes.Status503ServiceUnavailable, "Service temporairement indisponible." }
        };

        if (errorMessages.TryGetValue(context.Response.StatusCode, out string? errorMessage))
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync($"{{\"message\": \"{errorMessage}\"}}");
        }
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Une erreur interne est survenue.");

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync($"{{\"message\": \"Une erreur interne est survenue.\", \"details\": \"{ex.Message}\"}}");
    }
});

app.UseCors("AllowAngularApp");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();


app.UseAuthorization();

app.MapControllers();

app.Run();

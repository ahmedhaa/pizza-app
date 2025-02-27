using System.ComponentModel.DataAnnotations;

namespace pizza_app.Entities.UserModel
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression("^(Admin|User)$", ErrorMessage = "Le rôle doit être 'Admin' ou 'User'.")]
        public string Role { get; set; }
    }
}

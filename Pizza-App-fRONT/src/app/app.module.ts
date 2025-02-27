import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './admin/user-management/register/register.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { AdminComponent } from './admin/admin.component';
import { PizzaManagementComponent } from './pizza-management/pizza-management.component';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { AuthInterceptor } from './auth.interceptor';
import { FormsModule } from '@angular/forms'; // Importer FormsModule
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { UpdatePizzaDialogComponent } from './pizza-management/update-pizza-dialog/update-pizza-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { MatTableModule } from '@angular/material/table';
import { AuthGuard } from './auth.guard';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select'; // Importez ce module
import { MatOptionModule } from '@angular/material/core';
import { OrderManagementComponent } from './admin/order-management/order-management.component';
import { UpdateStatusDialogComponent } from './admin/order-management/update-status-dialog/update-status-dialog.component';
import { UserRoutingModule } from './user/user-routing.module';
import { UserModule } from './user/user.module';





@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    AdminComponent,
    PizzaManagementComponent,
    UpdatePizzaDialogComponent,
    UserManagementComponent,
    OrderManagementComponent,
    UpdateStatusDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatCardModule,
    MatInputModule,
    MatIconModule,
    MatListModule,
    FormsModule,
    MatSnackBarModule,
    MatDialogModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,  // Ajoutez ce module
    MatOptionModule,
    UserRoutingModule,
    UserModule
  ],
  providers: [
    // Enregistrer l'intercepteur pour toutes les requêtes HTTP
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    AuthGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

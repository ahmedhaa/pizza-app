import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { AdminComponent } from './admin/admin.component';
import { PizzaManagementComponent } from './pizza-management/pizza-management.component';
import { AuthGuard } from './auth.guard';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { OrderManagementComponent } from './admin/order-management/order-management.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // Redirection vers login par défaut
  { path: 'login', component: LoginComponent },
  {
    path: 'admin', component: AdminComponent, canActivate: [AuthGuard], children: [
      { path: 'pizza-management', component: PizzaManagementComponent },
      { path: 'user-management', component: UserManagementComponent },
      { path: 'order-management', component: OrderManagementComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

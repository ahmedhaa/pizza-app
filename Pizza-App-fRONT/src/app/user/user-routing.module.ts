// src/app/user/user-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProfileComponent } from './profile/profile.component';
import { OrdersComponent } from './orders/orders.component';
import { AuthGuard } from '../auth.guard';

const routes: Routes = [
    {
        path: 'user/profile',
        component: ProfileComponent,
        canActivate: [AuthGuard],  // Protège la route avec un garde
    },
    {
        path: 'user/orders',
        component: OrdersComponent,
        canActivate: [AuthGuard],  // Protège la route avec un garde
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UserRoutingModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileComponent } from './profile/profile.component';
import { OrdersComponent } from './orders/orders.component';
import { MatCardModule } from '@angular/material/card';



@NgModule({
  declarations: [
    ProfileComponent,
    OrdersComponent
  ],
  imports: [
    CommonModule,
    MatCardModule
  ]
})
export class UserModule { }

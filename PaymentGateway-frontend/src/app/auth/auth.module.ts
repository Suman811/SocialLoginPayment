import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthRoutingModule } from './auth-routing.module';

import { LoginComponent } from './login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { HttpClientModule } from '@angular/common/http';
import { FbauthorizationComponent } from './fbauthorization/fbauthorization.component'; 
import { MatDialogModule } from '@angular/material/dialog';

@NgModule({
  declarations: [
   
    LoginComponent,
    FbauthorizationComponent
  ],
  imports: [
    CommonModule,
    AuthRoutingModule,
    ReactiveFormsModule,
    ToastrModule,
    HttpClientModule,
    MatDialogModule
  ]
})
export class AuthModule { }

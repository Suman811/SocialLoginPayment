import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ToastrModule } from 'ngx-toastr';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialog } from '@angular/material/dialog';



@NgModule({
  declarations: [
    AppComponent
  ],
  
  imports: [
    BrowserModule,
   // SocialLoginModule,
    AppRoutingModule,
    HttpClientModule,
    
    
   
  
    ToastrModule.forRoot({
      timeOut:2000,
      positionClass:'toast-top-right',
      preventDuplicates:true

    }),
       BrowserAnimationsModule,
       
  
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

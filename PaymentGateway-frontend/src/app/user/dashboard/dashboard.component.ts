import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialogModule } from '@angular/material/dialog';
import { AuthServiceService } from 'src/app/services/auth-service.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  
  constructor(private route: Router ,private authService:AuthServiceService) {}

  subscribe(planType: string): void {
    
    this.route.navigate(['user/summary'], { queryParams: { plan: planType } });
  }
  logout(){
    //localStorage.removeItem('token');
   this.route.navigate(['']);
  }
  
}

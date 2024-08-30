

import { HttpClient } from '@angular/common/http';
import { ThisReceiver, Token } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { AuthServiceService } from 'src/app/services/auth-service.service';
import { FbauthService } from 'src/app/services/fbauth.service';
import { FbauthorizationComponent } from '../fbauthorization/fbauthorization.component';
import { MatDialog } from '@angular/material/dialog';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginform = this.fb.group({
    email: ['', [Validators.email, Validators.required]],
    password: ['', Validators.required]
  })

  showPassword = false;
  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  getControl(a: any) {
    return this.loginform.get(a);
  }

  saveForm() {
    console.log(this.loginform.value);
   
  }
 
  
  decodeJwt(token: string): any {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
  
    return JSON.parse(jsonPayload);

    
  }

  constructor(
    private fb: FormBuilder,
    private route: Router,
    private toastr: ToastrService,
    private authService:AuthServiceService   ,
    private facebookAuthService: FbauthService,
    private http: HttpClient,
    private dialog: MatDialog,
   
    
  ) { }

  
  


//changed latest
    
  private fbLoaded = false;



  ngOnInit(): void {

    const handleCredentialResponse = (response: any): void => {
      console.log('Encoded JWT ID token: ' + response.credential);

      const decodedToken = this.decodeJwt(response.credential);
      console.log('Decoded Token:', decodedToken);
  
      
      const userName = decodedToken.name;
      const userEmail = decodedToken.email;
      const googleID = decodedToken.sub;
      
  
      console.log('User Name :', userName);
      console.log('User Email:', userEmail);
      console.log('GoogleID:',googleID);


      this.authService.googleLogin(response.credential,googleID,userEmail,userName).subscribe((res: any)=> {
       
          console.log('Server response:', res);
          // if(res==response.credential){
          //   this.route.navigate(['/user']);
          // }
         
          this.route.navigate(['/user']);
        }, (error) => {
          console.error('Error:', error);
        });
      
    };

    
    if (window.google) {
      window.google.accounts.id.initialize({
        client_id: '447038926527-dtt81afop239tds5vetdol8femost6e7.apps.googleusercontent.com',
        callback: handleCredentialResponse
      });

      window.google.accounts.id.renderButton(
        document.querySelector('.g_id_signin') as HTMLElement,
        { theme: 'outline', size: 'large', shape: 'pill', logo_alignment: 'left' }
      );
    } else {
      console.error('Google Sign-In script not loaded.');
    }
  
  }
    
 

 


  public loadFacebookSDK(): void {
   
   this.openDialog();
    ((d, s, id) => {
      let js: any, fjs = d.getElementsByTagName(s)[0];
      if (d.getElementById(id)) { return; }
      js = d.createElement(s); js.id = id;
      js.src = 'https://connect.facebook.net/en_US/sdk.js';
      fjs.parentNode?.insertBefore(js, fjs);
      js.onload = () => {
        window.FB.init({
          appId: '517432940814916', 
          cookie: true,
          xfbml: true,
          version: 'v20.0'
        });
     
        window.FB.getLoginStatus((response: any) => {
          this.statusChangeCallback(response);
        });
        window.FB.Event.subscribe('auth.statusChange', (response: any) => {
          this.statusChangeCallback(response);
        });
       
      };
    })(document, 'script', 'facebook-jssdk');
  }

  openDialog(): void {
    //debugger
   const dialogRef= this.dialog.open(FbauthorizationComponent, {
      width: '250px',
      
    });
    dialogRef.beforeClosed().subscribe(response => {
      console.log('Dialog closed');
      if (response) {
       
      }
    });
  }
  
  public statusChangeCallback(response: any): void {
    if (response.status === 'connected') {
      this.fetchFacebookUserData(response.authResponse.accessToken);
      this.hideFacebookLoginButton();
      this.closedialog();
    }
  }
  hideFacebookLoginButton(): void {
    const fbLoginButton = document.querySelector('.fb-login-button');
    if (fbLoginButton) {
      (fbLoginButton as HTMLElement).style.display = 'none';
    }
  }
  closedialog(){
   this.dialog.closeAll();
  }
  public fetchFacebookUserData(accessToken: string): void {
  
    window.FB.api('/me', { fields: 'name,email' }, (response: any) => {
    
      console.log('Facebook User Data:', response);
     
      const userName = response.name;
      const userEmail = response.email;
      const facebookID = response.id;
      

      console.log('User Name:', userName);
      console.log('User Email:', userEmail);
      console.log('Facebook ID:', facebookID);
      localStorage.setItem('facebookAccessToken', accessToken);
      
      console.log(accessToken);
     // this.closeDialogAndNavigate('/user');
      // Send the token and user data to your backend
      this.facebookAuthService.facebookLogin(accessToken, facebookID, userEmail, userName).subscribe((res: any) => {
        console.log('Server response:', res);
    
        this.route.navigate(['/user']);
      }, (error) => {
        console.error('Error:', error);
      });
    });
  }
 

  public loginWithFacebook(): void {
   
    this.facebookAuthService.loginWithFacebook().then(response => {
      if (response.authResponse) {
       
        this.fetchFacebookUserData(response.authResponse.accessToken);
      } else {
       
        console.error('Login failed or was canceled.');
      }
    }).catch(error => {
    
      console.error('Facebook login error:', error);
      alert('There was an issue with logging in via Facebook. Please try again later.');
    });
  }
  
  public facebookLogin(): void {
    window.FB.login((response: any) => {
      this.statusChangeCallback(response);
    
    }, { scope: 'public_profile, email' });
  }

 
  
}

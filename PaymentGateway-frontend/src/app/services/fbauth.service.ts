import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FbauthService {
  
  constructor(private http:HttpClient){}

 

  
  

  public loginWithFacebook(): Promise<any> {
    return new Promise((resolve, reject) => {
      (window as any).FB.login((response: any) => {
        if (response.authResponse) {
          resolve(response.authResponse);
        } else {
          reject('User cancelled login or did not fully authorize.');
        }
      }, { scope: 'email' });
    });
  }


  public getFacebookProfile(): Promise<any> {
    
    return new Promise((resolve, reject) => {
      (window as any).FB.api('/me', { fields: 'name,email' }, (response: any) => {
        if (!response || response.error) {
          reject(response.error);
        } else {
          resolve(response);
        }
      });
    });
  }
  facebookLogin(accessToken: string, facebookID: string, email: string, name: string): Observable<any> {
    return this.http.post<any>(`https://localhost:7131/api/Values/Facebook`, {
      accessToken,
      facebookID,
      email,
      name
    });
  }
}

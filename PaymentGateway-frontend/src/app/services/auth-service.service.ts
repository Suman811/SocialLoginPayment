import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { PayPayRequest } from '../models/PayPalRequest';


export interface PaymentRequest {
  userId: number;
  email: string;
  sourceToken: string;
  amount: number;
}


@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {

  
  private backendUrl = 'https://localhost:7131/api/Values/google';
  private stripeApiUrl = 'https://localhost:7131/api/StripePayment/create-payment';
  private paypalApiUrl = 'https://localhost:7131/api/PayPal/process-request';
  private invoiceApiUrl = 'https://localhost:7131/api/PayPal/invoice'; 
  private stripInvoiceUrl = 'https://localhost:7131/api/StripePayment/transaction/';

  
 
   idToken: string | null = null;

 
  storeIdToken(token: string): void {
    this.idToken = token;
    
    localStorage.setItem('idToken', token);
  }

  getIdToken(): string | null {
    return this.idToken || localStorage.getItem('idToken');
  }

  clearUserData(): void {
    this.idToken = null;
    localStorage.removeItem('idToken');
   
  }
  constructor(private http: HttpClient,private route:Router) { }
  

 
  googleLogin(token:any, googleID: any,Email:any,UserName:any): Observable<any> {
   
    return this.http.post(`https://localhost:7131/api/Values/google`, { token,googleID,Email,UserName });
  }
   
  
  login(email: string, password: string): Observable<any> {
    return this.http.post(`${this.backendUrl}/login`, { email, password });
  }

  
  getUserInfo(): Observable<any> {
    return this.http.get(`${this.backendUrl}/user-info`);
  }

 
  logout(): void {
   
    localStorage.removeItem('token');
   this.route.navigate(['']);
  }
  
  saveStripeTransaction(data: PaymentRequest): Observable<any> {
    return this.http.post<any>(this.stripeApiUrl, data).pipe(
      catchError(this.handleError)
    );
  }

  savePayPalTransaction(data: PayPayRequest): Observable<any> {
    return this.http.post<any>(this.paypalApiUrl, data).pipe(
      catchError(this.handleError)
    );
  }

  getInvoice(transactionId: string): Observable<any> {
    return this.http.get<any>(`${this.invoiceApiUrl}/${transactionId}`).pipe(
      catchError(this.handleError)
    );
  }

  getTransactionDetails(transactionId: string): Observable<string> {
    return this.http.get<any>(`${this.stripInvoiceUrl}${transactionId}`).pipe(
      map(response => {
        // Assuming the response has a property named 'InvoiceUrl'
        if (response && response.InvoiceUrl) {
          return response.InvoiceUrl;
        } else {
          throw new Error('Invoice URL not found in the response');
        }
      }),
      catchError(this.handleError)
    );
  }


  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('An error occurred:', error.message);
    return throwError(() => new Error('Something went wrong; please try again later.'));
  }
}

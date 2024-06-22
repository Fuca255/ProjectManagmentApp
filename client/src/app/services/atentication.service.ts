import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Login } from '../models/login';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  baseUrl:string = environment.apiUrl
  private router = inject(Router);
  private spinner = inject(NgxSpinnerService);

  constructor(private httpClient: HttpClient) { }

  // API
  login(loginData: Login):Observable<string>{
    const httpOptions = {
      responseType: 'text' as 'json'  // Postavite responseType na 'text'
    };
    return this.httpClient.post<string>(this.baseUrl+`/Auth/login`, loginData, httpOptions)
  }

  logout() : void
  {
    // this.spinner.show();
    localStorage.removeItem("token");
    // this.spinner.hide();
    this.router.navigate(["auth"]);
    
  }

  // Services
  setToken(token: string){
    // kada se uspesno ulogujemo, zovemo funkciju da setujemo token dobijen sa beka
    let localStorage : Storage;
    if(typeof window !== 'undefined' && window.localStorage)
    {
      localStorage = window.localStorage;
      // console.log("Postavio sam token")
      localStorage.setItem("token", token);
    }
    
  }

  isAuthenticated(): boolean {
    let localStorage : Storage;
    if(typeof window !== 'undefined' && window.localStorage)
    {
      // console.log("Usao sam u if za autentifikaciju")
      localStorage = window.localStorage;
      if(localStorage.getItem("token"))
      {
        // console.log("Autentifikovan sam")
        return true
      }
    }
    return false
  }

  forgotPassword(email: string) : Observable<any> {
    const url = `${this.baseUrl}/Auth/forgotpassword?email=${email}`;
    return this.httpClient.post<any>(url, {});
  }

  resetPassword(payload: { resetToken: string , newPassword: string }): Observable<any> {
    const url = `${this.baseUrl}/Auth/resetpassword`;
    return this.httpClient.post(url, payload);
  }
}

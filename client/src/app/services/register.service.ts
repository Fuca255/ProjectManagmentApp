import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {
  apiUrl = environment.apiUrl;
  private baseUrl:string = `${this.apiUrl}/Auth/`;
  constructor(private http : HttpClient) { }

  register(userObj:any){
    return this.http.post<any>(`${this.baseUrl}register`,userObj);
  }
}
import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class JwtDecoderService {

  constructor() { }

  public decodeToken(token: string): any {
    try {
      return jwtDecode(token);
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }

  public getToken(): string | null {
    // Implement logic to retrieve the token from storage (e.g., localStorage, cookies)
    const token = localStorage.getItem('token');
    return token ? token : null;
  }

  public getLoggedUser() : any | null{
    let token = this.getToken();
    if(token)
    {
      return this.decodeToken(token);
    }
    return null;
  }
}

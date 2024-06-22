import { Injectable, inject } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from './atentication.service';
import { Location } from '@angular/common';
import { PreviousUrlService } from './url.service';
import { JwtDecoderService } from './jwt-decoder.service';
import { InvalidTokenError } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard{

  constructor(private authService: AuthenticationService, private router: Router, private location: Location) {}
  private jwt_serv = inject(JwtDecoderService);
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | boolean | UrlTree {

    const isLoggedIn = this.authService.isAuthenticated();

    if (isLoggedIn) {
      
      if (route.url[0].path === 'auth') {
        // console.log(this.location.path())
        let token = this.jwt_serv.getToken();
        if(token!==null)
        {
          let decoded = this.jwt_serv.decodeToken(token);
          if(decoded.role==="Admin")
            this.router.navigate(['/admin']);
          else if (decoded.role==="Project Manager")
            this.router.navigate(['/dashboard']);
          else
            this.router.navigate(['/projects']);
        }
        return false;
      }
      return true;
    } else {
      if (route.url[0].path !== 'auth') {
        // console.log(this.location.path())
        this.router.navigate(['/auth']);
        return false;
      }
      return true;
    }
  }
}
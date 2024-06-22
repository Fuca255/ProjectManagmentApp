import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { AuthenticationService } from './atentication.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authService: AuthenticationService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.isAuthenticated() ? localStorage.getItem('token') : null;

    let cloned = req;
    if (token) {
      cloned = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)
      });
    }

    return next.handle(cloned).pipe(
      catchError(err => this.handleAuthError(err))
    );
  }

  private handleAuthError(err: HttpErrorResponse): Observable<never> {
    if (err.status === 401) {
      this.authService.logout()
    }
    return throwError(err);
  }
}

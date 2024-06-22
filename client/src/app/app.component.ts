import { AfterViewInit, ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { AuthenticationService } from './services/atentication.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'client';

  userIsLogged: boolean = false
  

  constructor(private router: Router, public authService: AuthenticationService){

  }
  isLoginPage()
  {
    const currentUrl = this.router.url;
    const hasToken = currentUrl.includes('/reset-password') && currentUrl.includes('token=');
    return currentUrl === '/auth' || currentUrl === '/reset-password' || hasToken;
  }

  ngOnInit(): void {
    // this.userIsLogged = this.authService.isAuthenticated();
    // console.log("logovan sam iz app " + this.authService.isAuthenticated());
  }


}

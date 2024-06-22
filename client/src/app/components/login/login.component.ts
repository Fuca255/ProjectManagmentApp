import { HttpClient } from '@angular/common/http';
import { Component, inject} from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { environment } from '../../environments/environment';
import { Login } from '../../models/login';
import { AuthenticationService } from '../../services/atentication.service';
import { NgxSpinner, NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { NotificationService } from '../../services/notification.service';
import { error } from 'console';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent{
  loginObj: Login = {
    // username: "johndoe",
    // password: "password123"
  } as Login;

  loginForm!: FormGroup;

  idUser='';

  showPassword: boolean = false;
  errMessage : string = "";

  decodedToken: any;
  apiUrl = environment.apiUrl;
  spinner = inject(NgxSpinnerService);

  showForgotPassword = false;
  forgotPassword = false;
  forgotPasswordEmail = '';
  invalidEmail: boolean = false;
  buttonClicked: boolean = false;
  sent: boolean = false;

  constructor(private fb: FormBuilder,private http: HttpClient,private router: Router,private jwtDecoderService: JwtDecoderService, private authService: AuthenticationService) {
  }

  onLogin(): void {
    this.spinner.show();
    this.authService.login(this.loginObj).subscribe({
      next:(response)=>{
        this.spinner.hide();
        this.authService.setToken(response);
        let decoded = this.jwtDecoderService.decodeToken(response);

        if(decoded.role==="Admin")
          this.router.navigate(['/admin']);
        else if (decoded.role==="Project Manager")
          this.router.navigate(['/dashboard']);
        else
          this.router.navigate(['/projects']);
      },
      error:(error)=>{
        this.spinner.hide();
        // console.log(error)
        if(this.loginObj.username ===undefined || this.loginObj.password === undefined)
        {
          this.errMessage = "Please enter your credentials.";
        }
        else
          this.errMessage = "Wrong credentials.";
      }
    })
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }


  toggleForgotPasswordVisibility() {
    this.showForgotPassword = !this.showForgotPassword;
  }

  showForgotPasswordForm() {
    this.forgotPassword = true;
  }

  cancelForgotPassword() {
    this.forgotPassword = false;
  }

  sendResetPassword() {
    this.buttonClicked = true;
    if (this.invalidEmail) {
      return;
    }
    this.authService.forgotPassword(this.forgotPasswordEmail).subscribe(
      response => {
        this.forgotPassword = false;
        this.sent = true;
      },
      error => {
        console.error('Error sending password reset email', error);
        this.sent = false;
      }
    );
  }


  validateEmail() {
    const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    this.invalidEmail = !emailPattern.test(this.forgotPasswordEmail);
  }

  backToLogin() {
    this.sent = false;
    this.forgotPassword = false;
  }
}

import { Component, Input, OnInit, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterService } from '../../services/register.service';
import { Role } from '../../models/role';
import { Register } from '../../models/register';
import { NgxSpinnerService } from 'ngx-spinner';
import { Notify } from '../../models/notifications/notify';
import { NgToastService } from 'ng-angular-popup';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  notify : Notify;

  registerForm!: FormGroup;
  private spinner = inject(NgxSpinnerService);
  public validRole : boolean = false;
  @Input() public roles : Role[] = [];
  constructor(private fb: FormBuilder, private reg: RegisterService, private router: Router,private toast : NgToastService) {
    this.notify = new Notify(toast)
  }

  ngOnInit(): void {
    this.spinner.show();
    this.registerForm = this.fb.group({
      Name: ['', [Validators.required, Validators.pattern('[a-zA-Z ]*')]],
      Lastname: ['', [Validators.required, Validators.pattern('[a-zA-Z ]*')]],
      Email: ['', [Validators.required, Validators.pattern('[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}')]],
      Username: ['', [Validators.required, Validators.pattern('[a-zA-Z0-9_\\-\\.]*')]],
      Password: ['', [Validators.required, Validators.minLength(8)]],
      ConfirmPassword: ['', Validators.required],
      Department: [''],
      Organisation: [''],
      Role : [0]
    }, { validators: this.passwordMatchValidator });
    this.spinner.hide();
  }

  onSubmit() {
    this.spinner.show();
    if (this.registerForm.valid) {
      const { ConfirmPassword, ...formData } = this.registerForm.value;
      let register : Register = {
        username: formData.Username,
        name: formData.Name,
        lastname: formData.Lastname,
        password: formData.Password,
        email: formData.Email,
        roleId: formData.Role,
        department: formData.Department,
        organisation: formData.Organisation
      }
      if(register.roleId <= 0)
      {
        this.validRole = true;
        this.spinner.hide();
        return;
      }
      this.validRole = false;
      this.reg.register(register)
        .subscribe({
          next: (res) => {
            if(res.message == "This email already exists in the database.") {
              this.registerForm.get('Email')?.setErrors({ 'emailExists': true });
              this.notify.showWarn("Registration","Registration form not filled correctly!");
              this.spinner.hide();
              return;
            }
            if(res.message == "This username already exists in the database.") {
              this.registerForm.get('Username')?.setErrors({ 'usernameExists': true });
              this.notify.showWarn("Registration","Registration form not filled correctly!");
              this.spinner.hide();
              return;
            }
            // alert("User " + register.name + " " + register.lastname + " added successfully!");
            this.registerForm.reset();
            this.spinner.hide();
            this.notify.showSuccess("User added","User registered successfully!")
          },
          error: (err) => {
            this.notify.showWarn("Registration","Registration form not filled correctly!")
            this.spinner.hide();
          }
        });
    } else {
      this.validateAllFormFields(this.registerForm);
      // alert("Your form is invalid");
      this.spinner.hide();
    }
  }

  private passwordMatchValidator = (group: FormGroup) => {
    
    const passwordControl = group.get('Password');
    const confirmPasswordControl = group.get('ConfirmPassword');

    if (!passwordControl || !confirmPasswordControl) return null;

    if (passwordControl.value !== confirmPasswordControl.value) {
      confirmPasswordControl.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    } else {
      confirmPasswordControl.setErrors(null);
      return null;
    }
  };

  private validateAllFormFields(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      if (control instanceof FormControl) {
        control.markAsDirty({ onlySelf: true });
      } else if (control instanceof FormGroup) {
        this.validateAllFormFields(control);
      }
    });
  }
}
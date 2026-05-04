import { ChangeDetectorRef, Component } from '@angular/core';
import { LoginService } from '../login-service';
import { Router } from '@angular/router';
import { AlertService } from '../../../shared/alert/alert-service';
import { LoginInterface } from '../login-interface';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login 
{
  validationErrors: { [key: string]: string[] } = {};
  login = 
  {
    username: '',
    userPassword: ''
  };
  user:LoginInterface | null = null;

  constructor(private loginService: LoginService, private router: Router, private alertService: AlertService, private cdr: ChangeDetectorRef) 
  { 
  }

  UserLogin() 
  {
      this.loginService.Login(this.login).subscribe({
      next: (response) => 
      {
        this.user = response.data;
        sessionStorage.setItem('Logged_FullName', this.user?.fullName ?? '');
        sessionStorage.setItem('Logged_Username', this.user?.username ?? '');
        sessionStorage.setItem('Logged_Password', this.user?.userPassword ?? '');   
        this.router.navigate(["tasks/home"]);
      },
      error:(err)=>
      {
         if (err?.error?.errors) 
         {
            const validationErrors = err.error.errors;
            const errorMessages = Object.keys(validationErrors).map(key => validationErrors[key].join(' ')).join('\n');

            this.alertService.showError(errorMessages);
            this.cdr.markForCheck(); 
         } 
         else if (err?.error?.details) 
         {
            this.alertService.showError(err.error.details);
            this.cdr.markForCheck();
         } 
      }
      }) 
  }

  GoToCreateUser() 
  {
    this.router.navigate(["login/create-user"]);
  }

   ClearErrors(field: string): void 
   {
     if (this.validationErrors[field]) 
     {
       delete this.validationErrors[field];
     }
   }

}

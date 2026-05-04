import { ChangeDetectorRef, Component } from '@angular/core';
import { LoginInterface } from '../login-interface';
import { LoginService } from '../login-service';
import { Router } from '@angular/router';
import { AlertService } from '../../../shared/alert/alert-service';

@Component({
  selector: 'app-create-user',
  standalone: false,
  templateUrl: './create-user.html',
  styleUrl: './create-user.css',
})

export class CreateUser 
{
   validationErrors: { [key: string]: string } = {};
   users:LoginInterface=
   {
     id:0,
     username:'',
     userPassword:'',
     fullName:''
   }
   confirmPassword: string = '';

   constructor(private loginService:LoginService, private router:Router, private alertService: AlertService, private cdr: ChangeDetectorRef)
   {  
   }

   SaveUser() 
   {    
      this.loginService.SaveUser(this.users).subscribe({
      next:(response)=>
      {  
         this.alertService.showSuccess(response.details);
         this.ClearUI();
         this.cdr.markForCheck();
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
   
   Cancel()
   {
      this.ClearUI();
      this.router.navigate(["login/login"]);
   }   

   ClearErrors(field: string): void 
   {
     if (this.validationErrors[field]) 
     {
       delete this.validationErrors[field];
     }
   }

   ClearUI() 
   {
    this.users = 
    {
      username: '',
      userPassword: '',
      fullName: ''
    };

    this.confirmPassword= '',
    this.validationErrors = {};
   }

}
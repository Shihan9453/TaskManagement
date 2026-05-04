import { Component, OnInit } from '@angular/core';
import { AlertService } from './alert-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-alert',
  standalone: false,
  templateUrl: './alert.html',
  styleUrl: './alert.css',
})

export class Alert implements OnInit
{
  
  showSuccess = false;
  showError = false;
  successMessage = '';
  errorMessage = '';
  successFading = false;
  errorFading = false;

  constructor(private alertService: AlertService, private router: Router) 
  {
  }

  ngOnInit() 
  {
      this.alertService.success$.subscribe(msg => 
      {
      this.successMessage = msg;
      this.showSuccess = true;
      this.successFading = false;

      setTimeout(() => (this.successFading = true), 2500);
      setTimeout(() => 
      {
        this.showSuccess = false;
        location.reload();
      }, 3500);
      });

      this.alertService.error$.subscribe(msg => {
      this.errorMessage = msg;
      this.showError = true;
      this.errorFading = false;

      setTimeout(() => (this.errorFading = true), 2500);
      setTimeout(() => 
      {
        this.showError = false;
        location.reload();
      }, 3500);
    });
  }
    
}
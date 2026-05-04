import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})

export class Navbar 
{  
  fullName: string = '';

  constructor(private router: Router) 
  { 
  }

  ngOnInit() 
  {
    this.fullName = sessionStorage.getItem('Logged_FullName') || '';
  }

  logout() 
  {
    sessionStorage.clear(); 
    this.router.navigate(['/login']); 
  }

}


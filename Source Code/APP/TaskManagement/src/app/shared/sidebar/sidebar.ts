import { Component } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  standalone: false,
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})

export class Sidebar 
{
  isCollapsed = true;

  expandSidebar() 
  {
    this.isCollapsed = false;
  }

  collapseSidebar() 
  {
    this.isCollapsed = true;
  }
}

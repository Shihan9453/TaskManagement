import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedRoutingModule } from './shared-routing-module';
import { Alert } from './alert/alert';
import { Footer } from './footer/footer';
import { Navbar } from './navbar/navbar';
import { Sidebar } from './sidebar/sidebar';
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [
    Alert,
    Footer,
    Navbar,
    Sidebar
  ],
  imports: [
    CommonModule,
    SharedRoutingModule,
    RouterModule
  ],
    exports: [
    Navbar,
    Sidebar,
    Alert,
    Footer
  ] 
})

export class SharedModule 
{
}

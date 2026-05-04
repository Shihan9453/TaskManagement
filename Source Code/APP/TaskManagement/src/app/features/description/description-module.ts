import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DescriptionRoutingModule } from './description-routing-module';
import { Description } from './description/description';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    Description
  ],
  imports: [
    CommonModule,
    DescriptionRoutingModule,
    FormsModule
  ]
})

export class DescriptionModule 
{ 
}

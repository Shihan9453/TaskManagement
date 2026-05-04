import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Description } from './description/description';

const routes: Routes = 
[ 
  { path: 'description', component: Description }, 
  { path: '', redirectTo: 'description', pathMatch: 'full' } 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class DescriptionRoutingModule 
{ 
}

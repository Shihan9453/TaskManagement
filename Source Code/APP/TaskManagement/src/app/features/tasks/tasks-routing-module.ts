import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Home } from './home/home';
import { TaskList } from './task-list/task-list';
import { TaskForm } from './task-form/task-form';

const routes: Routes = 
[
  { 
    path: 'home', 
    component: Home,
    children: 
    [
      { path: '', component: TaskForm },              
      { path: 'task-form/:id', component: TaskForm } 
    ]
  },
  { path: '', redirectTo: 'home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class TasksRoutingModule 
{  
}

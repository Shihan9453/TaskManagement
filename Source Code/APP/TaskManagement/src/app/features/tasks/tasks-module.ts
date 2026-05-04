import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TasksRoutingModule } from './tasks-routing-module';
import { Home } from './home/home';
import { TaskList } from './task-list/task-list';
import { TaskForm } from './task-form/task-form';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared-module';


@NgModule({
  declarations: [
    Home,
    TaskList,
    TaskForm
  ],
  imports: [
    CommonModule,
    TasksRoutingModule,
    FormsModule,
    SharedModule
  ]
})

export class TasksModule 
{  
}

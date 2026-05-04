import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Layout } from './layout/layout/layout';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  { path: 'login', loadChildren: () => import('./features/login/login-module').then(m => m.LoginModule) },
  { path: '', component: Layout,
    children: 
    [
      { path: 'tasks', loadChildren: () => import('./features/tasks/tasks-module').then(m => m.TasksModule) },
      { path: 'description', loadChildren: () => import('./features/description/description-module').then(m => m.DescriptionModule)}
    ]
  },
  { path: '**', redirectTo: 'login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule 
{ 
}

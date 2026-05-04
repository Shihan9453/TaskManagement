import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from './shared/shared-module';
import { LayoutModule } from './layout/layout-module';
import { LoginModule } from './features/login/login-module';
import { TasksModule } from './features/tasks/tasks-module';
import { environment } from '../environments/environment.development';
import { basicAuthInterceptor } from './interceptors/basic-auth-interceptor';

@NgModule({
  declarations: [
    App
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    SharedModule,
    LayoutModule,
    LoginModule,
    TasksModule
  ],
  providers: 
  [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptors([basicAuthInterceptor])),
    { provide: 'APP_URL', useValue: environment.apiUrl },
  ],
  bootstrap: [App]
})

export class AppModule 
{ 
}

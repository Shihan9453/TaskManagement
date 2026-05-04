import { HttpInterceptorFn } from '@angular/common/http';

export const basicAuthInterceptor: HttpInterceptorFn = (req, next) => 
{

  const username = sessionStorage.getItem('Logged_Username') || '';
  const password = sessionStorage.getItem('Logged_Password') || '';
  
  const authHeader = 'Basic ' + btoa(`${username}:${password}`);
  
  const authReq = req.clone({ setHeaders: { Authorization: authHeader }});
  return next(authReq);

};
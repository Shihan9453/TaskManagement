import { HttpClient } from '@angular/common/http';
import { Inject, inject, Injectable } from '@angular/core';
import { LoginInterface } from './login-interface';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../api-response';

@Injectable({
  providedIn: 'root',
})

export class LoginService 
{

  private apiUrlAuth: string;
  private apiUrlUser: string;
  
  constructor(private _httpClient: HttpClient, @Inject('APP_URL') private baseUrl: string) 
  {
    this.apiUrlAuth = `${this.baseUrl}/Auth`;
    this.apiUrlUser = `${this.baseUrl}/User`;
  }

  Login(data: LoginInterface): Observable<ApiResponse<LoginInterface>> 
  {
    return this._httpClient.post<ApiResponse<LoginInterface>>(`${this.apiUrlAuth}/Login`, data);
  }
  
  SaveUser(data: LoginInterface): Observable<ApiResponse<LoginInterface>> 
  {
    return this._httpClient.post<ApiResponse<LoginInterface>>(`${this.apiUrlUser}/SaveUser`, data);
  }

}

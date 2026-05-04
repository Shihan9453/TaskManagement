import { HttpClient } from '@angular/common/http';
import { Inject, inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApiResponse } from '../../api-response';
import { TaskInterface } from './task-interface';
import { UserInterface } from './user-interface';

@Injectable({
  providedIn: 'root',
})

export class TaskService 
{
  private refreshListSource = new BehaviorSubject<boolean>(false);
  refreshList$ = this.refreshListSource.asObservable();

  private selectedTask = new BehaviorSubject<any>(null);
  selectedTask$ = this.selectedTask.asObservable();

  private apiUrlTask: string;
  private apiUrlUser: string;

  constructor(private _httpClient: HttpClient, @Inject('APP_URL') private baseUrl: string) 
  {
    this.apiUrlTask = `${this.baseUrl}/Task`;
    this.apiUrlUser = `${this.baseUrl}/User`;
  }
  
  GetTasks(): Observable<ApiResponse<TaskInterface[]>> 
  {
    return this._httpClient.get<ApiResponse<TaskInterface[]>>(`${this.apiUrlTask}/GetTasks`);
  }

  GetTask(id: number): Observable<ApiResponse<TaskInterface>> 
  {
    return this._httpClient.get<ApiResponse<TaskInterface>>(`${this.apiUrlTask}/GetTask/${id}`);
  }

  SaveTask(data: TaskInterface): Observable<ApiResponse<TaskInterface>> 
  {
    return this._httpClient.post<ApiResponse<TaskInterface>>(`${this.apiUrlTask}/SaveTask`, data);
  }

  UpdateTask(data: TaskInterface): Observable<ApiResponse<TaskInterface>> 
  {
    return this._httpClient.put<ApiResponse<TaskInterface>>(`${this.apiUrlTask}/EditTask/${data.id}`, data);
  }

  DeleteTask(id: number): Observable<ApiResponse<null>> 
  {
    return this._httpClient.delete<ApiResponse<null>>(`${this.apiUrlTask}/DeleteTask/${id}`);
  }

  GetStatuses(): Observable<ApiResponse<string[]>> 
  {
    return this._httpClient.get<ApiResponse<string[]>>(`${this.apiUrlTask}/GetTaskStatuses`);
  }

  GetPriorityLevels(): Observable<ApiResponse<string[]>> 
  {
    return this._httpClient.get<ApiResponse<string[]>>(`${this.apiUrlTask}/GetTaskPriorityLevels`);
  }  
  
  GetUsers(): Observable<ApiResponse<UserInterface[]>> 
  {
    return this._httpClient.get<ApiResponse<UserInterface[]>>(`${this.apiUrlUser}/GetUsers`);
  }

  triggerRefresh() 
  {
    this.refreshListSource.next(true);
  }

  setSelectedTask(task: any) 
  {
    this.selectedTask.next(task);
  }

}

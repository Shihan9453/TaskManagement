export interface ApiResponse<T> 
{
  status: number;
  message: string;
  details: string;
  data: T;
}
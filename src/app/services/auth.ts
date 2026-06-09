import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http: HttpClient
  ) {
  }
register(data: any)
{
  return this.http.post(
    'https://localhost:7113/api/Auth/register',
    data,
    {
      responseType: 'text'
    });
}
  login(data: any) {
    return this.http.post(//return observable"request sent wait i will notify" not token
      'https://localhost:7113/api/Auth/login',
      data,
      {
        responseType: 'text'
      }
    );
  }
  forgotPassword(data: any)
{
  return this.http.put(
    'https://localhost:7113/api/Auth/forgot-password',
    data,
    {
      responseType: 'text'
    }
  );
}

}
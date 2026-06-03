import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(
    private http: HttpClient
  ) {
  }

  addToCart(data: any)
  {
    const token =
      localStorage.getItem('token');

    const headers =
      new HttpHeaders({
        Authorization:
          `Bearer ${token}`
      });

    return this.http.post(
      'https://localhost:7113/api/cart/add',
      data,
      { headers,
         responseType: 'text'
       }
    );
  }
  getCartItems()
{
  const token =
    localStorage.getItem('token');

  const headers =
    new HttpHeaders({
      Authorization:
        `Bearer ${token}`
    });

  return this.http.get(
    'https://localhost:7113/api/cart',
    { headers }
  );
}
getCartTotal()
{
  const token =
    localStorage.getItem('token');

  const headers =
    new HttpHeaders({
      Authorization:
        `Bearer ${token}`
    });

  return this.http.get(
    'https://localhost:7113/api/cart/total',
    { headers }
  );
}
checkout()
{
  const token =
    localStorage.getItem('token');

  const headers =
    new HttpHeaders({
      Authorization:
        `Bearer ${token}`
    });

  return this.http.post(
    'https://localhost:7113/api/Orders/checkout',
    {},
    {
      headers,
      responseType: 'text'
    });
}
getOrders()
{
  const token =
    localStorage.getItem('token');

  const headers =
    new HttpHeaders({
      Authorization:
        `Bearer ${token}`
    });

  return this.http.get(
    'https://localhost:7113/api/Orders',
    { headers }
  );
}
getOrderDetails(orderId: string)
{
  const token =
    localStorage.getItem('token');

  const headers =
    new HttpHeaders({
      Authorization:
        `Bearer ${token}`
    });

  return this.http.get(
    `https://localhost:7113/api/Orders/${orderId}`,
    { headers }
  );
}
}
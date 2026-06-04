import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpHeaders
} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(
    private http: HttpClient
  ) {
  }

  getProducts()
  {
    return this.http.get(
      'https://localhost:7113/api/products'
    );
  }

  addProduct(data: FormData)
  {
    const token =
      localStorage.getItem('token');

    const headers =
      new HttpHeaders({
        Authorization:
          `Bearer ${token}`
      });

    return this.http.post(
      'https://localhost:7113/api/Products',
      data,
      {
        headers,
        responseType: 'text'
      }
    );
  }

  searchProducts(name: string)
{
  return this.http.get(
    `https://localhost:7113/api/Products/search?name=${name}`
  );
}
  getCategories()
{
  return this.http.get(
    'https://localhost:7113/api/Categories'
  );
}
deleteProduct(id: string)
{
  const token =
    localStorage.getItem('token');

  const headers =
    new HttpHeaders({
      Authorization:
        `Bearer ${token}`
    });

  return this.http.delete(
    `https://localhost:7113/api/Products/${id}`,
    {
      headers,
      responseType: 'text'
    }
  );
}
updateProduct(
  id: string,
  data: any)
{
  const token =
    localStorage.getItem('token');

  console.log('TOKEN:', token);

  const headers =
    new HttpHeaders({
      Authorization:
        `Bearer ${token}`
    });

  return this.http.put(
    `https://localhost:7113/api/Products/${id}`,
    data,
    {
      headers,
      responseType: 'text'
    });
}
}
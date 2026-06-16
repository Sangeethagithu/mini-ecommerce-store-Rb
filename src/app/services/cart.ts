import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(
    private http: HttpClient
  ) {
  }
updateOrderStatus(data: any)
{
  

 return this.http.put(
  'https://localhost:7113/api/Orders/status',
  data,
  {
    responseType: 'text'
  }
);
}
  addToCart(data: any)
  {
    

   return this.http.post(
  'https://localhost:7113/api/cart/add',
  data,
  {
    responseType: 'text'
  }
);
  }
  getCartItems()
{
  return this.http.get(
  'https://localhost:7113/api/cart'
);
}
getCartTotal()
{
  return this.http.get(
  'https://localhost:7113/api/cart/total'
);
}
checkout()
{
  return this.http.post(
  'https://localhost:7113/api/Orders/checkout',
  {},
  {
    responseType: 'text'
  }
);
}
getOrders()
{
  return this.http.get(
  'https://localhost:7113/api/Orders'
);
}
getOrderDetails(orderId: string)
{
 return this.http.get(
  `https://localhost:7113/api/Orders/${orderId}`
);
}
getAllOrdersForAdmin()
{
return this.http.get(
  'https://localhost:7113/api/Orders/admin'
);
}
//dashboard admin
getDashboardStats()
{
  return this.http.get(
  'https://localhost:7113/api/Orders/dashboard'
);
    
}
//recent prod
getRecentOrders()
{
  return this.http.get(
  'https://localhost:7113/api/Orders/recent-orders'
);
}
//update quantity
updateCartQuantity(data: any)
{
  return this.http.put(
  'https://localhost:7113/api/Cart/update',
  data,
  {
    responseType: 'text'
  }
);
}

removeCartItem(cartItemId: string)
{
return this.http.delete(
  `https://localhost:7113/api/Cart/${cartItemId}`,
  {
    responseType: 'text'
  }
);
}
}
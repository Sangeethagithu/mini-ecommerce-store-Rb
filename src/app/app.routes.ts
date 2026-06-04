import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { ProductsComponent } from './components/products/products';
import { CartComponent } from './components/cart/cart';
import { OrdersComponent } from './components/orders/orders';
import { AdminComponent } from './components/admin/admin';
import { AdminOrdersComponent }
from './components/admin-orders/admin-orders';


export const routes: Routes = [
  {
    path: '',
    component: LoginComponent
  },
  {
    path: 'products',
    component: ProductsComponent
  },
  {
  path: 'cart',
  component: CartComponent
},{
   path: 'orders',
  component: OrdersComponent
},{
  path: 'admin',
  component: AdminComponent
},{
  path: 'admin-orders',
  component: AdminOrdersComponent
}
];
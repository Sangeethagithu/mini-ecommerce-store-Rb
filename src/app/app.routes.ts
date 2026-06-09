import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { ProductsComponent } from './components/products/products';
import { CartComponent } from './components/cart/cart';
import { OrdersComponent } from './components/orders/orders';
import { AdminComponent } from './components/admin/admin';
import { AdminOrdersComponent }
from './components/admin-orders/admin-orders';
import { DashboardComponent }
from './components/dashboard/dashboard';
import { ProductDetailsComponent }
from './components/product-details/product-details';
import { RegisterComponent }
from './components/register/register';
import { ForgotPasswordComponent }
from './components/forgot-password/forgot-password';
import { AdminProductsComponent }
from './components/admin-products/admin-products';
export const routes: Routes = [

    {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  {
    path: 'login',
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
},{
  path: 'dashboard',
  component: DashboardComponent
},{
  path: 'product-details/:id',
  component: ProductDetailsComponent
},{
  path: 'register',
  component: RegisterComponent
},{
  path: 'forgot-password',
  component: ForgotPasswordComponent
},{
  path: 'admin-products',
  component: AdminProductsComponent
},{
  path: '**',
  redirectTo: 'login'
}
];
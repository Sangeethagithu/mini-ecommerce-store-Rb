import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartService } from '../../services/cart';
import { ChangeDetectorRef } from '@angular/core';
import { ProductService } from '../../services/product';
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent
implements OnInit {

  dashboard: any = {};lowStockProducts: any[] = [];
recentOrders: any[] = [];
  constructor(
    private cartService: CartService,
     private productService: ProductService,
    private cdr: ChangeDetectorRef
  ) {
  }

  ngOnInit(): void
  {
    this.cartService
      .getDashboardStats()
      .subscribe(
        (response: any) =>
        {
          console.log(response);

          this.dashboard = response;

          this.cdr.detectChanges();
        },
        (error) =>
        {
          console.log(error);
        });


    this.productService
  .getLowStockProducts()
  .subscribe(
    (response: any) =>
    {
      this.lowStockProducts = response;

      this.cdr.detectChanges();
    },
    (error) =>
    {
      console.log(error);
    });


    this.cartService
  .getRecentOrders()
  .subscribe(
    (response: any) =>
    {
      this.recentOrders =
        response;

      this.cdr.detectChanges();
    },
    (error) =>
    {
      console.log(error);
    });
  }
}
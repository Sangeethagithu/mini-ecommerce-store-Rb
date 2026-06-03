import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartService } from '../../services/cart';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './orders.html',
  styleUrl: './orders.css'
})
export class OrdersComponent implements OnInit {

  orders: any[] = [];

  selectedOrderItems: any[] = [];

  constructor(
    private cartService: CartService,
    private cdr: ChangeDetectorRef
  ) {
  }

  ngOnInit(): void {

    this.cartService.getOrders()
      .subscribe(
        (response: any) => {

          console.log(response);

          this.orders = response;

          this.cdr.detectChanges();
        },
        (error) => {

          console.log(error);
        });
  }

  viewDetails(orderId: string)
  {
    this.cartService.getOrderDetails(orderId)
      .subscribe(
        (response: any) =>
        {
          console.log(response);

          this.selectedOrderItems = response;

          this.cdr.detectChanges();
        },
        (error) =>
        {
          console.log(error);
        });
  }
}
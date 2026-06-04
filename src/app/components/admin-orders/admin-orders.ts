import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CartService } from '../../services/cart';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-admin-orders',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './admin-orders.html',
  styleUrl: './admin-orders.css'
})
export class AdminOrdersComponent implements OnInit {

  orders: any[] = [];

  constructor(
    private cartService: CartService,
    private cdr: ChangeDetectorRef
  ) {
  }

  ngOnInit(): void
  {
    this.loadOrders();
  }

  loadOrders()
  {
      this.cartService.getAllOrdersForAdmin()
      .subscribe(
        (response: any) =>
        {
          this.orders = response;

          this.cdr.detectChanges();
        },
        (error) =>
        {
          console.log(error);
        });
  }
  

  updateStatus(order: any)
  {
    const data =
    {
      orderId: order.id,
      status: order.status
    };

    this.cartService
      .updateOrderStatus(data)
      .subscribe(
        (response) =>
        {
          alert(response);

          this.loadOrders();

          this.cdr.detectChanges();
        },
        (error) =>
        {
          console.log(error);

          alert('Update failed');
        });
  }
}
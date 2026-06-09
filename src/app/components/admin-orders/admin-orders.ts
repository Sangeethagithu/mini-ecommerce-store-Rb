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
totalOrders = 0;
pendingOrders = 0;
processingOrders = 0;
shippedOrders = 0;
deliveredOrders = 0;
cancelledOrders = 0;
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

this.totalOrders = this.orders.length;

this.pendingOrders =
  this.orders.filter(
    x => x.status === 'Pending'
  ).length;

this.processingOrders =
  this.orders.filter(
    x => x.status === 'Processing'
  ).length;

this.shippedOrders =
  this.orders.filter(
    x => x.status === 'Shipped'
  ).length;

this.deliveredOrders =
  this.orders.filter(
    x => x.status === 'Delivered'
  ).length;

this.cancelledOrders =
  this.orders.filter(
    x => x.status === 'Cancelled'
  ).length;

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
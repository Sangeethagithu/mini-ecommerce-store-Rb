import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartService } from '../../services/cart';
import { ChangeDetectorRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { OrderDetailsDialogComponent }
from '../order-details-dialog/order-details-dialog';
@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './orders.html',
  styleUrl: './orders.css'
})
export class OrdersComponent implements OnInit {

  orders: any[] = [];

  

  constructor(
    private cartService: CartService,
    private cdr: ChangeDetectorRef,
     private dialog: MatDialog
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
    this.dialog.open(
      OrderDetailsDialogComponent,
      {
        width: '700px',

        data:
        {
          orderId: orderId
        }
      });
}
}
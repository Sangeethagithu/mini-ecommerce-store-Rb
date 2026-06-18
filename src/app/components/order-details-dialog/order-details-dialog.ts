import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MAT_DIALOG_DATA,
  MatDialogRef
} from '@angular/material/dialog';

import { CartService } from '../../services/cart';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-order-details-dialog',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './order-details-dialog.html',
  styleUrl: './order-details-dialog.css'
})
export class OrderDetailsDialogComponent
implements OnInit
{

  orderItems: any[] = [];

  constructor(
    @Inject(MAT_DIALOG_DATA)
    public data: any,

    private dialogRef:
      MatDialogRef<OrderDetailsDialogComponent>,

    private cartService: CartService,

    private cdr: ChangeDetectorRef
  )
  {
  }

  ngOnInit(): void
  {

    this.loadOrderDetails();

  }

  loadOrderDetails()
  {

    this.cartService
      .getOrderDetails(
        this.data.orderId
      )
      .subscribe({

        next: (response: any) =>
        {

          this.orderItems = response;

          this.cdr.detectChanges();

        },

        error: (error) =>
        {

          console.log(error);

        }

      });

  }

  closeDialog()
  {

    this.dialogRef.close();

  }

}
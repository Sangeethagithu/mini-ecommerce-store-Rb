import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartService } from '../../services/cart';
import { ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { NotificationService } from '../../services/notification';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart.html',
  styleUrl: './cart.css'
})






export class CartComponent implements OnInit {

  items: any[] = [];
  total: number = 0;

  constructor(private router: Router,
    private cartService: CartService,
    private cdr: ChangeDetectorRef,
     private notification: NotificationService,
       private dialog: MatDialog
  ) {
  }

  ngOnInit(): void {

    this.cartService.getCartItems()
      .subscribe(
        (response: any) => {

          console.log(response);

          this.items = response;

          this.cdr.detectChanges();

          this.cartService.getCartTotal()
            .subscribe(
              (response: any) => {

                console.log('Cart Total:', response);

                this.total = response;

                this.cdr.detectChanges();
              },
              (error) => {

                console.log(error);
              });
        },
        (error) => {

          console.log(error);
        });
  }
goToProducts() {
  this.router.navigate(['/products']);
}
goToOrders() {
  this.router.navigate(['/orders']);
}

  checkout()
  {
    this.cartService.checkout()
      .subscribe(
        (response: any) =>
        {
          this.notification.success(response);

          this.items = [];

          this.total = 0;

          this.cdr.detectChanges();
        },
        (error) =>
        {
          console.log(error);

          this.notification.error(
  'Checkout failed'
);
        });
  }
  increaseQuantity(item: any)
{
  const data =
  {
    cartItemId:
      item.cartItemId,

    quantity:
      item.quantity + 1
  };

  this.cartService
    .updateCartQuantity(data)
    .subscribe(
      () =>
      {
        this.ngOnInit();
        this.notification.success(
  'Quantity Updated'
);
      },
      (error) =>
      {
        console.log(error);
      });
}
decreaseQuantity(item: any)
{
  if (item.quantity <= 1)
  {
    return;
  }

  const data =
  {
    cartItemId:
      item.cartItemId,

    quantity:
      item.quantity - 1
  };

  this.cartService
    .updateCartQuantity(data)
    .subscribe(
      () =>
      {
        this.ngOnInit();
        this.notification.success(
  'Quantity Updated'
);
      },
      (error) =>
      {
        console.log(error);
      });
}
removeItem(item: any)
{

  const dialogRef = this.dialog.open(
    ConfirmationDialogComponent,
    {
      width: '350px',

      data:
      {
        title: 'Remove Item',

        message: 'Remove this item from your cart?'
      }
    });

  dialogRef.afterClosed()
    .subscribe(result =>
    {

      if(result)
      {

        this.cartService
          .removeCartItem(item.cartItemId)
          .subscribe({

            next: () =>
            {

              this.notification.success(
                'Item removed from cart'
              );

              this.ngOnInit();

            },

            error: error =>
            {

              console.log(error);

              this.notification.error(
                'Failed to remove item'
              );

            }

          });

      }

    });

}
}
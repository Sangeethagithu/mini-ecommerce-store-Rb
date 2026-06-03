import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartService } from '../../services/cart';
import { ChangeDetectorRef } from '@angular/core';

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

  constructor(
    private cartService: CartService,
    private cdr: ChangeDetectorRef
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

  checkout()
  {
    this.cartService.checkout()
      .subscribe(
        (response: any) =>
        {
          alert(response);

          this.items = [];

          this.total = 0;

          this.cdr.detectChanges();
        },
        (error) =>
        {
          console.log(error);

          alert('Checkout failed');
        });
  }
}
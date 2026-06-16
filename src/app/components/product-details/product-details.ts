import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
import { ProductService } from '../../services/product';
import { CartService } from '../../services/cart';
import { NotificationService } from '../../services/notification';
@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-details.html',
  styleUrl: './product-details.css'
})
export class ProductDetailsComponent
implements OnInit {

  product: any;

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private cartService: CartService,
    private cdr: ChangeDetectorRef, private notification: NotificationService
  )
  {
  }

  ngOnInit(): void
  {
    const id =
      this.route.snapshot.paramMap.get('id');

    this.productService
      .getProductById(id!)
      .subscribe(
        (response: any) =>
        {
          this.product = response;this.cdr.detectChanges();
        },
        (error:any) =>
        {
          console.log(error);
        });


    
  }

  addToCart()
{
  const data =
  {
    productId: this.product.id,
    quantity: 1
  };

  this.cartService
    .addToCart(data)
    .subscribe({

      next: () =>
      {
        this.notification.success(
          'Product added to cart.'
        );
      },

      error: (error: any) =>
      {
        console.log(error);

        this.notification.error(
          'Failed to add product.'
        );
      }

    });

}
}
//Product Componenet
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/product';
import { CartService } from '../../services/cart';
import {

  ChangeDetectorRef
} from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-products',
  standalone: true,
   imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class ProductsComponent implements OnInit {

  products: any[] = [];

 constructor(
  private productService: ProductService,
  private cartService: CartService,
  private cdr: ChangeDetectorRef
) {}

ngOnInit(): void {

  console.log('ngOnInit called');
  console.log('Instance in ngOnInit:', this);

  this.productService.getProducts()
    .subscribe({
  next: (response: any) => {

  this.products = response;

  console.log('Products assigned');
  console.log('Length:', this.products.length);

  this.cdr.detectChanges();
},
      error: (error) => {
        console.log(error);
      }
    });
}

checkProducts() {
  console.log('Button Clicked');
  console.log('Current Length:', this.products.length);
  console.log('Current Products:', this.products);
}


  addToCart(productId: string)
{
  const data =
  {
    productId: productId,
    quantity: 1
  };

  this.cartService.addToCart(data)
    .subscribe(
      () =>
      {
        alert('Product added to cart');
      },
      (error) =>
      {
        console.log(error);

        alert('Failed to add product');
      });
}

get productCount() {
  console.log('Getter called, length =', this.products.length);
  return this.products.length;
}
}
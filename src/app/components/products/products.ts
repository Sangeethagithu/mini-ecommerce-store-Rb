//Product Componenet
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/product';
import { CartService } from '../../services/cart';
import {

  ChangeDetectorRef
} from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-products',
  standalone: true,
   imports: [
    CommonModule,
    RouterModule, FormsModule
  ],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class ProductsComponent implements OnInit {

  products: any[] = [];
searchText = '';
suggestions: any[] = [];
categories: any[] = [];

selectedCategory = '';
allProducts: any[] = [];

minPrice = 0;
maxPrice = 99999999;




 constructor(
  private productService: ProductService,
  private cartService: CartService,
  private cdr: ChangeDetectorRef
) {}

ngOnInit(): void {

  console.log('ngOnInit called');
  console.log('Instance in ngOnInit:', this);
  this.loadProducts();
  this.loadCategories();
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
loadCategories()
{
  this.productService
    .getCategories()
    .subscribe(
      (response: any) =>
      {
        this.categories = response;
        this.cdr.detectChanges();
      },
      error =>
      {
        console.log(error);
      });
}
loadProducts()
{
  this.productService
    .getProducts()
    .subscribe(
      (response: any) =>
      {
        this.allProducts = response;
        this.products = response;

        this.cdr.detectChanges();
      },
      (error) =>
      {
        console.log(error);
      });
}

activePriceFilter = 'All Prices';
setPriceRange(
  min: number,
  max: number,
  label: string)
{
  this.minPrice = min;
  this.maxPrice = max;

  this.activePriceFilter = label;

  this.applyFilters();
}






applyFilters()
{
  let filtered = this.allProducts;

  // Category Filter
  if (this.selectedCategory)
  {
    filtered = filtered.filter(
      p => p.categoryId === this.selectedCategory
    );
  }

  // Price Filter
  filtered = filtered.filter(
    p =>
      p.price >= this.minPrice &&
      p.price <= this.maxPrice
  );

  this.products = filtered;

  this.cdr.detectChanges();
}

filterByCategory(categoryId: string)
{
  this.selectedCategory = categoryId;

  this.applyFilters();
}
searchProducts()
{
  if (!this.searchText.trim())
  {
    this.loadProducts();

    this.suggestions = [];

    return;
  }

  this.productService
    .searchProducts(this.searchText)
    .subscribe(
      (response: any) =>
      {
        this.products = response;

        this.suggestions = response;

        this.cdr.detectChanges();
      },
      (error) =>
      {
        console.log(error);
      });
}
selectProduct(product: any)
{
  this.searchText =
    product.name;

  this.suggestions = [];

  this.products = [product];

  this.cdr.detectChanges();
}
checkProducts() {
  console.log('Button Clicked');
  console.log('Current Length:', this.products.length);
  console.log('Current Products:', this.products);
}

logout()
{
  localStorage.removeItem('token');

  location.href = '/login';
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
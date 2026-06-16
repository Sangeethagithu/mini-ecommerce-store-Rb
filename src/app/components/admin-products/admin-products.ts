import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChangeDetectorRef } from '@angular/core';

import { MatDialog } from '@angular/material/dialog';

import { ProductService } from '../../services/product';
import { NotificationService } from '../../services/notification';

import { AddEditProductDialogComponent }
from '../add-edit-product-dialog/add-edit-product-dialog';

@Component({
  selector: 'app-admin-products',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './admin-products.html',
  styleUrl: './admin-products.css'
})
export class AdminProductsComponent
implements OnInit {

  products: any[] = [];

  categories: any[] = [];

  searchText = '';

  suggestions: any[] = [];

  constructor(
    private productService: ProductService,
    private notification: NotificationService,
    private dialog: MatDialog,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {

    this.loadProducts();

    this.loadCategories();

  }

  loadProducts()
  {
    this.productService
      .getProducts()
      .subscribe({

        next: (response: any) =>
        {
          this.products = response;

          this.cdr.detectChanges();
        },

        error: (error) =>
        {
          console.log(error);

          this.notification.error(
            'Failed to load products'
          );
        }

      });
  }

  loadCategories()
  {
    this.productService
      .getCategories()
      .subscribe({

        next: (response: any) =>
        {
          this.categories = response;
        },

        error: (error) =>
        {
          console.log(error);

          this.notification.error(
            'Failed to load categories'
          );
        }

      });
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
      .subscribe({

        next: (response: any) =>
        {
          this.products = response;

          this.suggestions = response;

          this.cdr.detectChanges();
        },

        error: (error) =>
        {
          console.log(error);
        }

      });
  }

  selectProduct(product: any)
  {
    this.searchText = product.name;

    this.suggestions = [];

    this.products = [product];
  }

  addProduct()
  {
    const dialogRef =
      this.dialog.open(
        AddEditProductDialogComponent,
        {
          width: '700px',

          disableClose: true,

          data:
          {
            mode: 'add',

            categories: this.categories
          }
        });

    dialogRef.afterClosed()
      .subscribe(result =>
      {
        if(result)
        {
          this.loadProducts();
        }
      });
  }

  editProduct(product: any)
  {
    const dialogRef =
      this.dialog.open(
        AddEditProductDialogComponent,
        {
          width: '700px',

          disableClose: true,

          data:
          {
            mode: 'edit',

            product: product,

            categories: this.categories
          }
        });

    dialogRef.afterClosed()
      .subscribe(result =>
      {
        if(result)
        {
          this.loadProducts();
        }
      });
  }

  deleteProduct(id: string)
  {
    if(!confirm(
      'Delete this product?'))
    {
      return;
    }

    this.productService
      .deleteProduct(id)
      .subscribe({

        next: () =>
        {
          this.notification.success(
            'Product Deleted Successfully'
          );

          this.loadProducts();
        },

        error: (error) =>
        {
          console.log(error);

          this.notification.error(
            'Failed to delete product'
          );
        }

      });
  }

}
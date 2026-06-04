import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../services/product';
import { OnInit } from '@angular/core';

import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';


@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [FormsModule  , CommonModule
],
  templateUrl: './admin.html',
  styleUrl: './admin.css'
})
export class AdminComponent implements OnInit{

  name = '';

  description = '';

  price = 0;

  stockQuantity = 0;

  categoryId = '';

  imageFile: any;
  imageUrl: string = '';
categories: any[] = [];
products: any[] = [];
searchText: string = '';
selectedProductId: string = '';
  constructor(
    private productService: ProductService,  private cdr: ChangeDetectorRef
  ) {
  }

  onFileSelected(event: any)
  {
    this.imageFile =
      event.target.files[0];
  }

  addProduct()
  {
    const formData =
      new FormData();

    formData.append(
      'name',
      this.name);

    formData.append(
      'description',
      this.description);

    formData.append(
      'price',
      this.price.toString());

    formData.append(
      'stockQuantity',
      this.stockQuantity.toString());

    formData.append(
      'categoryId',
      this.categoryId);

    formData.append(
      'image',
      this.imageFile);

    this.productService
      .addProduct(formData)
      .subscribe(
        (response) =>
        {
          alert(response);
          this.loadProducts();this.clearForm();
        },
        (error) =>
        {
          console.log(error);

          alert('Failed');
        });
  }
editProduct(product: any)
{
    alert('Edit clicked');

  console.log(product);
  this.selectedProductId =
    product.id;

  this.name =
    product.name;

  this.description =
    product.description;

  this.price =
    product.price;

  this.stockQuantity =
    product.stockQuantity;

  this.categoryId =
    product.categoryId;

  this.imageUrl =
    product.imageUrl;
 

  this.cdr.detectChanges();
}
clearForm()
{
  this.selectedProductId = '';

  this.name = '';

  this.description = '';

  this.price = 0;

  this.stockQuantity = 0;

  this.categoryId = '';

  this.imageUrl = '';
}
updateProduct()
{
  const data =
  {
    name: this.name,
    description: this.description,
    price: this.price,
    stockQuantity: this.stockQuantity,
    imageUrl: this.imageUrl,
    categoryId: this.categoryId
  };

  console.log('UPDATE DATA');
  console.log(data);

  this.productService
    .updateProduct(
      this.selectedProductId,
      data)
    .subscribe(
      (response) =>
      {
        alert(response);

        this.loadProducts();

        this.clearForm();
      },
      (error) =>
      {
        console.log(error);

        alert('Update failed');
      });
}
deleteProduct(id: string)
{
  if (!confirm('Delete product?'))
  {
    return;
  }

  this.productService
    .deleteProduct(id)
    .subscribe(
      (response) =>
      {
        alert(response);

        this.loadProducts();
      },
      (error) =>
      {
        console.log(error);

        alert('Delete failed');
      });
}


searchProducts()
{
  if (!this.searchText)
  {
    this.loadProducts();

    return;
  }

  this.productService
    .searchProducts(this.searchText)
    .subscribe(
      (response: any) =>
      {
        this.products = response;

        this.cdr.detectChanges();
      },
      (error) =>
      {
        console.log(error);
      });
}


  ngOnInit(): void
{
  this.productService
    .getCategories()
    .subscribe(
      (response: any) =>
      {
        this.categories = response;

        this.cdr.detectChanges();
      },
      (error) =>
      {
        console.log(error);
      });

  this.loadProducts();
}
loadProducts()
{  console.log('Loading Orders...');


  this.productService.getProducts()
    .subscribe(
      (response: any) =>
      {
        this.products = response;

        this.cdr.detectChanges();
      },
      (error) =>
      {
        console.log(error); console.log('Orders Error:');
        
      });
}
}
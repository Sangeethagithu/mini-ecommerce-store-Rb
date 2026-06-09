import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/product';
import { ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-admin-products',
  standalone: true,
  imports: [CommonModule,  FormsModule],
  templateUrl: './admin-products.html',
  styleUrl: './admin-products.css',
  
})
export class AdminProductsComponent
implements OnInit
{
  products: any[] = [];

  constructor(
    private productService: ProductService,  private cdr: ChangeDetectorRef
  )
  {
  }

  newProduct =
{
  name: '',
  description: '',
  price: 0,
  stockQuantity: 0,
  categoryId: '',
    image: null as File | null

};

showAddForm = false;

  ngOnInit(): void
  {
    this.loadProducts();
  }

  loadProducts()
  {
    this.productService
      .getProducts()
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
      
  
addProduct()
{
  this.showAddForm = true;
}

onFileSelected(event: any)
{
  this.newProduct.image =
    event.target.files[0];
}

saveProduct()
{
  const formData =
    new FormData();

  formData.append(
    'name',
    this.newProduct.name);

  formData.append(
    'description',
    this.newProduct.description);

  formData.append(
    'price',
    this.newProduct.price.toString());

  formData.append(
    'stockQuantity',
    this.newProduct.stockQuantity.toString());

  formData.append(
    'categoryId',
    this.newProduct.categoryId);

 if (this.newProduct.image) {
  formData.append(
    'image',
    this.newProduct.image
  );
}

  this.productService
    .addProduct(formData)
    .subscribe(
      () =>
      {
        alert('Product Added');

        this.showAddForm = false;

        this.loadProducts();
      },
      error =>
      {
        console.log(error);
      });
}
selectedProduct: any = null;

editProduct(product: any)
{
  this.selectedProduct =
  {
    ...product
  };
}

updateProduct()
{
  this.productService
    .updateProduct(
      this.selectedProduct.id,
      this.selectedProduct
    )
    .subscribe(
      () =>
      {
        alert(
          'Product Updated'
        );

        this.selectedProduct = null;

        this.loadProducts();
      },
      error =>
      {
        console.log(error);
      }
    );
}
  deleteProduct(id: string)
  {
    if (!confirm(
      'Delete this product?'))
    {
      return;
    }

    this.productService
      .deleteProduct(id)
      .subscribe(
        () =>
        {
          alert(
            'Product deleted');

          this.loadProducts();
        },
        (error) =>
        {
          console.log(error);
        });
  }
}
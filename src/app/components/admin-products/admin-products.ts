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
  categories: any[] = [];
  selectedImage: File | null = null;
  searchText = '';
  suggestions: any[] = [];

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
    this.loadProducts(); this.loadCategories();
  }
loadCategories()
{
  this.productService
      .getCategories()
      .subscribe(
      (response:any) =>
      {
          this.categories = response;
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
          this.products = response;
      this.cdr.detectChanges();
        },
        (error) =>
        {
          console.log(error);
        });

      }
      onEditFileSelected(event: any)
{
  this.selectedImage =
    event.target.files[0];
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
      },
      error =>
      {
          console.log(error);
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
  this.showAddForm = true;
}

onFileSelected(event: any)
{
  this.newProduct.image =
    event.target.files[0];
}

saveProduct()
{

  if (this.newProduct.stockQuantity < 1) {
  alert('Stock quantity must be at least 1');
  return;
}
  if (!this.newProduct.categoryId) {
    alert('Please select a category');
    return;
  }

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
  if (this.selectedProduct.stockQuantity < 1)
  {
    alert('Stock quantity must be at least 1');
    return;
  }

  const formData =
    new FormData();

  formData.append(
    'name',
    this.selectedProduct.name);

  formData.append(
    'description',
    this.selectedProduct.description);

  formData.append(
    'price',
    this.selectedProduct.price.toString());

  formData.append(
    'stockQuantity',
    this.selectedProduct.stockQuantity.toString());

  formData.append(
    'categoryId',
    this.selectedProduct.categoryId);

  if (this.selectedImage)
  {
    formData.append(
      'image',
      this.selectedImage);
  }

  this.productService
    .updateProduct(
      this.selectedProduct.id,
      formData
    )
    .subscribe(
      () =>
      {
        alert('Product Updated');

        this.selectedProduct = null;

        this.loadProducts();
      },
      error =>
      {
        console.log(error);
      });
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
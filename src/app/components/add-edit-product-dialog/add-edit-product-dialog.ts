import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef
} from '@angular/material/dialog';

import { ProductService } from '../../services/product';
import { NotificationService } from '../../services/notification';

@Component({
  selector: 'app-add-edit-product-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule
  ],
  templateUrl: './add-edit-product-dialog.html',
  styleUrl: './add-edit-product-dialog.css'
})
export class AddEditProductDialogComponent
implements OnInit {

  productForm!: FormGroup;

  selectedImage: File | null = null;

  isEditMode = false;

  categories: any[] = [];
  currentImage = '';

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private notification: NotificationService,

    public dialogRef:
      MatDialogRef<AddEditProductDialogComponent>,

    @Inject(MAT_DIALOG_DATA)
    public data: any
  )
  {
  }

  ngOnInit(): void
  {

    this.categories =
      this.data.categories;

    this.isEditMode =
      this.data.mode === 'edit';
      if (this.isEditMode)
{
  this.currentImage =
    this.data.product.imageUrl;
}

    this.productForm =
      this.fb.group({

        name: [
          '',
          Validators.required
        ],

        description: [
          '',
          Validators.required
        ],

        price: [
          0,
          [
            Validators.required,
            Validators.min(1)
          ]
        ],

        stockQuantity: [
          1,
          [
            Validators.required,
            Validators.min(1)
          ]
        ],

        categoryId: [
          '',
          Validators.required
        ]

      });

    if(this.isEditMode)
    {
      this.productForm.patchValue({

        name:
          this.data.product.name,

        description:
          this.data.product.description,

        price:
          this.data.product.price,

        stockQuantity:
          this.data.product.stockQuantity,

        categoryId:
          this.data.product.categoryId

      });
    }

  }

  onFileSelected(event: any)
  {
    if(event.target.files.length > 0)
    {
      this.selectedImage =
        event.target.files[0];
    }
  }

  closeDialog()
  {
    this.dialogRef.close(false);
  }
  saveProduct()
{

  if (this.productForm.invalid)
  {
    this.productForm.markAllAsTouched();

    return;
  }

  const formData =
    new FormData();

  formData.append(
    'name',
    this.productForm.value.name
  );

  formData.append(
    'description',
    this.productForm.value.description
  );

  formData.append(
    'price',
    this.productForm.value.price.toString()
  );

  formData.append(
    'stockQuantity',
    this.productForm.value.stockQuantity.toString()
  );

  formData.append(
    'categoryId',
    this.productForm.value.categoryId
  );

  if (this.selectedImage)
  {
    formData.append(
      'image',
      this.selectedImage
    );
  }

  if (this.isEditMode)
  {

    this.productService
      .updateProduct(
        this.data.product.id,
        formData
      )
      .subscribe({

        next: () =>
        {

          this.notification.success(
            'Product Updated Successfully'
          );

          this.dialogRef.close(true);

        },

        error: (error) =>
        {

          console.log(error);

          this.notification.error(
            'Failed to update product'
          );

        }

      });

  }
  else
  {

    this.productService
      .addProduct(formData)
      .subscribe({

        next: () =>
        {

          this.notification.success(
            'Product Added Successfully'
          );

          this.dialogRef.close(true);

        },

        error: (error) =>
        {

          console.log(error);

          this.notification.error(
            'Failed to add product'
          );

        }

      });

  }

}
}
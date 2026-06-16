import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../services/product';
import { ChangeDetectorRef } from '@angular/core';
import { NotificationService } from '../../services/notification';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog';
@Component({
  selector: 'app-admin-categories',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './admin-categories.html',
  styleUrl: './admin-categories.css'
})
export class AdminCategoriesComponent
implements OnInit
{
  categories: any[] = [];

  showAddForm = false;

  newCategory =
  {
    name: ''
  };

  selectedCategory: any = null;

  constructor(
    private productService: ProductService,  private cdr: ChangeDetectorRef,
    private notification: NotificationService,
     private dialog: MatDialog
  ) {}

  ngOnInit(): void
  {
    this.loadCategories();
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

  saveCategory()
  {
    this.productService
      .addCategory(this.newCategory)
      .subscribe(
        () =>
        {
           this.notification.success(
          'Category Added Successfully'
        )

          this.newCategory =
          {
            name: ''
          };

          this.showAddForm = false;

          this.loadCategories();
        });
  }

  editCategory(category: any)
  {
    this.selectedCategory =
    {
      ...category
    };
  }

  updateCategory()
  {
    this.productService
      .updateCategory(
        this.selectedCategory.id,
        this.selectedCategory
      )
      .subscribe(
        () =>
        {
            this.notification.success(
          'Category Updated Successfully'
        );

          this.selectedCategory = null;

          this.loadCategories();
        });
  }

  deleteCategory(id: string)
{

  const dialogRef = this.dialog.open(
    ConfirmationDialogComponent,
    {
      width: '350px',

      data:
      {
        title: 'Delete Category',

        message: 'Are you sure you want to delete this category?'
      }
    });

  dialogRef.afterClosed()
    .subscribe(result =>
    {

      if(result)
      {

        this.productService
          .deleteCategory(id)
          .subscribe({

            next: () =>
            {

              this.notification.success(
                'Category Deleted Successfully'
              );

              this.loadCategories();

            },

            error: error =>
            {

              console.log(error);

              this.notification.error(
                'Failed to delete category'
              );

            }

          });

      }

    });

}
}
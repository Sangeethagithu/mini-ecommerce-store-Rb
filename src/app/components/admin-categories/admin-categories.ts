import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../services/product';
import { ChangeDetectorRef } from '@angular/core';
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
    private productService: ProductService,  private cdr: ChangeDetectorRef
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
          alert('Category Added');

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
          alert('Category Updated');

          this.selectedCategory = null;

          this.loadCategories();
        });
  }

  deleteCategory(id: string)
  {
    if (!confirm('Delete category?'))
    {
      return;
    }

    this.productService
      .deleteCategory(id)
      .subscribe(
        () =>
        {
          alert('Category Deleted');

          this.loadCategories();
        });
  }
}
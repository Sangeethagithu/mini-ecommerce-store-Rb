import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-admin-sidebar',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './admin-sidebar.html',
  styleUrl: './admin-sidebar.css'
})
export class AdminSidebarComponent
{
  constructor(
    private router: Router)
  {
  }

  logout()
  {
    localStorage.removeItem(
      'token');

    this.router.navigate(
      ['/login']);
  }
}
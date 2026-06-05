import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [
    FormsModule,
    RouterModule
  ],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.css'
})
export class ForgotPasswordComponent {

  email = '';

  newPassword = '';

  showPassword = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
  }

  togglePassword()
  {
    this.showPassword =
      !this.showPassword;
  }

  resetPassword()
  {
    if (!this.email ||
        !this.newPassword)
    {
      alert(
        'Please fill all fields');

      return;
    }

    const emailPattern =
      /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!emailPattern.test(this.email))
    {
      alert(
        'Please enter valid email');

      return;
    }

    const data =
    {
      email: this.email,
      newPassword:
        this.newPassword
    };

    this.authService
      .forgotPassword(data)
      .subscribe(
        (response) =>
        {
          alert(response);

          this.router.navigate(
            ['/login']);
        },
        (error) =>
        {
          console.log(error);

          alert(error.error);
        });
  }
}
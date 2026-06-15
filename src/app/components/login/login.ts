import { Component, OnInit } from '@angular/core';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent implements OnInit {

  loginForm!: FormGroup;

  showPassword = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[^\s@]+@[^\s@]+\.[^\s@]+$/)
        ]
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(6)
        ]
      ]
    });
  }

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  login() {

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const data = this.loginForm.value;

    this.authService.login(data).subscribe({
      next: (response: any) => {

        localStorage.setItem(
          'token',
          response.accessToken
        );

        localStorage.setItem(
          'refreshToken',
          response.refreshToken
        );

        const payload = JSON.parse(
          atob(response.accessToken.split('.')[1])
        );

        const role =
          payload[
            'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
          ];

        console.log(role);

        if (role === 'Admin') {
          this.router.navigate(['/dashboard']);
        }
        else {
          this.router.navigate(['/products']);
        }

      },

      error: (error) => {
        console.log(error);

        alert('Invalid Email or Password');
      }
    });

  }

}
import { Component, OnInit } from '@angular/core';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth';
import { NotificationService }
from '../../services/notification';
import {
  tap,
  catchError,
  finalize
} from 'rxjs/operators';

import {
  throwError
} from 'rxjs';
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
    private router: Router,private notification: NotificationService
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

  login()
{

  if (this.loginForm.invalid)
  {
    this.loginForm.markAllAsTouched();

    this.notification.error(
      'Please correct the highlighted errors.'
    );

    return;
  }

  const data =
    this.loginForm.value;

  this.authService
    .login(data)

    .pipe(

      tap((response: any) =>
      {

        localStorage.setItem(
          'token',
          response.accessToken
        );

        localStorage.setItem(
          'refreshToken',
          response.refreshToken
        );

        const payload =
          JSON.parse(
            atob(
              response.accessToken.split('.')[1]
            )
          );

        const role =
          payload[
          'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
          ];

        this.notification.success(
          'Login Successful'
        );

        if(role === 'Admin')
        {
          this.router.navigate(
            ['/dashboard']
          );
        }
        else
        {
          this.router.navigate(
            ['/products']
          );
        }

      }),

      catchError(error =>
      {

        console.log(error);

        this.notification.error(
          'Invalid Email or Password'
        );

        return throwError(() => error);

      }),

      finalize(() =>
      {

        console.log(
          'Login request completed.'
        );

      })

    )

    .subscribe();

}

}
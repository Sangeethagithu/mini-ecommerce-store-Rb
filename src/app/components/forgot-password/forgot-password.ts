import { Component, OnInit } from '@angular/core';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

import { AuthService } from '../../services/auth';
import { EmailValidator } from '../../validators/email.validator';
import { PasswordValidator } from '../../validators/password.validator';
import { passwordMatchValidator } from '../../validators/confirm-password.validator';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterModule
  ],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.css'
})
export class ForgotPasswordComponent implements OnInit {

  forgotPasswordForm!: FormGroup;

  showPassword = false;
  showConfirmPassword = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {

    this.forgotPasswordForm = this.fb.group({

      email: [
        '',
        EmailValidator.emailValidators
      ],

      newPassword: [
        '',
        PasswordValidator.passwordValidators
      ],

      confirmPassword: [
        '',
        Validators.required
      ]

    },
    {
      validators: passwordMatchValidator()
    });

  }

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPassword() {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  resetPassword() {

    if (this.forgotPasswordForm.invalid) {

      this.forgotPasswordForm.markAllAsTouched();

      return;

    }

    const data = {
      email: this.forgotPasswordForm.value.email,
      newPassword: this.forgotPasswordForm.value.newPassword
    };

    this.authService
      .forgotPassword(data)
      .subscribe({

        next: (response) => {

          alert(response);

          this.router.navigate(['/login']);

        },

        error: (error) => {

          console.log(error);

          alert(error.error);

        }

      });

  }

}
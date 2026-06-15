import { Component, OnInit } from '@angular/core';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { passwordMatchValidator } from '../../validators/confirm-password.validator';
import { AuthService } from '../../services/auth';
import { EmailValidator } from '../../validators/email.validator';
import { PasswordValidator } from '../../validators/password.validator';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterModule
  ],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class RegisterComponent implements OnInit {

  registerForm!: FormGroup;

  showPassword = false;
showConfirmPassword = false;
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}
ngOnInit(): void {

  this.registerForm = this.fb.group({

    name: [
      '',
      [
        Validators.required,
        Validators.minLength(3)
      ]
    ],

    email: [
      '',
      EmailValidator.emailValidators
    ],

    password: [
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

  register() {

    if (this.registerForm.invalid) {

      this.registerForm.markAllAsTouched();

      return;

    }

    const data = this.registerForm.value;

    this.authService
      .register(data)
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
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class RegisterComponent {

  name = '';

  email = '';

  password = '';

  showPassword = false;

  constructor(
    private authService: AuthService,
    private router: Router
  )
  {
  }

  register()
  {
    if (!this.name ||
    !this.email ||
    !this.password)
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
      'Please enter a valid email');

    return;
}
    const data =
    {
      name: this.name,
      email: this.email,
      password: this.password
    };

    this.authService
      .register(data)
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

          alert(
            error.error);
        });
  }

  togglePassword()
  {
    this.showPassword =
      !this.showPassword;
  }
}
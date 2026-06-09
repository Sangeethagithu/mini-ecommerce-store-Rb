import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule,RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {

  email: string = '';

  password: string = '';

  constructor(
    private authService: AuthService ,//need a authservice ibject
    private router: Router
  ) {
  }
showPassword = false;
togglePassword()
{
  this.showPassword =
    !this.showPassword;
}

 login()
{
  if (!this.email || !this.password)
  {
    alert(
      'Please enter email and password');

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
    email: this.email,
    password: this.password
  };

  this.authService.login(data)
    .subscribe(
      (response: any) =>
      {
       localStorage.setItem(
  'token',
  response);

const payload =
  JSON.parse(
    atob(
      response.split('.')[1]
    ));

const role =
  payload[
    'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
  ];

console.log('ROLE:', role);

if (role === 'Admin')
{
  this.router.navigate(
    ['/dashboard']);
}
else
{
  this.router.navigate(
    ['/products']);
}
      },
      (error) =>
      {
        console.log(error);

        alert(
          'Invalid Email or Password');
      });
}

}
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth';
import { Router } from '@angular/router';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
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

  login() {

    const data = {     //create obj
      email: this.email,
      password: this.password
    };

    this.authService.login(data)
      .subscribe(
        (response: any) => {

          console.log(response);

          localStorage.setItem(
            'token',
            response
          );

          this.router.navigate(['/products']);
        },
        (error) => {

          console.log(error);

          alert('Invalid Email or Password');
        }
      );
  }
}
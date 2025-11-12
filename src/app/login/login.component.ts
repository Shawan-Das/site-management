import { Component } from '@angular/core';
import { AuthService } from '../service/auth.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  showPassword = false;

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    this.authService.login(this.username, this.password)
      .subscribe(() => {
        this.router.navigate(['/home']);
      }, error => {
        Swal.fire('Login failed', 'Invalid username or password', 'error');
      });
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }
}

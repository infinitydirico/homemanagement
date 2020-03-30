import { Component } from '@angular/core';
import { AuthService } from '../auth/authentication.service';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  username:string;
  password:string;
  remember: boolean = false;
  isLoading: boolean = false;

  constructor(private authenticationService: AuthService,
    private snackBar: MatSnackBar) {    
  }

  login(){
    this.isLoading = true;
    this.authenticationService.login(this.username, this.password, this.remember).subscribe(res => {
      this.isLoading = false;
    }, err => {
      this.isLoading = false;
      this.snackBar.open('An error happened while authenticating', 'ok', {
        duration : 2000
      })
    });
  }
}

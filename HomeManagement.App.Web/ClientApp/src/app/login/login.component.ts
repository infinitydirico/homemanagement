import { Component } from '@angular/core';
import { AuthService } from '../auth/authentication.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  username:string;
  password:string;
  isLoading: boolean = false;

  constructor(private authenticationService: AuthService) {    
  }

  login(){
    this.isLoading = true;
    this.authenticationService.login(this.username, this.password);
  }
}

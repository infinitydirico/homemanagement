import { Component } from '@angular/core';
import { AuthService } from '../../auth/authentication.service';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { IdentityService } from '../../api/identity/identity.service';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  username:string;
  password:string;
  isLoading: boolean = false;

  constructor(private authenticationService: AuthService,
    private snackBar: MatSnackBar,
    public dialog: MatDialog) {    
  }

  register(){
    this.isLoading = true;

    this.authenticationService
        .register(this.username, this.password)
        .subscribe(s => {
            this.isLoading = false;
        },err => {
            this.snackBar.open("Error during registration.", "Dismiss",{
                duration: 2000
            });
            this.isLoading = false;
        });
  }
}

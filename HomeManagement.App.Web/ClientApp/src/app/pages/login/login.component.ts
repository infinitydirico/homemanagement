import { Component } from '@angular/core';
import { AuthService } from '../../auth/authentication.service';
import { MatSnackBar, MatDialog } from '@angular/material';
import { ForgotPasswordDialogComponent } from '../../cards/user/forgot-password/forgot.password.dialog.component';
import { IdentityService } from '../../api/identity/identity.service';
import { HttpErrorResponse } from '@angular/common/http';

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
    private snackBar: MatSnackBar,
    public dialog: MatDialog,
    private identityService: IdentityService) {    
  }

  login(){
    this.isLoading = true;
    this.authenticationService.login(this.username, this.password, this.remember).subscribe(res => {
      this.isLoading = false;
    }, (errorResponse:HttpErrorResponse) => {
      this.isLoading = false;
      this.snackBar.open(errorResponse.error, 'ok', {
        duration : 2000
      })
    });
  }

  forgotPassword(){
    let forgotPasswordDialog = this.dialog.open(ForgotPasswordDialogComponent, {
      width: '250px'
    });

    forgotPasswordDialog.afterClosed().subscribe( (recoveryEmail:string) => {

      if(recoveryEmail === undefined || recoveryEmail === null || recoveryEmail === '') return;

      this.identityService.forgotPassword(recoveryEmail).subscribe(result => {
        this.snackBar.open('Recovery sent to: '+recoveryEmail, 'ok', {
          duration: 3000
        });
      }, error => {
        this.snackBar.open('Error sending the recovery', 'ok', {
          duration: 3000
        });
      });
    });
  }
}

import { Component } from '@angular/core';
import { AuthService } from '../../auth/authentication.service';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ForgotPasswordDialogComponent } from '../../cards/user/forgot-password/forgot.password.dialog.component';
import { IdentityService } from '../../api/identity/identity.service';
import { HttpErrorResponse } from '@angular/common/http';
import { TwoFactorAuthenticationService } from 'src/app/api/identity/twofactorauthentication.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  username:string;
  password:string;
  remember: boolean = false;
  code: number;
  isLoading: boolean = false;
  needSecurityCode: boolean = false;

  constructor(private authenticationService: AuthService,
    private snackBar: MatSnackBar,
    public dialog: MatDialog,
    private identityService: IdentityService,
    private twoFactorAuthenticationService: TwoFactorAuthenticationService) {    
  }

  login(){
    this.isLoading = true;

    this.twoFactorAuthenticationService.isEnabledByEmail(this.username).subscribe(result => {
      if(result){
        this.needSecurityCode = true;
        this.isLoading = false;
      }else{
        this.authenticationService.login(this.username, this.password, null, this.remember).subscribe(res => {
          this.isLoading = false;
        }, (errorResponse:HttpErrorResponse) => {
          this.isLoading = false;
          this.snackBar.open(errorResponse.error, 'ok', {
            duration : 2000
          })
        });
      }
    });
  }

  twoFactorLogin(){
    this.isLoading = true;
    this.authenticationService.login(this.username, this.password, this.code, this.remember).subscribe(res => {
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

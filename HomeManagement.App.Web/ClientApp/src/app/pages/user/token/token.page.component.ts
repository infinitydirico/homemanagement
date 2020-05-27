import { Component, OnInit } from '@angular/core';
import { ColorService } from 'src/app/services/color.service';
import { MatSnackBar } from '@angular/material';
import { PasswordService } from 'src/app/common/password.service';
import { ActivatedRouteSnapshot, Router } from '@angular/router';
import { IdentityService } from 'src/app/api/identity.service';

@Component({
  selector: 'token',
  templateUrl: './token.page.component.html',
  styleUrls: ['token.page.component.css']
})
export class TokenPageComponent implements OnInit{

    token:string;
    email:string;
    newPassword:string;
    repeatNewPassword:string;

    constructor(public colorService: ColorService, 
        private snackBar: MatSnackBar,
        private passwordService: PasswordService,
        private router: Router,
        private identityService: IdentityService){
    }

    ngOnInit(): void {
        this.token = this.router.routerState.snapshot.root.queryParams.value;
        this.email = this.router.routerState.snapshot.root.queryParams.email;
    }

    submit(){

        if(this.isPasswordEmpty()){
            this.snackBar.open('The password cannot be empty.', 'close', {
                duration:2000
            });
            return;
        }

        if(this.passwordMatches()){
            this.snackBar.open('The passwords doesn\'t match.', 'close', {
                duration:2000
            });
            return;
        }

        this.identityService.tokenPasswordChange(this.email, this.token, this.newPassword).subscribe(r => {
            this.router.navigate(['/login']);
        }, error => {
            this.snackBar.open('An error happened while trying to reset your password.', 'close', {
                duration:2000
            });
        });
    }

    isPasswordEmpty(){
        return this.newPassword === undefined || this.newPassword === null || this.newPassword === '';
    }

    passwordMatches(){
        return this.newPassword != this.repeatNewPassword;
    }

    isStrong(){
        return this.passwordService.isStrong(this.newPassword);
    }
}
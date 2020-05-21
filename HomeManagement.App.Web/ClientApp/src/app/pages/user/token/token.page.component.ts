import { Component } from '@angular/core';
import { ColorService } from 'src/app/services/color.service';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'token',
  templateUrl: './token.page.component.html',
  styleUrls: ['token.page.component.css']
})
export class TokenPageComponent {

    newPassword:string;
    repeatNewPassword:string;

    constructor(public colorService: ColorService, private snackBar: MatSnackBar){
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
    }

    isPasswordEmpty(){
        return this.newPassword === undefined || this.newPassword === null || this.newPassword === '';
    }

    passwordMatches(){
        return this.newPassword != this.repeatNewPassword;
    }
}
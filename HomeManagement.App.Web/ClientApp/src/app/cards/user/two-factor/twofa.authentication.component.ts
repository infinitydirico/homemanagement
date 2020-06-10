import { Component, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { TwoFactorAuthenticationService } from "src/app/api/identity/twofactorauthentication.service";

@Component({
    selector: 'twofa-authentication',
    templateUrl: 'twofa.authentication.component.html'
})
export class TwoFaAuthenticationComponent implements OnInit {

    twoFactorEnabled: boolean = false;

    constructor(private twoFactorAuthenticationService: TwoFactorAuthenticationService,
        private snackBar: MatSnackBar) {
    }

    ngOnInit(): void {
        this.twoFactorAuthenticationService.isEnabled().subscribe(result => {
            this.twoFactorEnabled = result as boolean;
        });
    }

    onTwoFactorChanged(){
        if(this.twoFactorEnabled){
            this.twoFactorAuthenticationService.Enable().subscribe(r => {});
        }
        else{
            this.twoFactorAuthenticationService.Disable().subscribe(r => {});
        }
    }
}
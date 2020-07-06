import { Component, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { FormControl, Validators } from "@angular/forms";

@Component({
    selector: 'forgot-password',
    templateUrl: 'forgot.password.dialog.component.html'
})
export class ForgotPasswordDialogComponent {

    email:string;
    emailFormControl: FormControl = new FormControl('', [
        Validators.required
    ]);

    constructor(
        public dialogRef: MatDialogRef<ForgotPasswordDialogComponent>,
        private snackBar: MatSnackBar) {
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    ok(){
        if(!this.emailFormControl.valid){
            this.snackBar.open('Email is required', 'Close',{
                duration: 2000
            });

            return;
        }

        this.dialogRef.close(this.email);
    }
}
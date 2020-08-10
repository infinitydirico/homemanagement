import { Component, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Account } from "src/app/models/account";
import { Currency, GetAccountTypes, AccountType, FormError } from "src/app/models/base-types";
import { CurrencyService } from "src/app/api/main/currency.service";
import { FormControl, Validators } from "@angular/forms";

@Component({
    selector: 'account-dialog',
    templateUrl: 'account.add.dialog.component.html'
})
export class AccountAddDialog implements OnInit {

    account: Account = new Account();
    currencies: Array<Currency> = new Array<Currency>();
    accountTypes: Array<AccountType> = GetAccountTypes();

    nameFormControl: FormControl = new FormControl('', [
        Validators.required
    ]);

    currencyFormControl: FormControl = new FormControl('', [
        Validators.required
    ]);

    accountTypeFormControl: FormControl = new FormControl('', [
        Validators.required
    ]);
    
    errors: Array<FormError> = new Array<FormError>();

    constructor(
        public dialogRef: MatDialogRef<AccountAddDialog>,
        private snackBar: MatSnackBar,
        private currencyService: CurrencyService) {
            
        this.errors.push(new FormError('Name is required', this.nameFormControl));
        this.errors.push(new FormError('Currency is required', this.currencyFormControl));
        this.errors.push(new FormError('Account Type is required', this.accountTypeFormControl));
    }

    ngOnInit(): void {
        this.currencyService.get().subscribe(result => {
            result.forEach(c => {
                this.currencies.push(c);
            });
        });
    }

    onCurrencyChanged(currency: Currency){
        this.account.currencyId = currency.id;
    }

    onAccountTypeChanged(accountype: AccountType){
        this.account.accountType = accountype.id;
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    ok(){
        for (let index = 0; index < this.errors.length; index++) {
            const element = this.errors[index];

            if(!element.control.valid){
                this.snackBar.open(element.message, 'Close',{
                    duration: 2000
                });
    
                return;
            }          
        }

        this.dialogRef.close(this.account);
    }
}
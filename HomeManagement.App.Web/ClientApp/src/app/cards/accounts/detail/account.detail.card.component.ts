import { Component, Input } from '@angular/core';
import { AccountService } from 'src/app/api/account.service';
import { AccountType, GetAccountTypes, Currency } from 'src/app/models/base-types';
import { CurrencyService } from 'src/app/api/currency.service';
import { Account } from '../../../models/account';
import { ColorService } from 'src/app/services/color.service';
import { MatSnackBar } from '@angular/material';
import { saveAs } from 'file-saver';

@Component({
  selector: 'account-detail-card',
  templateUrl: './account.detail.card.component.html'
})
export class AccountDetailCardComponent {

    accountTypes: Array<AccountType> =  GetAccountTypes();
    @Input() account: Account;
    currencies: Array<Currency> = new Array<Currency>();
    fileInput: any;
    button: any;

    constructor(private currencyService: CurrencyService,
        private accountService: AccountService,
        private colorService: ColorService,
        private snackBar: MatSnackBar){
        
    }

    getColor(){
      return this.account.balance > 0 ? this.colorService.getSuccess() : this.colorService.getDanger();
    }

    getCurrencyName(){
      return this.currencyService.getCurrencyName(this.account.currencyId);
    }

    onMeasureChanged(){
      this.accountService.update(this.account).subscribe(result =>{
        let message = this.account.measurable ? 'The account is being measured' : 'The account is no longer being measured';
        this.snackBar.open(message,'Dismiss',{
          duration: 2000
        });
      });
    }

    getAccountType(){
      let accountType = this.accountTypes.find(at => at.id === this.account.accountType);
      return accountType;
    }

    download(){
      this.accountService.download(this.account.id).subscribe(result => {
        saveAs(result, this.account.name + '.csv')
      });
    }

    selectFile(){
      this.fileInput = document.getElementById("hiddenInput");
      this.fileInput.click();
    }

    upload(fileInput: any){
        var f = fileInput.target.files[0];        
        var formData: FormData = new FormData();
        formData.append(f.name, f, f.name);

        this.accountService.upload(this.account.id, formData).subscribe(result => {
          this.snackBar.open('File upload succesfully', 'ok', {
            duration: 1000
          });
        });
    }
}
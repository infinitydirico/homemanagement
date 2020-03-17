import { Component, Input } from '@angular/core';
import { AccountService } from 'src/app/api/account.service';
import { AccountType, GetAccountTypes, Currency } from 'src/app/models/base-types';
import { CurrencyService } from 'src/app/api/currency.service';
import { Account } from '../../../models/account';
import { ColorService } from 'src/app/services/color.service';

@Component({
  selector: 'account-detail-card',
  templateUrl: './account.detail.card.component.html'
})
export class AccountDetailCardComponent {

    accountTypes: Array<AccountType> =  GetAccountTypes();
    @Input() account: Account;
    currencies: Array<Currency> = new Array<Currency>();

    constructor(private currencyService: CurrencyService,
        private accountService: AccountService,
        private colorService: ColorService){
        
    }

    getColor(){
      return this.account.balance > 0 ? this.colorService.getSuccess() : this.colorService.getDanger();
    }

    getCurrencyName(){
      return this.currencyService.getCurrencyName(this.account.currencyId);
    }
}
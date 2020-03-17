import { Component, OnInit } from '@angular/core';
import { Account } from '../../../models/account';
import { AccountService } from 'src/app/api/account.service';
import { Currency, AccountType, GetAccountTypes } from 'src/app/models/base-types';
import { CurrencyService } from 'src/app/api/currency.service';
import { Router } from '@angular/router';

@Component({
  selector: 'accounts-list',
  templateUrl: './account.list.card.component.html',
  styleUrls: ['./account.list.card.component.css']
})
export class AccountListCardComponent implements OnInit {

  displayedColumns: string[] = ['name', 'balance', 'currency', 'accountType'];
  accounts: Array<Account> = new Array<Account>();
  currencies: Array<Currency> = new Array<Currency>();
  accountTypes:Array<AccountType> = GetAccountTypes();
  
  constructor(private accountService: AccountService,
    private currencyService: CurrencyService,
    private router: Router) {
    
  }
  ngOnInit(): void {
    
    this.accountService.getAllAccounts().subscribe(result => {
      
      result.forEach(a => {
        this.accounts.push(a);
      });
    });

    this.currencyService.get().subscribe(result => {
      result.forEach(r => {
        this.currencies.push(r);
      });
    });
  }

  getCurrencyName(account:Account){

    return this.currencyService.getCurrencyName(account.currencyId);
  }

  getAccountTypeName(account:Account){
    for (let index = 0; index < this.accountTypes.length; index++) {
      const accountType = this.accountTypes[index];

      if(accountType.id === account.accountType){
        return accountType.name;
      }
    }

    return "";
  }

  view(account: Account){
    this.router.navigate(['/account', account.id]);
  }
}
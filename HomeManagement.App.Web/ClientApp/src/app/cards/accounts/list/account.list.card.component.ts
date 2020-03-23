import { Component, OnInit } from '@angular/core';
import { Account } from '../../../models/account';
import { AccountService } from 'src/app/api/account.service';
import { Currency, AccountType, GetAccountTypes } from 'src/app/models/base-types';
import { CurrencyService } from 'src/app/api/currency.service';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { AccountAddDialog } from '../add/account.add.dialog.component';
import { ColorService } from 'src/app/services/color.service';

@Component({
  selector: 'accounts-list',
  templateUrl: './account.list.card.component.html',
  styleUrls: ['./account.list.card.component.css']
})
export class AccountListCardComponent implements OnInit {

  displayedColumns: string[] = ['name', 'balance', 'currency', 'accountType', 'delete'];
  accounts: Array<Account> = new Array<Account>();
  currencies: Array<Currency> = new Array<Currency>();
  accountTypes: Array<AccountType> = GetAccountTypes();

  constructor(private accountService: AccountService,
    private currencyService: CurrencyService,
    private router: Router,
    public dialog: MatDialog,
    private colorService: ColorService) {

  }
  ngOnInit(): void {
    this.loadAccounts();
    this.loadCurrencies();
  }

  getCurrencyName(account: Account) {
    return this.currencyService.getCurrencyName(account.currencyId);
  }

  getAccountTypeName(account: Account) {
    for (let index = 0; index < this.accountTypes.length; index++) {
      const accountType = this.accountTypes[index];

      if (accountType.id === account.accountType) {
        return accountType.name;
      }
    }

    return "";
  }

  view(account: Account) {
    this.router.navigate(['/account', account.id]);
  }

  add() {
    let accountDialog = this.dialog.open(AccountAddDialog, {
      width: '250px'
    })

    accountDialog.afterClosed().subscribe(account => {

      if(account === undefined) return;
      
      this.accounts.splice(0, this.accounts.length);
      this.accountService.add(account).subscribe(result => {
        this.loadAccounts();
      });
    });
  }

  delete(element:Account){
    this.accounts.splice(0, this.accounts.length);
    this.accountService.remove(element).subscribe(res => {
      this.loadAccounts();
    });
  }

  loadAccounts(){    
    this.accountService.getAllAccounts(true).subscribe(result => {
      result.forEach(a => {
        this.accounts.push(a);
      });
    });
  }

  loadCurrencies(){
    this.currencyService.get().subscribe(result => {
      result.forEach(r => {
        this.currencies.push(r);
      });
    });
  }
}
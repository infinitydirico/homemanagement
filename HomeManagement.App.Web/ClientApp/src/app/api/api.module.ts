import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountService } from "./account.service";
import { ApiService } from './api.service';
import { TransactionService } from './transaction.service';
import { CurrencyService } from './currency.service';

@NgModule({
  imports: [
    CommonModule,
  ],
  providers: [
    AccountService,
    TransactionService,
    ApiService,
    CurrencyService],
  exports: []
})
export class ApiModule { }

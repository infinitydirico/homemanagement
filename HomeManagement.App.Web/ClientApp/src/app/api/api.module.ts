import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountService } from "./account.service";
import { ApiService } from './api.service';
import { TransactionService } from './transaction.service';
import { CurrencyService } from './currency.service';
import { CategoryService } from './category.service';
import { PreferencesService } from './preferences.service';
import { UserService } from './user.service';
import { ReminderService } from './reminder.service';

@NgModule({
  imports: [
    CommonModule,
  ],
  providers: [
    AccountService,
    TransactionService,
    ApiService,
    CurrencyService,
    CategoryService,
    PreferencesService,
    UserService,
    ReminderService],
  exports: []
})
export class ApiModule { }

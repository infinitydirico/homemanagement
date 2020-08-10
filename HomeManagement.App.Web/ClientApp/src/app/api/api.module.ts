import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountService } from "./main/account.service";
import { ApiService } from './api.service';
import { TransactionService } from './main/transaction.service';
import { CurrencyService } from './main/currency.service';
import { CategoryService } from './main/category.service';
import { PreferencesService } from './main/preferences.service';
import { UserService } from './main/user.service';
import { ReminderService } from './main/reminder.service';
import { AccountMetricService } from './main/account.metric.service';
import { NotificationService } from './main/notification.service';
import { IdentityService } from './identity/identity.service';
import { TwoFactorAuthenticationService } from './identity/twofactorauthentication.service';
import { TransactionMetricService } from './main/transaction.metric.service';

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
    ReminderService,
    AccountMetricService,
    NotificationService,
    IdentityService,
    TwoFactorAuthenticationService,
    TransactionMetricService],
  exports: []
})
export class ApiModule { }

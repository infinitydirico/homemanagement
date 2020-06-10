import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AccountListCardComponent } from "./accounts/list/account.list.card.component";
import { MaterialModule } from '../materials.module';
import { ContentCardComponent } from './custom/card.component';
import { AccountDetailCardComponent } from './accounts/detail/account.detail.card.component';
import { TransactionListCardComponent } from './transactions/list/transaction.list.card.component';
import { TransactionAddCardComponent } from './transactions/add/transaction.add.card.component';
import { PreferredCurrencyComponent } from './user/preferred.currency.component';
import { CategoryListCardComponent } from './category/list/category.list.component';
import { ReminderListComponent } from './reminders/list/reminder.list.component';
import { ReminderAddDialog } from './reminders/add/reminder.add.dialog.compnent';
import { AccountAddDialog } from './accounts/add/account.add.dialog.component';
import { CategoryAddDialog } from './category/add/category.add.dialog.component';
import { IncomeCardComponent } from './metrics/income/income.card.component';
import { OutcomeCardComponent } from './metrics/outcome/outcome.card.component';
import { BarChartComponent } from './custom/charts/bar/bar.chart.component';
import { LineChartComponent } from './custom/charts/line/line.chart.component';
import { OutcomeCategoriesChart } from './accounts/outcome-categories/outcome.categories.chart.component';
import { AccountsEvolutionCardComponent } from './metrics/accounts-evolution/accounts.evolution.card.component';
import { TransactionAddDialogComponent } from './transactions/add-dialog/transaction.add.dialog.component';
import { PreferencesCardComponent } from './preferences/preferences.card.component';
import { ForgotPasswordDialogComponent } from "./user/forgot-password/forgot.password.dialog.component";
import { TwoFaAuthenticationComponent } from './user/two-factor/twofa.authentication.component';
import { ChangePasswordComponent } from './user/password/change.password.component';

@NgModule({
    declarations: [
        AccountListCardComponent,
        ContentCardComponent,
        AccountDetailCardComponent,
        TransactionListCardComponent,
        TransactionAddCardComponent,
        PreferredCurrencyComponent,
        CategoryListCardComponent,
        ReminderListComponent,
        ReminderAddDialog,
        AccountAddDialog,
        CategoryAddDialog,
        IncomeCardComponent,
        OutcomeCardComponent,
        BarChartComponent,
        LineChartComponent,
        OutcomeCategoriesChart,
        AccountsEvolutionCardComponent,
        TransactionAddDialogComponent,
        PreferencesCardComponent,
        ForgotPasswordDialogComponent,
        TwoFaAuthenticationComponent,
        ChangePasswordComponent
    ],
    imports: [
        MaterialModule,
        FormsModule,
        ReactiveFormsModule        
    ],
    exports: [
        AccountListCardComponent,
        ContentCardComponent,
        AccountDetailCardComponent,
        TransactionListCardComponent,
        TransactionAddCardComponent,
        PreferredCurrencyComponent,
        CategoryListCardComponent,
        ReminderListComponent,
        ReminderAddDialog,
        AccountAddDialog,
        CategoryAddDialog,
        IncomeCardComponent,
        OutcomeCardComponent,
        BarChartComponent,
        LineChartComponent,
        OutcomeCategoriesChart,
        AccountsEvolutionCardComponent,
        TransactionAddDialogComponent,
        PreferencesCardComponent,
        ForgotPasswordDialogComponent,
        TwoFaAuthenticationComponent,
        ChangePasswordComponent
    ],
    entryComponents: [
        ReminderAddDialog,
        AccountAddDialog,
        CategoryAddDialog,
        TransactionAddDialogComponent,
        ForgotPasswordDialogComponent
    ]
})
export class CardsModule { }

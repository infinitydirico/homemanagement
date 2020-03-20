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

@NgModule({
    declarations: [
        AccountListCardComponent,
        ContentCardComponent,
        AccountDetailCardComponent,
        TransactionListCardComponent,
        TransactionAddCardComponent,
        PreferredCurrencyComponent,
        CategoryListCardComponent
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
        CategoryListCardComponent
    ]
})
export class CardsModule { }

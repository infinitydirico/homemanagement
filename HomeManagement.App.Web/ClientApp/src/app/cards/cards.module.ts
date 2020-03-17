import { NgModule } from '@angular/core';

import { AccountListCardComponent } from "./accounts/list/account.list.card.component";
import { MaterialModule } from '../materials.module';
import { ContentCardComponent } from './custom/card.component';
import { AccountDetailCardComponent } from './accounts/detail/account.detail.card.component';
import { TransactionListCardComponent } from './transactions/list/transaction.list.card.component';

@NgModule({
    declarations: [
        AccountListCardComponent,
        ContentCardComponent,
        AccountDetailCardComponent,
        TransactionListCardComponent
    ],
    imports: [
        MaterialModule
    ],
    exports: [
        AccountListCardComponent,
        ContentCardComponent,
        AccountDetailCardComponent,
        TransactionListCardComponent
    ]
})
export class CardsModule { }

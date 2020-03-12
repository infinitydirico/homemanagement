import { NgModule } from '@angular/core';

import { AccountListCardComponent } from "./accounts/list/account.list.card.component";
import { MaterialModule } from '../materials.module';
import { ContentCardComponent } from './custom/card.component';

@NgModule({
    declarations: [
        AccountListCardComponent,
        ContentCardComponent
    ],
    imports: [
        MaterialModule
    ],
    exports: [
        AccountListCardComponent,
        ContentCardComponent
    ]
})
export class CardsModule { }

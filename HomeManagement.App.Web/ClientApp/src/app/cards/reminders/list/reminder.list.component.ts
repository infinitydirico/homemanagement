import { Component, Input, OnInit } from '@angular/core';
import { Account } from '../../../models/account';
import { TransactionService } from 'src/app/api/transaction.service';
import { TransactionPageModel, Transaction } from 'src/app/models/transaction';
import { ColorService } from 'src/app/services/color.service';
import { Category } from 'src/app/models/category';
import { CategoryService } from 'src/app/api/category.service';
import { HelperService } from 'src/app/services/helper.service';

@Component({
    selector: 'transaction-list',
    templateUrl: './transaction.list.component.html'//,
    //styleUrls: ['transaction.list.component.css']
})
export class ReminderListComponent implements OnInit {


    ngOnInit(): void {
        throw new Error("Method not implemented.");
    }

}
import { Component, Input, OnInit } from '@angular/core';
import { Account } from '../../../models/account';
import { TransactionService } from 'src/app/api/transaction.service';
import { TransactionPageModel, Transaction } from 'src/app/models/transaction';

@Component({
  selector: 'transaction-list-card',
  templateUrl: './transaction.list.card.component.html',
  styleUrls: ['transaction.list.card.component.css']
})
export class TransactionListCardComponent implements OnInit{

    page:TransactionPageModel = new TransactionPageModel();
    @Input()account: Account;
    displayedColumns: string[] = ['name', 'categoryId', 'date', 'price'];
    transactions:Array<Transaction> = new Array<Transaction>();

    constructor(private transactionService: TransactionService){
    }

    ngOnInit(): void {
        this.page.accountId = this.account.id;
        this.page.currentPage = 1;
        this.page.pageCount = 10;
        this.page.skip = 0;

        this.transactions.splice(0, this.transactions.length);
        this.transactionService.paginate(this.page).subscribe(trans => {
            trans.transactions.forEach(tr => {
                this.transactions.push(tr);
            });            
        });
    }

}
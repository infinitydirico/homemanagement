import { Component, Input, OnInit } from '@angular/core';
import { Account } from '../../../models/account';
import { TransactionService } from 'src/app/api/transaction.service';
import { TransactionPageModel, Transaction } from 'src/app/models/transaction';
import { ColorService } from 'src/app/services/color.service';
import { Category } from 'src/app/models/category';
import { CategoryService } from 'src/app/api/category.service';
import { HelperService } from 'src/app/services/helper.service';
import { CommonService } from 'src/app/common/common.service';

@Component({
    selector: 'transaction-list-card',
    templateUrl: './transaction.list.card.component.html',
    styleUrls: ['transaction.list.card.component.css']
})
export class TransactionListCardComponent implements OnInit {

    page: TransactionPageModel = new TransactionPageModel();
    @Input() account: Account;
    displayedColumns: string[] = ['name', 'categoryId', 'date', 'price', 'delete'];
    transactions: Array<Transaction> = new Array<Transaction>();
    categories: Array<Category> = new Array<Category>();
    totalPages: Array<number> = [];
    isMobile = this.commonService.isMobile();

    constructor(private transactionService: TransactionService,
        private colorService: ColorService,
        private categoryService: CategoryService,
        private helperService: HelperService,
        private commonService: CommonService) {
    }

    ngOnInit(): void {

        if(this.isMobile){
            this.displayedColumns = this.displayedColumns.filter(x => !x.includes('date'));
        }

        this.load();
    }

    paginate(p: number) {
        this.transactions.length = 0;
        this.page.accountId = this.account.id;
        this.page.currentPage = p;
        this.page.pageCount = 10;
        this.page.skip = 0;

        this.transactionService.paginate(this.page).subscribe(_ => {
            _.transactions.forEach(charge => {
                this.transactions.push(charge);

            });

            this.page.totalPages = _.totalPages;
            this.totalPages = this.helperService.thinPageNumbers(this.page.currentPage, _.totalPages);
        });
    }

    previousPage(){
        let page = this.page.currentPage - 1;
        this.paginate(page);
    }

    nextPage(){
        let page = this.page.currentPage + 1;
        this.paginate(page);
    }

    getColor(transaction: Transaction) {
        return transaction.transactionType > 0 ? this.colorService.getDanger() : this.colorService.getSuccess();
    }

    getCategoryIcon(transaction: Transaction) {
        let category = this.categories.find(c => transaction.categoryId === c.id);
        return category.icon;
    }

    edit(transaction: Transaction) {
        console.log(transaction);
    }

    delete(transaction: Transaction) {
        this.transactionService.removeTransaction(transaction).subscribe(result => {
            this.paginate(this.page.currentPage);
        });
    }

    load(){
        this.paginate(1);
        this.categoryService.getCategories().subscribe(result => {
            result.forEach(c => {
                this.categories.push(c);
            });
        });
    }
}
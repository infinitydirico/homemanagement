import { Component, Input, OnInit } from '@angular/core';
import { Account } from '../../../models/account';
import { TransactionService } from 'src/app/api/main/transaction.service';
import { TransactionPageModel, Transaction } from 'src/app/models/transaction';
import { ColorService } from 'src/app/services/color.service';
import { Category } from 'src/app/models/category';
import { CategoryService } from 'src/app/api/main/category.service';
import { HelperService } from 'src/app/services/helper.service';
import { CommonService } from 'src/app/common/common.service';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TransactionAddDialogComponent } from '../add-dialog/transaction.add.dialog.component';
import { TransactionEditDialogComponent } from '../edit/transaction.edit.dialog.component';

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
    isLoading: boolean = false;

    constructor(private transactionService: TransactionService,
        public colorService: ColorService,
        private categoryService: CategoryService,
        private helperService: HelperService,
        private commonService: CommonService,
        public dialog: MatDialog,
        private snackBar: MatSnackBar) {
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
        this.isLoading = true;
        this.transactionService.paginate(this.page).subscribe(_ => {            
            this.isLoading = false;
            _.forEach(charge => {
                this.transactions.push(charge);
            });
            
            this.totalPages = this.helperService.thinPageNumbers(this.page.currentPage, 100);//will implement infinite scrolling
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

    getCategoryName(transaction: Transaction){
        let category = this.categories.find(c => transaction.categoryId === c.id);
        return category.name;
    }

    edit(transaction: Transaction) {
        let transactionDialog = this.dialog.open(TransactionEditDialogComponent, {
            width: '550px',
            data: transaction
          })
      
          transactionDialog.afterClosed().subscribe((updateTransaction:Transaction) => {
      
            if (updateTransaction === undefined) return;
      
            this.transactionService.update(updateTransaction).subscribe(result => {
                this.snackBar.open("Transaction " + updateTransaction.name + " updated !", "Dismiss", {
                    duration: 2000
                })
            });
          });
    }

    delete(transaction: Transaction) {
        this.transactionService.removeTransaction(transaction).subscribe(result => {
            this.paginate(this.page.currentPage);
        });
    }

    deleteAll(){
        this.transactionService.removeAll(this.account).subscribe(_ => {
            this.paginate(1);
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

    add(){
        let transactionDialog = this.dialog.open(TransactionAddDialogComponent, {
            width: '550px',
            data: this.account
          })
      
          transactionDialog.afterClosed().subscribe(newTransaction => {
      
            if (newTransaction === undefined) return;
      
            this.transactionService.add(newTransaction).subscribe(result => {
                this.paginate(1);
            });
          });
        
    }
}
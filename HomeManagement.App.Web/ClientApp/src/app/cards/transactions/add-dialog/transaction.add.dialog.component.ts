import { Component, Input, OnInit, Inject } from '@angular/core';
import { Account } from '../../../models/account';
import { TransactionService } from 'src/app/api/transaction.service';
import { Transaction } from 'src/app/models/transaction';
import { CurrencyService } from 'src/app/api/currency.service';
import { Currency, FormError } from 'src/app/models/base-types';
import { CategoryService } from 'src/app/api/category.service';
import { Category } from 'src/app/models/category';
import { FormControl, Validators } from '@angular/forms';
import { startWith, map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { MatSnackBar, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ColorService } from 'src/app/services/color.service';

@Component({
  selector: 'transaction-add-dialog',
  templateUrl: './transaction.add.dialog.component.html',
  styleUrls: ['transaction.add.dialog.component.css']
})
export class TransactionAddDialogComponent implements OnInit{

    @Input() account: Account;
    currencies: Array<Currency> = new Array<Currency>();
    categories: Array<Category> = new Array<Category>();
    filteredCategoryies: Observable<Array<string>>;
    transaction: Transaction = new Transaction();
    transactionTypes: Array<number> = [0, 1];
    selectedTransactionType: number;
    categoryControl = new FormControl('',[
        Validators.required
    ]);
    nameFormControl: FormControl = new FormControl('', [
        Validators.required
    ]);
    priceFormControl: FormControl = new FormControl('', [
        Validators.required
    ]);
    dateFormControl: FormControl = new FormControl('', [
        Validators.required
    ]);

    errors: Array<FormError> = new Array<FormError>();

    constructor(public dialogRef: MatDialogRef<TransactionAddDialogComponent>,
        private transactionService: TransactionService,
        private currencyService: CurrencyService,
        private categoryService: CategoryService,
        private snackBar: MatSnackBar,
        public colorService: ColorService,
        @Inject(MAT_DIALOG_DATA) public ac: Account){
            
        this.errors.push(new FormError('Name is required', this.nameFormControl));
        this.errors.push(new FormError('Price is required', this.priceFormControl));
        this.errors.push(new FormError('Category is required', this.categoryControl));
        this.errors.push(new FormError('Date is required', this.dateFormControl));
        this.account = ac;
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    getTypeLabel(index:number){
        return index > 0 ? 'Outcome' : 'Income';
    }

    ngOnInit(): void {
        this.transaction.accountId = this.account.id;

        this.currencyService.get().subscribe(result => {

            result.forEach(c => {
                this.currencies.push(c);
            });
        });

        this.categoryService.getActiveCategories().subscribe(result => {
            result.forEach(c => {
                this.categories.push(c);
            });
        });

        this.filteredCategoryies = this.categoryControl.valueChanges
        .pipe(startWith(''),
                map(value => this.filterCategories(value)));
    }

    submit(){
        for (let index = 0; index < this.errors.length; index++) {
            const element = this.errors[index];

            if(!element.control.valid){
                this.snackBar.open(element.message, 'Close',{
                    duration: 2000
                });
    
                return;
            }            
        }

        this.dialogRef.close(this.transaction);
    }

    private filterCategories(value:any) : Array<string> {
        let categoryName = value.toLowerCase();

        var filteredValues = this.categories            
            .filter(option => {
                let selected = option.name.toLowerCase().includes(categoryName);

                if(selected){
                    this.transaction.categoryId = option.id;
                }
                
                return selected;
            })
            .map(c => c.name);

        return filteredValues;
    }
}
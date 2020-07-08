export class Transaction
{
    id:number = 0;
    name:string = '';
    price:number = 0;
    date:Date = new Date();
    transactionType:number = 1;
    accountId: number = 0;
    categoryId: number = 0;
    selected: boolean = false;
    hover: boolean = false;

    static new(transaction:Transaction){
        let value = new Transaction();
        value.id = transaction.id;
        value.name = transaction.name;
        value.price = transaction.price;
        value.date = transaction.date;
        value.transactionType = transaction.transactionType;
        value.accountId = transaction.accountId;
        value.categoryId = transaction.categoryId;
        value.selected = false;
        return value;
    }
}

export class TransactionType{
    name:string;
    value:number;
}

export class TransactionPageModel {
    accountId: number;
    transactions: Array<Transaction>;
    currentPage: number;
    pageCount: number;
    skip: number;
    totalPages: number;
    property: string;
    filterValue: string;
    operator: number;
}
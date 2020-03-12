export class Transaction
{
    id:number;
    name:string;
    price:number;
    date:Date = new Date();
    transactionType:number = 1;
    accountId: number;
    categoryId: number;
    selected: boolean = false;
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
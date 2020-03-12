export class Account {
    id: number;
    name: string;
    userId: number;
    balance: number;
    currencyId:number;
    accountType:number;
    measurable:boolean;
    hovering:boolean = false;
}

export class AccountPageModel {
    userId: number;
    accounts: Array<Account>;
    currentPage: number;
    pageCount: number;
    totalPages: number;
    property: string;
    filterValue: string;
    operator: number;
}

export class TransferDto{
    operationName:string;
    sourceAccountId: number;
    targetAccountId: number;
    price: number;    
    categoryId: number;        
}
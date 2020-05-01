import { FormControl } from "@angular/forms";

export class KeyValuePairBaseType{
    id:number;
    name:string;

    constructor(id:number,name:string){
        this.id = id;
        this.name = name;
    }
}

export class Currency extends KeyValuePairBaseType{
    value:number;

    constructor(id:number,name:string){
        super(id,name);
    }

    toString(){
        return this.name;
    }
}

export class MoneyType extends KeyValuePairBaseType{
    constructor(id:number,name:string){
        super(id,name);
    }

    toString(){
        return this.name;
    }
}

export class AccountType extends KeyValuePairBaseType{
    constructor(id:number,name:string){
        super(id,name);
    }

    toString(){
        return this.name;
    }
}

export function GetAccountTypes(){
    let accountTypes = Array<AccountType>();
    accountTypes.push(new AccountType(0,"Cash"));
    accountTypes.push(new AccountType(1,"Bank"));
    accountTypes.push(new AccountType(2,"Credit Card"));
    return accountTypes;
}

export class FormError{
    message:string = "";
    control: FormControl;

    constructor(m:string, c:FormControl){
        this.message = m;
        this.control = c;
    }
}

export class Metric{
    percentage:number;
    total:number;
}
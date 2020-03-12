export class User{
    id: number = 0;
    email: string;
    password: string;
    token:string = "";
    language:string;
    currency:string;
}

export class token{
    issueDate:Date;
    value:string;
}
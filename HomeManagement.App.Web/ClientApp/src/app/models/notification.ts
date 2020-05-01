export class Notification{
    id:number;
    title:string;  
    description:string;
    dueDay:number;
    dismissed:boolean;
    remainderId:number;  
}

export function sampleNotifications(){
    let n1 = new Notification();
    n1.id = 1;
    n1.title = "Mike John responded to your email";

    let n2 = new Notification();
    n2.id = 2;
    n2.title = "you have 5 new tasks";

    let array = new Array<Notification>();
    array.push(n1);
    array.push(n2);

    return array;
}
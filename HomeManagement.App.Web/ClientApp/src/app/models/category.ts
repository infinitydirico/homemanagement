export class Category {
    id:number;
    name:string;
    isActive:boolean;
    editing:boolean = false;
    hovering:boolean = false;
    icon:string;
    selected:boolean = false;
    measurable:boolean;
}

export class OutcomeCategory{
    category: Category;
    price:number;
}

export class OutcomeCategories{
    categories:Array<OutcomeCategory>;
    lowestValue:number;
    highestValue:number;
}
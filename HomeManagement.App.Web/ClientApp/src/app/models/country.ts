export class Country{
    name:string;
}

export function NewCountry(name:string){
    let c = new Country();
    c.name = name;
    return c;
}
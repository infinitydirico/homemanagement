import { Injectable } from "@angular/core";

@Injectable()
export class CacheService {
    public exists(key:string) : boolean {
        let item = localStorage.getItem(key);
        return item !== null;
    }

    public get<T>(key:string) : T{
        let item = localStorage.getItem(key);
        let value = JSON.parse(item) as T;
        return value; 
    }

    public save(key:string, value:any){

        if(typeof(value) !== "string"){
            localStorage.setItem(key, JSON.stringify(value));
        }else{
            localStorage.setItem(key, value);
        }
    }

    public remove(key:string){
        localStorage.removeItem(key);
    }
}
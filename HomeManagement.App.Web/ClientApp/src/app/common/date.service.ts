import { Injectable } from "@angular/core";

@Injectable()
export class DateService {

    months: Array<string> = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    today: Date = new Date();

    currentMonthName(): string{
        var date = new Date();
    
        let label = this.months[date.getMonth()]
        
        return label;
    }

    getMonth(){
        return this.today.getMonth() + 1;
    }
}
import { Component, OnInit, Input } from "@angular/core";
import { DateService } from "src/app/common/date.service";
import { AccountService } from "src/app/api/main/account.service";
import { Account } from "src/app/models/account";

@Component({
    selector: 'account-outcome-categories-chart',
    templateUrl: 'top-charge.chart.component.html'
})
export class AccountOutcomeCategoriesChart implements OnInit{

    month:number;
    data: Array<number>;
    labels: Array<string>;
    @Input() account: Account;
    
    constructor(private accountService: AccountService,
        private dateService: DateService){
        }

    ngOnInit(){
        this.month = this.dateService.getMonth();
        this.accountService.getAccountTopCharges(this.account, 6).subscribe(result => {

            this.data = result.map(x => x.price);
            this.labels = result.map(x => x.category.name);
        });
    }
}
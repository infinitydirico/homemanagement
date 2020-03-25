import { Component, OnInit } from "@angular/core";
import { AccountMetricService } from "src/app/api/account.metric.service";
import { Metric } from "src/app/models/base-types";
import { ColorService } from "src/app/services/color.service";
import { DateService } from "src/app/common/date.service";

@Component({
    selector: 'income-metric',
    templateUrl: 'income.card.component.html'
})
export class IncomeCardComponent implements OnInit {

    income: Metric;
    month:string;

    constructor(private accountMetricService: AccountMetricService,
        private colorService: ColorService,
        private dateService: DateService){

    }

    ngOnInit(): void {
        this.month = this.dateService.currentMonthName();

        this.accountMetricService.getTotalIncome().subscribe(result => {
            this.income = result;
        });
    }

}
import { Component, OnInit, ApplicationRef } from "@angular/core";
import { AccountMetricService } from "src/app/api/main/account.metric.service";
import { DateService } from "src/app/common/date.service";
import { OutcomeCategories } from "src/app/models/category";

@Component({
    selector: 'outcome-categories-chart',
    templateUrl: 'outcome.categories.chart.component.html'
})
export class OutcomeCategoriesChart implements OnInit{

    month:number;
    categories: OutcomeCategories;
    data: Array<number> = new Array<number>();
    labels: Array<string>;

    constructor(private metricService: AccountMetricService,
        private dateService: DateService){
        }

    ngOnInit(){
        this.month = this.dateService.getMonth();
        this.metricService.outcomeGroupedByCategories(this.month).subscribe(result => {
            this.categories = result;

            this.data = this.categories.categories.map(x => x.price);
            this.labels = this.categories.categories.map(x => x.category.name);
        });
    }
}
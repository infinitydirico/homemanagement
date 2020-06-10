import { Component, OnInit } from "@angular/core";
import { AccountMetricService } from "src/app/api/main/account.metric.service";
import { AccountsEvolutionModel } from "src/app/models/account-chart-data";
import { PaletteService } from "src/app/services/palette.service";

@Component({
    selector: 'accounts-evolution',
    templateUrl: 'accounts.evolution.card.component.html'
})
export class AccountsEvolutionCardComponent implements OnInit {

    evolution: AccountsEvolutionModel;
    datasets: any;
    labels: Array<string>;

    constructor(private accountMetricService: AccountMetricService,
        private paletteService: PaletteService){
    }

    ngOnInit(): void {
        this.accountMetricService.getAccountsEvolution().subscribe(result => {
            this.evolution = result;

            let account = this.evolution.accounts[0];

            this.labels = account.balanceEvolution.map(x => x.month);

            let colors = this.paletteService.getColors(this.evolution.accounts.length);

            this.datasets = this.evolution.accounts.map(x => {
                return {
                    data: x.balanceEvolution.map(x => x.balance),
                    backgroundColor: colors[this.evolution.accounts.indexOf(x)],
                    borderColor: '#424242',
                    fill: 'white',
                    label: x.accountName
                };
            });
        });
    }
}
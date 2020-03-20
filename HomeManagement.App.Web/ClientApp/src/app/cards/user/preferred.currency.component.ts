import { Component, OnInit } from '@angular/core';
import { CurrencyService } from 'src/app/api/currency.service';
import { Currency } from 'src/app/models/base-types';
import { PreferencesService } from 'src/app/api/preferences.service';

@Component({
  selector: 'preferred-currency',
  templateUrl: './preferred.currency.component.html',
  styleUrls: ['preferred.currency.component.css']
})
export class PreferredCurrencyComponent implements OnInit {

    currencies: Array<Currency> = new Array<Currency>();
    selectedCurrency: Currency;

    constructor(private currencyService: CurrencyService,
        private preferredCurrency: PreferencesService){

    }

    ngOnInit(): void {
        this.currencyService.get().subscribe(result => {
            result.forEach(element => {
                this.currencies.push(element);
            });
        });

        this.preferredCurrency.getCurrencyAndLanguage().subscribe(result => {
            let currency = this.getCurrency(result.currency);
            this.selectedCurrency = currency;
        });
    }

    getCurrency(name:string){
        let c = this.currencies.find(r => r.name === name);
        return c;
    }

    onCurrencyChanged(){
        console.log(this.selectedCurrency);
    }
}
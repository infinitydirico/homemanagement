import { Component, OnInit } from '@angular/core';
import { CurrencyService } from 'src/app/api/main/currency.service';
import { Currency } from 'src/app/models/base-types';
import { PreferencesService } from 'src/app/api/main/preferences.service';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'preferred-currency',
  templateUrl: './preferred.currency.component.html',
  styleUrls: ['preferred.currency.component.css']
})
export class PreferredCurrencyComponent implements OnInit {

    currencies: Array<Currency> = new Array<Currency>();
    selectedCurrency: Currency;

    constructor(private currencyService: CurrencyService,
        private preferredCurrency: PreferencesService,
        private preferencesService: PreferencesService,
        private snackBar: MatSnackBar){

    }

    ngOnInit(): void {
        this.currencyService.get().subscribe(result => {
            result.forEach(element => {
                this.currencies.push(element);
            });

            this.preferredCurrency.getCurrencyAndLanguage().subscribe(result => {
                let currency = this.currencies.find(r => r.name === result.currency);
                this.selectedCurrency = currency;
            });
        });
    }

    onCurrencyChanged(currency: Currency){
        this.selectedCurrency = currency;
        this.preferencesService.updatePreferredCurrency(this.selectedCurrency.name).subscribe(_ =>{
            this.snackBar.open('Preferred currency updated !', 'ok', {
                duration: 1000
            });
        });
    }
}
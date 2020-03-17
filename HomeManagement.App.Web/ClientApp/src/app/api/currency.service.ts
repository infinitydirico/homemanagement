import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { Currency } from '../models/base-types';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../auth/authentication.service';
import { environment } from 'src/environments/environment';

@Injectable()
export class CurrencyService {

    private currencies:Array<Currency> = new Array<Currency>();
    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    
    constructor(private http: HttpClient,
        private authenticationService: AuthService) {
            this.httpOptions.headers = this.httpOptions.headers.append('Authorization', this.authenticationService.getToken());
    }

    get(){
        return this.http.get<Array<Currency>>(environment.api + '/api/currency', this.httpOptions)
        .pipe(map(_ =>{
            var result = _;

            if(this.currencies.length > 0) return this.currencies;

            result.forEach(r => {
                this.currencies.push(r);
            });

            return result;
        }, err => {
            
        }));
    }

    getCurrencyName(id:number){
        let currency = this.currencies.find(c => {
            if(c.id === id) return c;
        });

        return currency.name;
    }
}
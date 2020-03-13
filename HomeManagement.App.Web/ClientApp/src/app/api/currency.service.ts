import { Injectable } from '@angular/core';
import { ServiceConstants } from './service.constants';
import { map } from 'rxjs/operators';
import { Currency } from '../models/base-types';
import { ApiService } from './api.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../auth/authentication.service';

@Injectable()
export class CurrencyService {

    private currencies:Array<Currency> = new Array<Currency>();
    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    
    constructor(private http: HttpClient,
        private apiService: ApiService,
        private authenticationService: AuthService) {
            this.httpOptions.headers = this.httpOptions.headers.append('Authorization', this.authenticationService.getToken());
    }

    get(){
        return this.http.get<Array<Currency>>(ServiceConstants.apiCurrency, this.httpOptions)
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
}
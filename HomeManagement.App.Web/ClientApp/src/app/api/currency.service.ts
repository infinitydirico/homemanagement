import { Injectable, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { Currency } from '../models/base-types';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../auth/authentication.service';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable()
export class CurrencyService implements OnInit {

    private currencies:Array<Currency> = new Array<Currency>();
    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    
    constructor(private http: HttpClient,
        private authenticationService: AuthService) {
            this.httpOptions.headers = this.httpOptions.headers.append('Authorization', this.authenticationService.getToken());
    }

    ngOnInit(): void {
        this.get().subscribe(_ => {});
    }

    get() : Observable<Array<Currency>>{
        if(this.currencies.length > 0){
            return new Observable(observer => {
                observer.next(this.currencies);
            });
        }

        return this.http.get<Array<Currency>>(environment.api + '/api/currency', this.httpOptions)
        .pipe(map(_ =>{
            var result = _;
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
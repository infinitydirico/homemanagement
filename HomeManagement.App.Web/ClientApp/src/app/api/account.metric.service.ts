import { Injectable, EventEmitter, Output } from '@angular/core';
import { User } from "../models/user";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { AuthService } from '../auth/authentication.service';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Metric } from '../models/base-types';
import { OutcomeCategories } from '../models/category';
import { AccountBalanceModel, AccountsEvolutionModel } from '../models/account-chart-data';

@Injectable()
export class AccountMetricService {

    user: User;
    @Output() serviceUpdate = new EventEmitter();
    endpoint: string;
    income:Metric;
    outcome:Metric;

    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    constructor(private http: HttpClient,
        private authenticationService: AuthService) {

        this.httpOptions.headers = this.httpOptions.headers.append('Authorization', this.authenticationService.getToken());
        this.endpoint = environment.api + '/api/account';
    }


    getTotalIncome() : Observable<Metric> {

        if(this.income !== undefined){
            return new Observable(observer => {                
                observer.next(this.income);
            });
        }

        return this.http.get<Metric>(this.endpoint + '/incomes', this.httpOptions)
        .pipe(map(result => {
            this.income = result;
            return result;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    getTotalOutcome() : Observable<Metric> {

        if(this.income !== undefined){
            return new Observable(observer => {                
                observer.next(this.outcome);
            });
        }

        return this.http.get<Metric>(this.endpoint + '/outcomes', this.httpOptions)
        .pipe(map(result => {
            this.outcome = result;
            return result;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    outcomeGroupedByCategories(month:number) : Observable<OutcomeCategories> {

        return this.http.get<OutcomeCategories>(this.endpoint + '/toptransactions/' + month, this.httpOptions)
        .pipe(map(result => {
            return result;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    getAccountsEvolution() : Observable<AccountsEvolutionModel> {

        return this.http.get<AccountsEvolutionModel>(this.endpoint + '/accountsevolution', this.httpOptions)
        .pipe(map(result => {
            return result;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }
}

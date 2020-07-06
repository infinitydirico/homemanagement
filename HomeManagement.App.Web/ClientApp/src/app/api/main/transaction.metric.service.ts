import { Injectable, EventEmitter, Output, Directive } from '@angular/core';
import { Transaction, TransactionPageModel } from "../../models/transaction";
import { Account } from "../../models/account";
import { map } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './../../auth/authentication.service'
import { environment } from 'src/environments/environment';

@Directive()
@Injectable()
export class TransactionMetricService {

    @Output() serviceUpdate = new EventEmitter();
    endpoint: string;

    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    
    constructor(private http: HttpClient,
        private authenticationService: AuthService) {

        this.httpOptions.headers = this.httpOptions.headers.append('Authorization', this.authenticationService.getToken());
        this.endpoint = environment.api + '/api/transactions';
    }

    getTransactionDataByDate(month:number, year:number){
        return this.http.get(this.endpoint + '/by/date/' + year + '/' + month, this.httpOptions)
            .pipe(map(result => {
                console.log(result);
                return true;
            }));
    }

    getTransactionDataByDateAndAccount(month:number, year:number, accountId:number){
        return this.http.get(this.endpoint + '/by/date/' + year + '/' + month + '/account/' + accountId, this.httpOptions)
            .pipe(map(result => {
                console.log(result);
                return true;
            }));
    }
}
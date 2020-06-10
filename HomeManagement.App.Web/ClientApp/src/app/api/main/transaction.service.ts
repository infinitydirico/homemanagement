import { Injectable, EventEmitter, Output } from '@angular/core';
import { Transaction, TransactionPageModel } from "../../models/transaction";
import { Account } from "../../models/account";
import { map } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './../../auth/authentication.service'
import { environment } from 'src/environments/environment';

@Injectable()
export class TransactionService {

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

    paginate(page: TransactionPageModel) {
        return this.http.post<TransactionPageModel>(this.endpoint + '/paging', page, this.httpOptions)
        .pipe(map(res => {
          return res;
        }));
    }

    getLastFive() {
        return this.http.get<Array<Transaction>>(this.endpoint + '/getlastfive')
        .pipe(map((result, index) => {
            return result as Array<Transaction>;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    add(transaction:Transaction){
        return this.http.post(this.endpoint, transaction, this.httpOptions)
        .pipe(map(result => {
            this.serviceUpdate.emit();
            return result;
        }));
    }

    updateCharge(transaction:Transaction){
        return this.http.put(this.endpoint, transaction)
        .pipe(map(result => {
            this.serviceUpdate.emit();
            return result;
        }));
    }

    removeTransaction(transaction:Transaction){
        return this.http.delete(this.endpoint + '/' + transaction.id, this.httpOptions)
        .pipe(map(result => {
            return result;
        }));
    }

    removeAll(a:Account){
        return this.http.delete(this.endpoint + '/deleteall/' + a.id)
        .pipe(map(result => {
            return result;
        }));
    }

    updateAll(transaction:Array<Transaction>){
        return this.http.post(this.endpoint + '/updateAll', transaction)
        .pipe(map(result => {
            return true;
        }));
    }

    createFromImage(file:File){
        return this.http.post<Transaction>(this.endpoint, file)
        .pipe(map(result => {
            return result;
        }));
    }

    isImageServiceConfigured(){
        return this.http.get<boolean>(this.endpoint + "/isconfigured")
        .pipe(map(result => {
            return result;
        }));
    }
}

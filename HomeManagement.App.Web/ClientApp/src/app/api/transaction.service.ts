import { Injectable, EventEmitter, Output } from '@angular/core';
import { Transaction, TransactionPageModel } from "../models/transaction";
import { Account } from "../models/account";
import { ServiceConstants } from './service.constants';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ApiService } from './api.service';

@Injectable()
export class TransactionService {

    @Output() serviceUpdate = new EventEmitter();

    constructor(private http: HttpClient,
        private apiService: ApiService) {

    }

    paginate(page: TransactionPageModel) {
        return this.http.get("", this.apiService.getHeaders())
        .pipe(map(res => {
          return res;
        }));
    }

    getLastFive() {
        return this.http.get<Array<Transaction>>(ServiceConstants.apiCharge + '/getlastfive')
        .pipe(map((result, index) => {
            return result as Array<Transaction>;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    addCharge(transaction:Transaction){
        return this.http.post(ServiceConstants.apiCharge, transaction)
        .pipe(map(result => {
            this.serviceUpdate.emit();
            return result;
        }));
    }

    updateCharge(transaction:Transaction){
        return this.http.put(ServiceConstants.apiCharge, transaction)
        .pipe(map(result => {
            this.serviceUpdate.emit();
            return result;
        }));
    }

    removeCharge(transaction:Transaction){
        return this.http.delete(ServiceConstants.apiCharge + '/' + transaction.id)
        .pipe(map(result => {
            return result;
        }));
    }

    removeAll(a:Account){
        return this.http.delete(ServiceConstants.apiCharge + '/deleteall/' + a.id)
        .pipe(map(result => {
            return result;
        }));
    }

    updateAll(transaction:Array<Transaction>){
        return this.http.post(ServiceConstants.apiCharge + '/updateAll', transaction)
        .pipe(map(result => {
            return true;
        }));
    }

    createFromImage(file:File){
        return this.http.post<Transaction>(ServiceConstants.apiImages, file)
        .pipe(map(result => {
            return result;
        }));
    }

    isImageServiceConfigured(){
        return this.http.get<boolean>(ServiceConstants.apiImages + "/isconfigured")
        .pipe(map(result => {
            return result;
        }));
    }

}

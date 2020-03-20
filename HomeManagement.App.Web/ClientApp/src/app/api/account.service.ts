import { Injectable, EventEmitter, Output } from '@angular/core';
import { User } from "../models/user";
import { Account, AccountPageModel, TransferDto } from "../models/account";
import { ServiceConstants } from './service.constants';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { AuthService } from '../auth/authentication.service';
import { AccountEvolutionModel, AccountsEvolutionModel } from '../models/account-chart-data';
import { pipe, Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable()
export class AccountService {

    user: User;
    accounts: Array<Account> = new Array<Account>();
    pageModel: AccountPageModel;
    @Output() serviceUpdate = new EventEmitter();
    endpoint: string;

    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    constructor(private http: HttpClient,
        private authenticationService: AuthService) {

        this.httpOptions.headers = this.httpOptions.headers.append('Authorization', this.authenticationService.getToken());
        this.endpoint = environment.api + '/api/account';
    }

    getAccount(id:number) : Observable<Account>{

        if(this.pageModel !== undefined && this.pageModel.accounts.length > 0){
            return new Observable(observer => {
                let account = this.pageModel.accounts.find(value => {
                    if(value.id === id)return value;
                });
                observer.next(account);
            });
        }

        return this.http.get<Account>(this.endpoint + '/' + id.toString())
        .pipe(map(result => {
            return result;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    getAllAccounts() : Observable<Array<Account>> {

        if(this.pageModel !== undefined && this.pageModel.accounts.length > 0){
            return new Observable(observer => {
                observer.next(this.pageModel.accounts);
            });
        }

        let model = new AccountPageModel();
        model.userId = this.authenticationService.getUser().id;
        model.currentPage = 1;
        model.pageCount = 50;

        return this.http.post<AccountPageModel>(this.endpoint + '/paging', model, this.httpOptions)
        .pipe(map(result => {
            let pageModel = result;
            this.pageModel = pageModel;
            if(this.accounts.length === 0){
                this.accounts = pageModel.accounts;
            }
            return pageModel.accounts;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    getAccounts(model: AccountPageModel) {

        return this.http.post<AccountPageModel>(this.endpoint + '/paging', model)
        .pipe(map(result => {
            let pageModel = result;
            if(this.accounts.length === 0){
                this.accounts = pageModel.accounts;
            }
            return pageModel;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    getAccountsEvolution() {
        var user = this.authenticationService.getUser();

        return this.http.get<AccountsEvolutionModel>(this.endpoint + '/accountsevolution/' + user.id)
        .pipe(map(result => {
            return result;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    getAccountEvolution(accountId: string) {

        return this.http.get<AccountEvolutionModel>(this.endpoint + '/accountevolution/' + accountId)
        .pipe(map(result => {
            return result;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    import(account: Account, formData: FormData){
        return this.http.post(ServiceConstants.apiCharge + '/upload/' + account.id, formData)
        .pipe(map(result => {
            return true;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    export(account: Account){
        return this.http.get(ServiceConstants.apiCharge + '/download/' + account.id)
        .pipe(map(result => {
            return result;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    add(account:Account){
        return this.http.post(this.endpoint, account)
        .pipe(map(result => {
            this.serviceUpdate.emit();
            return true;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    remove(account:Account){
        return this.http.delete(this.endpoint + '/' + account.id)
        .pipe(map(result => {
            return true;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    transfer(t:TransferDto){
        return this.http.post(this.endpoint + '/transfer', t)
        .pipe(map(result => {
            return true;
        }));
    }

    getAccountTopCharges(account: Account, month: Number){

        return this.http.get(this.endpoint + '/' + account.id + '/toptransactions/' + month)
        .pipe(map(result => {
            return result;
        }));
    }

    update(account:Account){
        return this.http.put(this.endpoint, account, this.httpOptions)
        .pipe(map(result => {
            return true;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

}

import { Injectable, EventEmitter, Output } from '@angular/core';
import { User } from "../../models/user";
import { Account, AccountPageModel, TransferDto } from "../../models/account";
import { ServiceConstants } from '../service.constants';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { AuthService } from '../../auth/authentication.service';
import { AccountEvolutionModel, AccountsEvolutionModel } from '../../models/account-chart-data';
import { pipe, Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Http, RequestOptions, Headers, Response, ResponseContentType } from '@angular/http';

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

    constructor(private httpClient: HttpClient,
        private authenticationService: AuthService,
        private http: Http) {

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

        return this.httpClient.get<Account>(this.endpoint + '/' + id.toString())
        .pipe(map(result => {
            return result;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    getAllAccounts(reload?:boolean) : Observable<Array<Account>> {

        if(!reload && this.pageModel !== undefined && this.pageModel.accounts.length > 0){
            return new Observable(observer => {
                observer.next(this.pageModel.accounts);
            });
        }

        let model = new AccountPageModel();
        model.userId = this.authenticationService.getUser().id;
        model.currentPage = 1;
        model.pageCount = 50;

        return this.httpClient.post<AccountPageModel>(this.endpoint + '/paging', model, this.httpOptions)
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

        return this.httpClient.post<AccountPageModel>(this.endpoint + '/paging', model)
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

        return this.httpClient.get<AccountsEvolutionModel>(this.endpoint + '/accountsevolution/' + user.id)
        .pipe(map(result => {
            return result;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    getAccountEvolution(accountId: string) {

        return this.httpClient.get<AccountEvolutionModel>(this.endpoint + '/accountevolution/' + accountId)
        .pipe(map(result => {
            return result;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    import(account: Account, formData: FormData){
        return this.httpClient.post(ServiceConstants.apiCharge + '/upload/' + account.id, formData)
        .pipe(map(result => {
            return true;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    export(account: Account){
        return this.httpClient.get(ServiceConstants.apiCharge + '/download/' + account.id)
        .pipe(map(result => {
            return result;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    add(account:Account){
        return this.httpClient.post(this.endpoint, account, this.httpOptions)
        .pipe(map(result => {
            this.serviceUpdate.emit();
            return true;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    remove(account:Account){
        return this.httpClient.delete(this.endpoint + '/' + account.id, this.httpOptions)
        .pipe(map(result => {
            return true;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    transfer(t:TransferDto){
        return this.httpClient.post(this.endpoint + '/transfer', t)
        .pipe(map(result => {
            return true;
        }));
    }

    getAccountTopCharges(account: Account, month: Number){

        return this.httpClient.get(this.endpoint + '/' + account.id + '/toptransactions/' + month)
        .pipe(map(result => {
            return result;
        }));
    }

    update(account:Account){
        return this.httpClient.put(this.endpoint, account, this.httpOptions)
        .pipe(map(result => {
            return true;
        }, err => {
            alert(JSON.stringify(err));
        }));
    }

    download(accountId:number){

        let headers = new Headers();

        headers.append('Authorization', this.authenticationService.getToken());

        return this.http.get(environment.api + '/api/Transactions/download/' + accountId,
        {
            responseType: ResponseContentType.Blob,
            headers: headers
        })
        .pipe(map(result => {
            return result.blob();
        }, error => 
        {

        }));
    }

    upload(accountId:number, data: FormData){

        let headers = new Headers();

        headers.append('Authorization', this.authenticationService.getToken());

        return this.http.post(environment.api + '/api/Transactions/upload/' + accountId, data,
        {
            headers: headers
        })
        .pipe(map(result => {
            return true;
        }, error => 
        {

        }));
    }
}

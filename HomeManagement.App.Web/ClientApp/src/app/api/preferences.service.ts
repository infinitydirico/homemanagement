import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { AuthService } from '../auth/authentication.service';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { PreferredPreference } from '../models/preference';

@Injectable()
export class PreferencesService {

    endpoint: string;
    preferredCurrency: PreferredPreference;

    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    constructor(private http: HttpClient,
        private authenticationService: AuthService) {

        this.httpOptions.headers = this.httpOptions.headers.append('Authorization', this.authenticationService.getToken());
        this.endpoint = environment.api + '/api/preferences';
    }

    getCurrencyAndLanguage() : Observable<PreferredPreference> {

        if(this.preferredCurrency !== undefined){
            return new Observable<PreferredPreference>(obs => obs.next(this.preferredCurrency));
        }

        return this.http.get<PreferredPreference>(this.endpoint, this.httpOptions)
        .pipe(map(_ => {
            this.preferredCurrency = _;
            return _;
        }, err => {

        }));
    }

    updateLanguage(lang:string){

        return this.http.post(this.endpoint + '/changelanguage/' + lang, this.httpOptions, null)
        .pipe(map(_ => {
            return true;
        }, err => {

        }));
    }

    updatePreferredCurrency(currency:string){
        return this.http.post(this.endpoint + '/changepreferredcurrency/' + currency, this.httpOptions, null)
        .pipe(map(_ => {
            return true;
        }, err => {
        }));
    }

    saveCountry(country:string){
        return this.http.post(this.endpoint + '/savecountry/' + country, this.httpOptions, null)
        .pipe(map(_ => {
            return true;
        }, err => {
        }));
    }

    downloadUserData()
    {
        return this.http.get(this.endpoint + '/downloaduserdata', {
            responseType: 'blob',
            headers: this.httpOptions.headers
        });
    }

}
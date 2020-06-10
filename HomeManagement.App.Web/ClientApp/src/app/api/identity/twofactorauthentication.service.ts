import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../auth/authentication.service';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable()
export class TwoFactorAuthenticationService {

    @Output() serviceUpdate = new EventEmitter();
    endpoint: string;

    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    
    constructor(private http: HttpClient,
        private authenticationService: AuthService) {

        this.httpOptions.headers = this.httpOptions.headers.append('Authorization', this.authenticationService.getToken());
        this.endpoint = environment.identityApi + '/api/twofactorauthentication';
    }

    isEnabled(){
        return this.http.get(this.endpoint + '/IsEnabled', this.httpOptions)
        .pipe(map(result => {
            return result;
        }, error => {

        }));
    }

    Enable(){
        return this.http.post(this.endpoint + '/Enable', null, this.httpOptions)
        .pipe(map(result => {
            return result;
        }, error => {

        }));
    }

    Disable(){
        return this.http.post(this.endpoint + '/Disable', null, this.httpOptions)
        .pipe(map(result => {
            return result;
        }, error => {

        }));
    }
}
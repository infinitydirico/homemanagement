import { Injectable, EventEmitter, Output, Directive } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../auth/authentication.service';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Directive()
@Injectable()
export class IdentityService {

    @Output() serviceUpdate = new EventEmitter();
    endpoint: string;

    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    
    constructor(private http: HttpClient,
        private authenticationService: AuthService) {

        this.httpOptions.headers = this.httpOptions.headers.append('Authorization', this.authenticationService.getToken());
        this.endpoint = environment.identityApi + '/api/passwordmanagement';
    }

    forgotPassword(email:string){
        let model = {
            email: email
        };

        return this.http.post(this.endpoint + '/forgotpassword', model, this.httpOptions)
        .pipe(map(result => {
            return true;
        }, error => {

        }));
    }

    tokenPasswordChange(email:string, token:string, newPassword:string){
        let model = {
            email: email,
            token: token,
            newPassword: newPassword
        };

        return this.http.post(this.endpoint + '/tokenpasswordchange', model, this.httpOptions)
        .pipe(map(result => {
            return true;
        }, error => {

        }));        
    }
}
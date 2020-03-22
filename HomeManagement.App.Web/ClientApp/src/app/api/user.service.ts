import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../auth/authentication.service';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable()
export class UserService {

    @Output() serviceUpdate = new EventEmitter();
    endpoint: string;

    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    
    constructor(private http: HttpClient,
        private authenticationService: AuthService) {

        this.httpOptions.headers = this.httpOptions.headers.append('Authorization', this.authenticationService.getToken());
        this.endpoint = environment.api + '/api/user';
    }

    downloadUserData()
    {
        let options = {
            headers: new HttpHeaders(
                { 
                    'Content-Type': 'blob',
                    'Authorization': this.authenticationService.getToken()
                })
        };
        
        return this.http.get(this.endpoint + '/downloaduserdata', options)
        .pipe(map(result => {
            return result;
        }, error => {

        }));
    }
}
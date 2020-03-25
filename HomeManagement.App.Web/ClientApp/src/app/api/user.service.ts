import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../auth/authentication.service';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { Http, RequestOptions, Headers, Response, ResponseContentType } from '@angular/http';

@Injectable()
export class UserService {

    @Output() serviceUpdate = new EventEmitter();
    endpoint: string;

    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    
    constructor(private http: Http,
        private authenticationService: AuthService) {

        this.httpOptions.headers = this.httpOptions.headers.append('Authorization', this.authenticationService.getToken());
        this.endpoint = environment.api + '/api/user';
    }

    downloadUserData()
    {
        let headers = new Headers();

        headers.append('Authorization', this.authenticationService.getToken());
        
        return this.http.get(this.endpoint + '/downloaduserdata', 
        {
             responseType: ResponseContentType.Blob,
             headers: headers
        })
        .pipe(map(result => {
            return result.blob();
        }, error => {

        }));
    }
}
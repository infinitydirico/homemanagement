import { Injectable } from '@angular/core';
import { Notification, sampleNotifications } from "../../models/notification";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './../../auth/authentication.service'
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable()
export class NotificationService {

    endpoint: string;
    notifications: Array<Notification> = [];

    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    constructor(private http: HttpClient,
        private authenticationService: AuthService) {

        this.endpoint = environment.api + '/api/notification';
    }

    get() : Observable<Array<Notification>> {

        if(this.notifications.length > 0){
            return new Observable(obs => obs.next(this.notifications));
        }

        this.httpOptions.headers = this.httpOptions.headers.append('Authorization', this.authenticationService.getToken());

        return this.http.get<Array<Notification>>(this.endpoint, this.httpOptions)
        .pipe(map(_ => {
            _.forEach(c => {
                this.notifications.push(c);
            })
            return this.notifications;
        }));
    }

    dismiss(n: Notification){
        this.notifications.splice(0, this.notifications.length);

        return this.http.put(this.endpoint, n, this.httpOptions)
        .pipe(map(_ => {
            return true;
        }));
    }

}

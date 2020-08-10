import { Injectable } from '@angular/core';
import { Reminder } from "../../models/reminder";
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { AuthService } from './../../auth/authentication.service'
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable()
export class ReminderService {

    endpoint: string;

    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    constructor(private http: HttpClient,
        private authenticationService: AuthService) {

        this.httpOptions.headers = this.httpOptions.headers.append('Authorization', this.authenticationService.getToken());
        this.endpoint = environment.api + '/api/reminder';
    }

    get() : Observable<Array<Reminder>>{
        return this.http.get<Array<Reminder>>(this.endpoint, this.httpOptions)
        .pipe(map(_ =>{
            return _
        }));
    }

    addReminder(reminder:Reminder){
        return this.http.post(this.endpoint, reminder, this.httpOptions).
        pipe(map(_ =>{
            return true;
        }));
    }

    updateReminder(reminder:Reminder){
        return this.http.put(this.endpoint, reminder, this.httpOptions)
        .pipe(map(_ =>{
            return true;
        }));
    }

    deleteReminder(reminder:Reminder){
        return this.http.delete(this.endpoint + '/' + reminder.id, this.httpOptions)
        .pipe(map(_ =>{
            return true;
        }));
    }

}

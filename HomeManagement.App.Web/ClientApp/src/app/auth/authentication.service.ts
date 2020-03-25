import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { EndpointsService } from '../endpoints.service';
import { HttpClient } from "@angular/common/http";
import { CryptoService } from '../services/crypto.service';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable()
export class AuthService {

    private user:User;
    onUserAuthenticated = new Subject<User>();

    constructor(private router: Router, 
        private endpointsService: EndpointsService,
        private http: HttpClient,
        private cryptoService: CryptoService) {        
    }

    login(username:string, password:string){

        let endpoint = environment.identityApi;        
        this.user = new User();
        this.user.email = username;
        this.user.password = this.cryptoService.encrypt(password);
        return this.http.post<User>(endpoint + "/Api/Authentication/SignIn", this.user)
            .pipe(map(result => {
                this.user = result;
                this.onUserAuthenticated.next(this.user);
                this.router.navigate(['/']);
            }, err => {
                return err;
            }));

        //this.http.post<User>(endpoint + "/Api/Authentication/SignIn", this.user).subscribe(result => {
        //    this.user = result;
        //    this.onUserAuthenticated.next(this.user);
        //    this.router.navigate(['/']);
        //});        
    }

    logout(){
        this.user = null;
        this.onUserAuthenticated.next(this.user);
        this.router.navigate(['/login']);
    }

    isAuthenticated(){
        return this.user != null;
    }

    getToken(){
        if(this.isAuthenticated()) return this.user.token;
        else return "";
    }

    getUser(){
        return this.user;
    }
}
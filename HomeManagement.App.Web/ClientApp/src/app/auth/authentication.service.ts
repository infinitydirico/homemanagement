import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { HttpClient } from "@angular/common/http";
import { CryptoService } from '../services/crypto.service';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { CacheService } from '../services/cache.service';

@Injectable()
export class AuthService {

    cacheKey: string = "auth";
    private user:User;
    onUserAuthenticated = new Subject<User>();

    constructor(private router: Router, 
        private http: HttpClient,
        private cryptoService: CryptoService,
        private cacheService: CacheService) {
            
        if(this.cacheService.exists(this.cacheKey)){
            this.user = this.cacheService.get<User>(this.cacheKey);            
        }
    }

    login(username:string, password:string, remember?:boolean){

        let endpoint = environment.identityApi;        
        this.user = new User();
        this.user.email = username;
        this.user.password = this.cryptoService.encrypt(password);
        return this.http.post<User>(endpoint + "/Api/Authentication/SignIn", this.user)
            .pipe(map(result => {
                this.user = result;

                if(remember){
                    this.cacheService.save(this.cacheKey, this.user);                    
                }

                this.onUserAuthenticated.next(this.user);
                this.router.navigate(['/']);
            }, err => {
                return err;
            }));     
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
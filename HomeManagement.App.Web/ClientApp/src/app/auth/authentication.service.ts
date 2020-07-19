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

            if(new Date(this.user.expirationDate) < new Date()){
                this.user = null;
            }
        }
    }

    login(username:string, password:string, securityCode?:number, remember?:boolean){

        let endpoint = environment.identityApi;        
        this.user = new User();
        this.user.email = username;
        this.user.securityCode = securityCode || 0;
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

    register(username:string, password:string){
        let endpoint = environment.identityApi;        
        this.user = new User();
        this.user.email = username;
        this.user.securityCode = 0;
        this.user.password = this.cryptoService.encrypt(password);

        return this.http.post(endpoint + "/Api/Registration", this.user)
            .pipe(map(() => {

                this.login(username, password).subscribe();
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
        return this.user != null && new Date(this.user.expirationDate) > new Date();
    }

    getToken(){
        if(this.isAuthenticated()) return this.user.token;
        else return "";
    }

    getUser(){
        return this.user;
    }
}
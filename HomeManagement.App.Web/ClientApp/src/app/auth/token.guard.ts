import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from "./authentication.service";
import { Observable } from 'rxjs';

@Injectable()
export class TokenGuard implements CanActivate {

    value:string;
    constructor(private router: Router, private authService: AuthService) { }

    canActivate(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        
        if(!this.hasValue(childRoute)){
            this.router.navigate(['/login']);

            return false;
        }

        if(!this.isValid()) {
            this.router.navigate(['/login']);

            return false;
        }
        
        return true;
    }

    hasValue(childRoute: ActivatedRouteSnapshot){
        if(!childRoute.queryParams.hasOwnProperty('value')) return false;

        this.value = childRoute.queryParams.value;

        return true;
    }

    isValid(){
        return this.value.length > 100;
    }
}
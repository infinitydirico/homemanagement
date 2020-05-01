import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AuthService } from "../auth/authentication.service";

@Injectable()
export class ApiService {

    constructor(private http: HttpClient, 
        private authenticationService: AuthService){  
    }
    
    public addTokenHeaders():HttpHeaders{

        let token = this.authenticationService.getToken();
        
        let headers = new HttpHeaders();

        headers.append('Authorization', token);
        headers.append('Content-Type', 'application/json');
        return headers;
    }

    public getHeaders(){
        var options = { headers: this.addTokenHeaders()};
        return options;
    }    
}
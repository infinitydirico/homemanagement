import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Injectable()
export class EndpointsService {

    endpoints: Array<Endpoint> = new Array<Endpoint>();

    constructor(private http: HttpClient){
        this.load(); 
    }

    getIdentityServiceEndpoint(){
        return this.searchEndpoint("Identity");
    }

    getHomeManagementApiEndpoint(){
        return this.searchEndpoint("HomeManagement");
    }

    private searchEndpoint(endpoint: string){
        let e = this.endpoints.find(e => {
            if(e.key.includes(endpoint)) return e;            
        });

        if(e !== undefined){
            return e.value;
        }

        return "";
    }

    private load(){
        return this.http.get<Array<Endpoint>>(location.origin + '/endpoints').subscribe(result => {
            this.endpoints = result;
          }, error => console.error(error));
    }
}

export class Endpoint{
    key:string;
    value:string;
}
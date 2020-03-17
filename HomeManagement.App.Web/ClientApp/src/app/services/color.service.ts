import { Injectable } from "@angular/core";

@Injectable()
export class ColorService {

    getPrimary() {
        return '#0275d8';
    }

    getSuccess(){
        return '#5cb85c';
    }

    getInfo(){
        return '#5bc0de';
    }

    getWarning(){
        return '#f0ad4e';
    }

    getDanger(){
        return '#d9534f';
    }
}
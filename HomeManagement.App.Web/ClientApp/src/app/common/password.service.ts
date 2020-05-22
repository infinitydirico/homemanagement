import { Injectable } from "@angular/core";

@Injectable()
export class PasswordService {

    public isStrong(password:string){
        let hasNumber = false;
        let hasUpperCase = false;
        let hasLowerCase = false;
        let hasSpecialCharacter = false;
        let isGreaterThanSix = false;

        for (let index = 0; index < password.length; index++) {
            let code = password.charCodeAt(index);

            if(index >= 6){
                isGreaterThanSix = true;
            }

            if(this.isUpperCase(code)){
                hasUpperCase = true;
            }

            if(this.isLowerCase(code)){
                hasLowerCase = true;
            }

            if(this.isNumeric(code)){
                hasNumber = true;
            }

            if(this.isSpecialCharacter(code)){
                hasSpecialCharacter = true;
            }

            if(isGreaterThanSix && hasUpperCase && hasLowerCase && hasNumber && hasSpecialCharacter){
                return true;
            }
        }

        return false;
    }

    private isUpperCase(code:number){
        return code >= 65 && code <= 90;
    }

    private isLowerCase(code:number){
        return code >= 97 && code <= 122;
    }

    private isNumeric(code:number){
        return code >= 48 && code <= 57;
    }

    private isSpecialCharacter(code:number){
        return (code >= 33 && code <= 47) || (code >= 58 && code <= 64) ||(code >= 91 && code <= 96) || (code >= 123 && code <= 126)
    }
}
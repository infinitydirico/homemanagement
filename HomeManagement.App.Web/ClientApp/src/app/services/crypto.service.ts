import { Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';

@Injectable()
export class CryptoService {

    cryptoValue: string = '9126486423168794';
    key:any = CryptoJS.enc.Utf8.parse(this.cryptoValue);
    iv:any = CryptoJS.enc.Utf8.parse(this.cryptoValue);

    encrypt(value: string){
        var encrypted = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(value), this.key, this.getConfigOptions());

        return encrypted.toString();
    }

    decrypt(value: string){
        var decrypted = CryptoJS.AES.decrypt(value, this.key, this.getConfigOptions());

        return decrypted.toString(CryptoJS.enc.Utf8);
    }

    private getConfigOptions():any{
        return  {
            keySize: 128 / 8,
            iv: this.iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        };
    }
}
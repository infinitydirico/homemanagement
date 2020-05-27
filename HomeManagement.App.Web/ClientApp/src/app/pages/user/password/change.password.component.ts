import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ColorService } from 'src/app/services/color.service';
import { PasswordService } from 'src/app/common/password.service';

@Component({
  selector: 'change-password',
  templateUrl: './change.password.component.html'
})
export class ChangePasswordComponent {

  newPassword:string;
  repeatNewPassword:string;

  oldPasswordFormControl: FormControl = new FormControl('', [
    Validators.required
  ]);

  newPasswordFormControl: FormControl = new FormControl('', [
    Validators.required
  ]);

  repeatNewPasswordFormControl: FormControl = new FormControl('', [
    Validators.required
  ]);

  constructor(public colorService: ColorService, public passwordService: PasswordService) {
    
  }

  passwordMatches(){
    return this.newPassword != this.repeatNewPassword;
  }

  isStrong(){
      return this.passwordService.isStrong(this.newPassword);
  }
}
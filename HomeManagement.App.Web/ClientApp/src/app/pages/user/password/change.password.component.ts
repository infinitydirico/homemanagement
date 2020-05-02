import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ColorService } from 'src/app/services/color.service';

@Component({
  selector: 'change-password',
  templateUrl: './change.password.component.html'
})
export class ChangePasswordComponent {

  oldPasswordFormControl: FormControl = new FormControl('', [
    Validators.required
  ]);

  newPasswordFormControl: FormControl = new FormControl('', [
    Validators.required
  ]);

  repeatNewPasswordFormControl: FormControl = new FormControl('', [
    Validators.required
  ]);

  constructor(public colorService: ColorService) {
    
  }
}
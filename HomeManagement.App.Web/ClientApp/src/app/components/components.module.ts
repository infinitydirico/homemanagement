import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation.dialog.component';


@NgModule({
  imports: [
    CommonModule,
  ],
  providers: [
    ConfirmationDialogComponent],
  exports: [
    ConfirmationDialogComponent
  ],
  entryComponents: [
    ConfirmationDialogComponent
  ]
})
export class ComponentsModule { }

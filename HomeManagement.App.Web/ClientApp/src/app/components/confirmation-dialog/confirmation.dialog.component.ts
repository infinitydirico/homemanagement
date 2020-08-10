import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'confirmation-dialog',
  templateUrl: './confirmation.dialog.component.html'
})
export class ConfirmationDialogComponent {

    constructor(
        public dialogRef: MatDialogRef<ConfirmationDialogComponent>) {}
    
      onNoClick(): void {
        this.dialogRef.close(false);
      }
}
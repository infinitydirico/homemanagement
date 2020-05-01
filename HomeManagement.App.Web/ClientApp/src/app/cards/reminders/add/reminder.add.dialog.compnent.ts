import { Component, Inject } from "@angular/core";
import { Reminder } from "src/app/models/reminder";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material";

@Component({
    selector: 'reminder-dialog',
    templateUrl: 'reminder.add.dialog.compnent.html',
    styleUrls: ['reminder.add.dialog.compnent.css']
})
export class ReminderAddDialog {

    constructor(
        public dialogRef: MatDialogRef<ReminderAddDialog>,
        @Inject(MAT_DIALOG_DATA) public reminder: Reminder,
        private snackBar: MatSnackBar) {
            this.reminder = new Reminder();
        }

    onNoClick(): void {
        this.dialogRef.close();
    }

    ok(){
        if(this.reminder.title === undefined || this.reminder.title === ''){
            this.snackBar.open('Title cannot be empty', 'ok', {
                duration: 2000
            });
            return;
        }

        if(this.reminder.dueDay === undefined || this.reminder.dueDay < 1 || this.reminder.dueDay > 30){
            this.snackBar.open('Due day cannot be less than 1 and greater than 30', 'ok', {
                duration: 2000
            });
            return;
        }

        this.dialogRef.close(this.reminder);
    }
}
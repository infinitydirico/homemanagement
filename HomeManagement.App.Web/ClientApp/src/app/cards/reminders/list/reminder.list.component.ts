import { Component, Input, OnInit } from '@angular/core';
import { ReminderService } from 'src/app/api/main/reminder.service';
import { Reminder } from 'src/app/models/reminder';
import { ReminderAddDialog } from '../add/reminder.add.dialog.compnent';
import { MatDialog } from '@angular/material/dialog';
import { ColorService } from 'src/app/services/color.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
    selector: 'reminder-list',
    templateUrl: './reminder.list.component.html',
    styleUrls: ['reminder.list.component.css']
})
export class ReminderListComponent implements OnInit {

    reminders: Array<Reminder> = new Array<Reminder>();
    displayedColumns: string[] = ['active', 'title', 'dueDay', 'delete'];

    constructor(private reminderService: ReminderService,
        public dialog: MatDialog,
        private colorService: ColorService,
        private snackBar: MatSnackBar) {

    }

    ngOnInit(): void {
        this.reminders.splice(0, this.reminders.length);

        this.reminderService.get().subscribe(result => {
            result.forEach(r => {
                this.reminders.push(r);
            });
        });
    }

    add() {
        let reminderDialog = this.dialog.open(ReminderAddDialog, {
            width: '250px'
        })

        reminderDialog.afterClosed().subscribe(reminder => {
            
            this.reminderService.addReminder(reminder).subscribe(result => {
                this.ngOnInit();
            });
        });
    }

    delete(reminder:Reminder){
        this.reminderService.deleteReminder(reminder).subscribe(result => {
            this.ngOnInit();
        });
    }

    activeChanged(reminder:Reminder){
        this.reminderService.updateReminder(reminder).subscribe(result => {
            this.snackBar.open('Reminder ' + reminder.title + ' updated !', 'ok', {
                duration: 1000
            });
        });
    }
}
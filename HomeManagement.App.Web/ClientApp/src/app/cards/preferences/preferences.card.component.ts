import { Component, OnInit } from '@angular/core';
import { PreferencesService } from 'src/app/api/main/preferences.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Preference } from 'src/app/models/preference';

@Component({
  selector: 'preferences',
  templateUrl: './preferences.card.component.html'
})
export class PreferencesCardComponent implements OnInit {

    weeklyBackup: Preference;
    weeklyBackupEnabled: boolean = false;

    dailyBackup: Preference;
    dailyBackupEnabled: boolean = false;

    constructor(
        private preferencesService: PreferencesService,
        private snackBar: MatSnackBar){
            
        this.weeklyBackup = new Preference();
        this.weeklyBackup.key = 'EnableWeeklyEmails';
        this.weeklyBackup.value = 'false';

        this.dailyBackup = new Preference();
        this.dailyBackup.key = 'EnableDailyBackups';
        this.dailyBackup.value = 'false';
    }

    ngOnInit(): void {
        this.preferencesService.get('EnableWeeklyEmails').subscribe(result => {
            this.weeklyBackup = result;
            
            this.weeklyBackupEnabled = this.weeklyBackup.value === 'true';
        });

        this.preferencesService.get('EnableDailyBackups').subscribe(result => {
            this.dailyBackup = result;
            
            this.dailyBackupEnabled = this.dailyBackup.value === 'true';
        });
    }

    onWeeklyBackupChanged(){
        this.weeklyBackup.value = String(this.weeklyBackupEnabled);

        this.preferencesService.save(this.weeklyBackup).subscribe(_ => {
            var message = 'Weekly email backup has been ' + (this.weeklyBackupEnabled ? 'enabled' : 'disabled');
            this.snackBar.open(message, 'ok',
            {
                duration: 2000
            });
        });
    }

    onDailyBackupChanged(){
        this.dailyBackup.value = String(this.dailyBackupEnabled);

        this.preferencesService.save(this.dailyBackup).subscribe(_ => {
            var message = 'Daily backup has been ' + (this.dailyBackupEnabled ? 'enabled' : 'disabled');
            this.snackBar.open(message, 'ok',
            {
                duration: 2000
            });
        });
    }
}
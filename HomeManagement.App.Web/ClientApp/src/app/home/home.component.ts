import { Component, OnInit } from '@angular/core';
import { PreferencesService } from '../api/preferences.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  constructor(private preferencesService: PreferencesService){

  }
  ngOnInit(): void {
    this.preferencesService.getCurrencyAndLanguage().subscribe();
  }
}

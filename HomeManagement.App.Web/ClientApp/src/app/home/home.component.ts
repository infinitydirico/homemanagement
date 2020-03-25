import { Component, OnInit } from '@angular/core';
import { PreferencesService } from '../api/preferences.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  data: Array<number> = [55,32,78,95,35];
  labels: Array<string> = ["Marzo", "Abril", "Mayo", "Junio", "Julio"];

  constructor(private preferencesService: PreferencesService){

  }
  ngOnInit(): void {
    this.preferencesService.getCurrencyAndLanguage().subscribe();
  }
}

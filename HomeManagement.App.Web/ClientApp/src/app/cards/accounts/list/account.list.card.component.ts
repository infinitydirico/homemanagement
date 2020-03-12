import { Component, OnInit } from '@angular/core';
import { Account } from '../../../models/account';

@Component({
  selector: 'accounts-list',
  templateUrl: './account.list.card.component.html',
  styleUrls: ['./account.list.card.component.css']
})
export class AccountListCardComponent implements OnInit {

  ngOnInit(): void {
    
    let account = new Account();
    account.name = "HSBC";
    this.dataSource.push(account);

    let a = new Account();
    a.name = "BBVA";
    this.dataSource.push(a);
  }

  displayedColumns: string[] = ['name'];
  dataSource: Array<Account> = [];
}
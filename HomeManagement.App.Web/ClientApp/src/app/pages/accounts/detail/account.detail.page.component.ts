import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/api/main/account.service';
import { ActivatedRoute } from '@angular/router';
import { Account } from "../../../models/account";

@Component({
  selector: 'account-detail',
  templateUrl: './account.detail.page.component.html'
})
export class AccountDetailComponent implements OnInit {

  account: Account;

  constructor(private accountService: AccountService,
    private route: ActivatedRoute){
      
  }

  ngOnInit(): void {
    let id = +this.route.snapshot.params.id;

    this.accountService.getAccount(id).subscribe(result => {
      this.account = result;
    });
  }

}
import { Component, OnInit, Input } from '@angular/core';
import { AccountService } from 'src/app/api/main/account.service';
import { ActivatedRoute, ParamMap, Params } from '@angular/router';
import { switchMap, map } from 'rxjs/operators';
import { Account } from '../../models/account';

@Component({
  selector: 'account',
  templateUrl: './account.component.html'
})
export class AccountComponent implements OnInit {

    account: Account;

    constructor(private accountService: AccountService,
      private route: ActivatedRoute){
        
    }

    ngOnInit(): void {
      let id = +this.route.firstChild.snapshot.params.id;

      this.accountService.getAccount(id).subscribe(result => {
        this.account = result;
      });
    }

}
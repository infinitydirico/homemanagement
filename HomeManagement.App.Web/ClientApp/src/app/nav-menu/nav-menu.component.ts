import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/authentication.service';
import { Router } from '@angular/router';
import { saveAs } from 'file-saver';
import { UserService } from '../api/user.service';
import { CommonService } from '../common/common.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  isExpanded = false;
  isAuthenticated = false;
  isMobile = false;

  constructor(private authenticationService: AuthService,
    private router:Router,
    private userService: UserService,
    private commonService: CommonService){    
  }

  ngOnInit(): void {
    this.isMobile = this.commonService.isMobile();

    this.authenticationService.onUserAuthenticated.subscribe(user =>
    {
      this.isAuthenticated = user != null;
    });
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout(){
    this.authenticationService.logout();
  }

  backToHome(){
    this.router.navigate(['/']);
  }

  viewAccount(){
    this.router.navigate(['/user']);
  }

  downloadUserInfo(){
    this.userService.downloadUserData().subscribe(result => {
      saveAs(result, "userdata.zip");
    });    
  }
}

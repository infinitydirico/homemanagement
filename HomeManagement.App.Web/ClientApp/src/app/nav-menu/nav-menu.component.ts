import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/authentication.service';
import { Router } from '@angular/router';
import { saveAs } from 'file-saver';
import { UserService } from '../api/user.service';
import { CommonService } from '../common/common.service';
import { NotificationService } from '../api/notification.service';
import { MatBottomSheet } from '@angular/material';
import { NotificationsBottomBarComponent } from '../components/notifications-bottom-bar/notifications.bar.component';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  isExpanded = false;
  isAuthenticated = false;
  isMobile = false;
  hasNotifications = false;

  constructor(private authenticationService: AuthService,
    private router:Router,
    private userService: UserService,
    private commonService: CommonService,
    private notificationService: NotificationService,
    private bottomSheet: MatBottomSheet){    
  }

  ngOnInit(): void {
    this.isMobile = this.commonService.isMobile();

    this.isAuthenticated = this.authenticationService.isAuthenticated();

    this.authenticationService.onUserAuthenticated.subscribe(user =>
    {
      this.isAuthenticated = user != null;
    });

    this.notificationService.get().subscribe(_ => {
      this.hasNotifications = _.length > 0;
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

  register(){

  }

  login(){
    this.router.navigate(['/login']);
  }

  viewNotifications(){
    this.bottomSheet.open(NotificationsBottomBarComponent,{
      panelClass: 'bottom-sheet'
    });
  }

  changePassword(){
    this.router.navigate(['/changepassword']);
  }
}

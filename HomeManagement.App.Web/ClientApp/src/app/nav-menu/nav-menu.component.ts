import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  isExpanded = false;
  isAuthenticated = false;

  constructor(private authenticationService: AuthService,
    private router:Router){    
  }

  ngOnInit(): void {
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
}

import { BrowserModule } from '@angular/platform-browser';
import { NgModule, Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { HttpModule } from "@angular/http";

//custom modules
import { CardsModule } from './cards/cards.module';
import { ApiModule } from './api/api.module';
import { MaterialModule } from './materials.module';

//components
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';

//auth
import { AuthService } from './auth/authentication.service';
import { AuthGuard } from "./auth/auth.guard";

//services
import { EndpointsService } from './endpoints.service';
import { CryptoService } from './services/crypto.service';
import { ColorService } from './services/color.service';

//pages
import { HomeComponent } from './pages/home/home.component';
import { AccountComponent } from './pages/accounts/account.component';
import { AccountDetailComponent } from './pages/accounts/detail/account.detail.page.component';
import { LoginComponent } from './pages/login/login.component';
import { HelperService } from './services/helper.service';
import { UserComponent } from './pages/user/user.page.component';
import { CommonService } from './common/common.service';
import { DateService } from './common/date.service';
import { PaletteService } from './services/palette.service';
import { CacheService } from './services/cache.service';
import { NotificationsBottomBarComponent } from './components/notifications-bottom-bar/notifications.bar.component';
import { MatMenuModule } from '@angular/material/menu';
import { TokenGuard } from './auth/token.guard';
import { TokenPageComponent } from './pages/user/token/token.page.component';
import { PasswordService } from './common/password.service';
import { SecuritySettingsComponent } from './pages/user/security-settings/security-settings.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    UserComponent,
    AccountComponent,
    AccountDetailComponent,
    NotificationsBottomBarComponent,
    TokenPageComponent,
    SecuritySettingsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([      
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'login', component: LoginComponent},
      { 
        path: 'account', 
        component: AccountComponent,
        children: [
          { path: ':id', component: AccountDetailComponent}
        ],
        canActivate: [AuthGuard]
      },
      { path: 'user', component: UserComponent, canActivate: [AuthGuard]},
      { path: 'security-settings', component: SecuritySettingsComponent, canActivate: [AuthGuard]},
      { path: 'token', component: TokenPageComponent, canActivate: [TokenGuard]}
    ]),
    BrowserModule,
    MaterialModule,
    CardsModule,
    ApiModule,
    HttpModule,
    MatMenuModule,
    ReactiveFormsModule    
  ],
  providers: [
    AuthGuard,
    TokenGuard,
    AuthService,
    EndpointsService,
    CryptoService,
    ColorService,
    ApiModule,
    HelperService,
    CommonService,
    DateService,
    PaletteService,
    CacheService,
    PasswordService,
  ],
  bootstrap: [AppComponent],
  entryComponents: [
    NotificationsBottomBarComponent
]
})
export class AppModule { }

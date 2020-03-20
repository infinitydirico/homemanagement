import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

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
import { HomeComponent } from './home/home.component';
import { AccountComponent } from './pages/accounts/account.component';
import { AccountDetailComponent } from './pages/accounts/detail/account.detail.page.component';
import { LoginComponent } from './login/login.component';
import { HelperService } from './services/helper.service';
import { UserComponent } from './pages/user/user.page.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    UserComponent,
    AccountComponent,
    AccountDetailComponent
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
    ]),
    BrowserModule,
    MaterialModule,
    CardsModule,
    ApiModule
  ],
  providers: [
    AuthGuard,
    AuthService,
    EndpointsService,
    CryptoService,
    ColorService,
    ApiModule,
    HelperService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

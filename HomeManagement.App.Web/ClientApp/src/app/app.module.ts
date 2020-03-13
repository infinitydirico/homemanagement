import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

//custom modules
import { CardsModule } from './cards/cards.module';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { MaterialModule } from './materials.module';
import { LoginComponent } from './login/login.component';

//auth
import { AuthService } from './auth/authentication.service';
import { AuthGuard } from "./auth/auth.guard";
import { EndpointsService } from './endpoints.service';
import { CryptoService } from './services/crypto.service';
import { ApiModule } from './api/api.module';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([      
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'counter', component: CounterComponent, canActivate: [AuthGuard] },
      { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthGuard] },
      { path: 'login', component: LoginComponent}
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
    ApiModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

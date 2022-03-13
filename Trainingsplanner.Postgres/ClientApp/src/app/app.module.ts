import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { TrainingsExerciseClient, TrainingsGroupClient, UserClient } from '../clients/api.generated.clients';
import { ConfigurationService } from './services/configuration.service';
import { AuthorizeService } from '../api-authorization/authorize.service';
import { GroupSelectComponent } from './Gruppen/group-select/group-select.component';
import { GroupPageComponent } from './Gruppen/group-page/group-page.component';
import { GroupInfoComponent } from './Gruppen/group-info/group-info.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AdminComponent } from './admin/admin.component';

const appInitializerFn = (appConfig: ConfigurationService) => {
  return () => {
    return appConfig.loadConfig();
  };
};

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    GroupSelectComponent,
    GroupPageComponent,
    GroupInfoComponent,
    AdminComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthorizeGuard] },
      { path: 'groups', component: GroupSelectComponent, canActivate: [AuthorizeGuard] },
      { path: 'admin', component: AdminComponent, canActivate: [AuthorizeGuard] }
    ]),
    NgbModule,
    CalendarModule.forRoot({ provide: DateAdapter, useFactory: adapterFactory }),
    BrowserAnimationsModule
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: appInitializerFn,
      multi: true,
      deps: [ConfigurationService]
    },
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
     {
      provide: TrainingsExerciseClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizeService
      ) => {
        return new TrainingsExerciseClient(authorizationService, http, config.apiAddress);
      },
       deps: [HttpClient, ConfigurationService, AuthorizeService]
    },
    {
      provide: TrainingsGroupClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizeService
      ) => {
        return new TrainingsExerciseClient(authorizationService, http, config.apiAddress);
      },
      deps: [HttpClient, ConfigurationService, AuthorizeService]
    },
    {
      provide: UserClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizeService
      ) => {
        return new TrainingsExerciseClient(authorizationService, http, config.apiAddress);
      },
      deps: [HttpClient, ConfigurationService, AuthorizeService]
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

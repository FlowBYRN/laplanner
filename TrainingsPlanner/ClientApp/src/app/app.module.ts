import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { APP_INITIALIZER, ChangeDetectorRef, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { TrainingsAppointmentClient, TrainingsExerciseClient, TrainingsGroupClient, TrainingsGroupUserClient, TrainingsModuleClient, TrainingsModuleTagClient } from 'src/clients/api.generated.clients';
import { GroupPageComponent } from '../app/Gruppen/group-page/group-page.component';
import { ShedulerPageComponent } from '../app/Kalendar/sheduler-page/sheduler-page.component';
import { TrainingPageComponent } from '../app/Trainingsmodule/training-page/training-page.component';
import { ModuleplannerPageComponent } from '../app/Trainingsmodule/moduleplanner-page/moduleplanner-page.component';
import { AccountPageComponent } from './Benutzer/account-page/account-page.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ModulefilterPipe } from './pipes/modulefilter.pipe';
import { DraggableTrainingsModuleComponent } from './angular-modules/draggable-trainings-module/draggable-trainings-module.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CalenderHeaderComponent } from './Kalendar/calender-header/calender-header.component';
import { CallbackComponent } from './authorization/callback/callback.component';
import { CreateExerciseModuleComponent } from './angular-modules/create-exercise-module/create-exercise-module.component';
import { TrainingOverviewComponent } from './Trainingsmodule/training-overview/training-overview.component';
import { UserDropdownComponent } from './authorization/user-dropdown/user-dropdown.component';
import { RegistrationClient, UserClient } from 'src/clients/is4.generated.clients';
import { RegistrationPageComponent } from './Benutzer/registration-page/registration-page.component';
import { GroupInfoComponent } from './Gruppen/group-info/group-info.component';
import { GroupSelectComponent } from './Gruppen/group-select/group-select.component';
import { AuthorizationService } from './services/authorization.service';
import { ConfigurationService } from './services/configuration.service';
import { CalenderOverviewComponent } from './Kalendar/calender-overview/calender-overview.component';
import { AuthenticationGuard } from './guards/authentication.guard';
import { TrainerGuard } from './guards/trainer.guard';

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
    GroupPageComponent,
    ShedulerPageComponent,
    TrainingPageComponent,
    ModuleplannerPageComponent,
    AccountPageComponent,
    ModulefilterPipe,
    DraggableTrainingsModuleComponent,
    CalenderHeaderComponent,
    CallbackComponent,
    TrainingOverviewComponent,
    UserDropdownComponent,
    RegistrationPageComponent,
    GroupInfoComponent,
    GroupSelectComponent,
    CreateExerciseModuleComponent,
    CalenderOverviewComponent,
  ],
  imports: [
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'group', component: GroupSelectComponent, canActivate: [AuthenticationGuard, TrainerGuard] },
      { path: 'mymodules', component: ModuleplannerPageComponent, canActivate: [AuthenticationGuard, TrainerGuard] },
      { path: 'trainingplanner', component: TrainingPageComponent, canActivate: [AuthenticationGuard, TrainerGuard] },
      { path: 'trainingoverview', component: TrainingOverviewComponent, canActivate: [AuthenticationGuard] },
      { path: 'calender', component: CalenderOverviewComponent, canActivate: [AuthenticationGuard] },
      { path: 'callback', component: CallbackComponent, canActivate: [] },
      { path: 'account', component: AccountPageComponent, canActivate: [AuthenticationGuard] },
    ]),
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    DragDropModule,
    NgbModule,
    FormsModule,
    BrowserAnimationsModule,
    CalendarModule.forRoot({ provide: DateAdapter, useFactory: adapterFactory })
  ],
  providers: [
    AuthenticationGuard,
    TrainerGuard,
    {
      provide: APP_INITIALIZER,
      useFactory: appInitializerFn,
      multi: true,
      deps: [ConfigurationService]
    },
    {
      provide: TrainingsModuleClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizationService
      ) => {
        return new TrainingsModuleClient(authorizationService, http, config.apiAddress);
      },
      deps: [HttpClient, ConfigurationService, AuthorizationService]
    },
    {
      provide: TrainingsAppointmentClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizationService
      ) => {
        return new TrainingsAppointmentClient(authorizationService, http, config.apiAddress);
      },
      deps: [HttpClient, ConfigurationService, AuthorizationService]
    },
    {
      provide: TrainingsExerciseClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizationService
      ) => {
        return new TrainingsExerciseClient(authorizationService, http, config.apiAddress);
      },
      deps: [HttpClient, ConfigurationService, AuthorizationService]
    },
    {
      provide: TrainingsModuleTagClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizationService
      ) => {
        return new TrainingsModuleTagClient(authorizationService, http, config.apiAddress);
      },
      deps: [HttpClient, ConfigurationService, AuthorizationService]
    },
    {
      provide: TrainingsGroupClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizationService
      ) => {
        return new TrainingsGroupClient(authorizationService, http, config.apiAddress);
      },
      deps: [HttpClient, ConfigurationService, AuthorizationService]
    },
    {
      provide: TrainingsGroupUserClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizationService
      ) => {
        return new TrainingsGroupUserClient(authorizationService, http, config.apiAddress);
      },
      deps: [HttpClient, ConfigurationService, AuthorizationService]
    },
    {
      provide: RegistrationClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizationService
      ) => {
        return new RegistrationClient(authorizationService, http, config.identityServerAddress);
      },
      deps: [HttpClient, ConfigurationService, AuthorizationService]
    },
    {
      provide: UserClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizationService
      ) => {
        return new UserClient(authorizationService, http, config.identityServerAddress);
      },
      deps: [HttpClient, ConfigurationService, AuthorizationService]
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }


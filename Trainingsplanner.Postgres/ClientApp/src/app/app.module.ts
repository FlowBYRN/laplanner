import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
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
import { FollowClient, TrainingsAppointmentClient, TrainingsExerciseClient, TrainingsGroupClient, TrainingsGroupUserClient, TrainingsModuleClient, TrainingsModuleTagClient, UserClient } from '../clients/api.generated.clients';
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
import { CalenderOverviewComponent } from './Kalendar/calender-overview/calender-overview.component';
import { AdminGuard } from '../api-authorization/admin.guard';
import { CalenderHeaderComponent } from './Kalendar/calender-header/calender-header.component';
import { ShedulerPageComponent } from './Kalendar/sheduler-page/sheduler-page.component';
import { RegistrationPageComponent } from './Gruppen/registration-page/registration-page.component';
import { TrainingPageComponent } from './Training/training-page/training-page.component';
import { TrainingOverviewComponent } from './Training/training-overview/training-overview.component';
import { ModuleplannerPageComponent } from './Module/moduleplanner-page/moduleplanner-page.component';
import { CreateExerciseModuleComponent } from './basic-modules/create-exercise-module/create-exercise-module.component';
import { DraggableTrainingsModuleComponent } from './basic-modules/draggable-trainings-module/draggable-trainings-module.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ModulefilterPipe } from './pipes/modulefilter.pipe';
import { TrainerGuard } from '../api-authorization/trainer.guard';
import { ModuleBrowseComponent } from './Module/module-browse/module-browse.component';

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
    RegistrationPageComponent,
    AdminComponent,
    CalenderOverviewComponent,
    ShedulerPageComponent,
    CalenderHeaderComponent,
    TrainingOverviewComponent,
    TrainingPageComponent,
    ModuleplannerPageComponent,
    CreateExerciseModuleComponent,
    DraggableTrainingsModuleComponent,
    ModulefilterPipe,
    ModuleBrowseComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    DragDropModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'groups', component: GroupSelectComponent, canActivate: [TrainerGuard] },
      { path: 'calender', component: CalenderOverviewComponent, canActivate: [AuthorizeGuard] },
      { path: 'browsemodules', component: ModuleBrowseComponent, canActivate: [AuthorizeGuard] },
      { path: 'mymodules', component: ModuleplannerPageComponent, canActivate: [TrainerGuard] },
      { path: 'trainingplanner', component: TrainingPageComponent, canActivate: [TrainerGuard] },
      { path: 'trainingoverview', component: TrainingOverviewComponent, canActivate: [AuthorizeGuard] },
      { path: 'admin', component: AdminComponent, canActivate: [AdminGuard] }
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
      provide: TrainingsModuleClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizeService
      ) => {
        return new TrainingsModuleClient(authorizationService, http, config.apiAddress);
      },
      deps: [HttpClient, ConfigurationService, AuthorizeService]
    },
    {
      provide: TrainingsAppointmentClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizeService
      ) => {
        return new TrainingsAppointmentClient(authorizationService, http, config.apiAddress);
      },
      deps: [HttpClient, ConfigurationService, AuthorizeService]
    },
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
      provide: TrainingsModuleTagClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizeService
      ) => {
        return new TrainingsModuleTagClient(authorizationService, http, config.apiAddress);
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
        return new TrainingsGroupClient(authorizationService, http, config.apiAddress);
      },
      deps: [HttpClient, ConfigurationService, AuthorizeService]
    },
    {
      provide: TrainingsGroupUserClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizeService
      ) => {
        return new TrainingsGroupUserClient(authorizationService, http, config.apiAddress);
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
        return new UserClient(authorizationService, http, config.apiAddress);
      },
      deps: [HttpClient, ConfigurationService, AuthorizeService]
    },
    {
      provide: FollowClient,
      useFactory: (
        http: HttpClient,
        config: ConfigurationService,
        authorizationService: AuthorizeService
      ) => {
        return new FollowClient(authorizationService, http, config.apiAddress);
      },
      deps: [HttpClient, ConfigurationService, AuthorizeService]
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

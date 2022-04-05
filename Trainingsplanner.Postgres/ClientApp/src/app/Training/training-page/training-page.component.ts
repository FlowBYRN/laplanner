import { Component, OnInit } from '@angular/core';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { ApplicationUser, FollowClient, TrainingsAppointment, TrainingsAppointmentClient, TrainingsAppointmentTrainingsModuleDto, TrainingsDifficulty, TrainingsGroup, TrainingsGroupClient, TrainingsModuleClient, TrainingsModuleDto, UserClient } from 'src/clients/api.generated.clients';
import { Router } from '@angular/router';
import { ContextService } from 'src/app/services/context.service';
import { AuthorizeService } from '../../../api-authorization/authorize.service';

@Component({
  selector: 'app-training-page',
  templateUrl: './training-page.component.html',
  styleUrls: ['./training-page.component.scss']
})
export class TrainingPageComponent implements OnInit {

  difficulty: TrainingsDifficulty = 0;
  searchText: string = '';
  myModules: TrainingsModuleDto[] = [];
  selectedModules: TrainingsModuleDto[] = [];
  training: TrainingsAppointment = new TrainingsAppointment();
  groups: TrainingsGroup[] = [];
  private currentUser: ApplicationUser;
  private modulesFetched = false;
  public showOwnModules: boolean = true;
  trainingsduration: number = 90;

  constructor(private trainingsModuleClient: TrainingsModuleClient,
    private trainingsClient: TrainingsAppointmentClient,
    private trainingsGroupClient: TrainingsGroupClient,
    private userClient: UserClient,
    private followClient: FollowClient,
    private contextService: ContextService,
    private authorizationService: AuthorizeService,
    private router: Router) { }


  async ngOnInit() {
    this.groups = await this.trainingsGroupClient.getAllGroups().toPromise();
    if (this.groups.length > 0)
      this.training.trainingsGroupId = this.groups[0].id;
    if (this.contextService.editAppointment) {
      this.contextService.editAppointment = false;
      this.authorizationService.getUser().subscribe(async u => {
        this.currentUser = await this.userClient.getUserByEmail(u.email).toPromise();
        if (!this.modulesFetched) {
          this.modulesFetched = true;
          let appointmentId = this.contextService.getAppointmentId();
          this.training = await this.trainingsClient.getAppointmentById(appointmentId).toPromise();
          //set local time
          this.training.startTime.setHours(this.training.startTime.getHours() - this.training.startTime.getTimezoneOffset() / 60);
          this.training.endTime.setHours(this.training.endTime.getHours() - this.training.endTime.getTimezoneOffset() / 60);
          //calculate duration
          this.trainingsduration = (this.training.endTime.getTime() - this.training.startTime.getTime())/60000;

          this.selectedModules = await this.trainingsModuleClient.getTrainingsModulesByAppointmentId(appointmentId).toPromise();
          this.myModules = await (this.trainingsModuleClient.getTrainingsModulesByUserId(this.currentUser.id).toPromise());
          this.myModules = this.myModules.filter(m => !this.selectedModules.map(s => s.id).includes(m.id));
        }
      });
    }
    else {
      this.authorizationService.getUser().subscribe(async u => {
        this.currentUser = await this.userClient.getUserByEmail(u.email).toPromise();
        if (!this.modulesFetched) {
          this.modulesFetched = true;
          this.myModules = await (this.trainingsModuleClient.getTrainingsModulesByUserId(this.currentUser.id).toPromise())
        }
      });
    }
  }



  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex);
    }
  }

  getTrainingsDifficulty(focus: TrainingsDifficulty): string {
    return TrainingsDifficulty[focus];
  }

  getGroupNameById(id: number) {
    return this.groups.find(g => g.id == id).title;
  }

  convertTime() {
    this.training.startTime = new Date(this.training.startTime);
    this.training.endTime = new Date(this.training.startTime);
    this.training.endTime.setMinutes(this.training.endTime.getMinutes() + this.trainingsduration);
  }
  async saveTraining() {
    this.convertTime();
    if (this.training.id > 0) {
      this.training = await this.trainingsClient.updateAppointment(this.training).toPromise();
    }
    else {
      this.training = await this.trainingsClient.createAppointment(this.training).toPromise();
      await this.userClient.allowEditAppointment(this.training.id, this.training.trainingsGroupId).toPromise();
      await this.authorizationService.signIn("");
    }

    let tatms: TrainingsAppointmentTrainingsModuleDto[] = [];
    let order = 1
    this.selectedModules.forEach(m => {
      let tatm = new TrainingsAppointmentTrainingsModuleDto({
        orderId: order,
        trainingsModuleId: m.id,
        trainingsAppointmentId: this.training.id
      });
      order = order + 1;
      tatms.push(tatm);
    })
    console.log(tatms);
    await this.trainingsClient.addModuleToAppointment(tatms).toPromise();

    this.contextService.setAppointmentId(this.training.id, this.training.startTime);
    this.contextService.setGroupId(this.training.trainingsGroupId);
    this.router.navigateByUrl('calender');

  }
  async valueChange($event) {
    console.log($event.target.value);
    if ($event.target.value === 'true') {
      this.myModules = await this.trainingsModuleClient.getTrainingsModulesByUserId(this.currentUser.id).toPromise();
      console.log("my", this.myModules);
    } else {
      this.myModules = await this.followClient.getFollows(this.currentUser.id).toPromise();
      console.log("followed", this.myModules);
    }
  }
}

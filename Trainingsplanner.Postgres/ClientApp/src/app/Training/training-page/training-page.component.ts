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
  private showOwnModules:boolean = true;

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

    this.authorizationService.getUser().subscribe(async u => {
      this.currentUser = await this.userClient.getUserByEmail(u.email).toPromise();
      if (!this.modulesFetched) {
        this.modulesFetched = true;
        this.myModules = await (this.trainingsModuleClient.getTrainingsModulesByUserId(this.currentUser.id).toPromise())
      }
    })
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
    this.training.endTime = new Date(this.training.endTime);
  }

  async saveTraining() {
    this.convertTime();
    console.log(this.training);
    this.training = await this.trainingsClient.createAppointment(this.training).toPromise();
    await this.userClient.allowEditAppointment(this.training.id, this.currentUser.id).toPromise();
    await this.authorizationService.signIn("");

    await this.trainingsClient.addModuleToAppointment(this.training.id, this.selectedModules.map(sm => sm.id)).toPromise();

    this.contextService.setAppointmentId(this.training.id);
    this.contextService.setGroupId(this.training.trainingsGroupId);
    this.router.navigateByUrl('calender');

  }
  async valueChange($event) {
    console.log($event.target.value, this.showOwnModules);
    if (this.showOwnModules === true) {
      this.myModules = await this.trainingsModuleClient.getTrainingsModulesByUserId(this.currentUser.id).toPromise();
      console.log("my", this.myModules);
    } else {
      this.myModules = await this.followClient.getFollows(this.currentUser.id).toPromise();
      console.log("followed",this.myModules);
    }
  }
}

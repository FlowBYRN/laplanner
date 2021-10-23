import { Component, OnInit } from '@angular/core';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { TrainingsAppointment, TrainingsAppointmentClient, TrainingsAppointmentTrainingsModuleDto, TrainingsDifficulty, TrainingsGroup, TrainingsGroupClient, TrainingsModuleClient, TrainingsModuleDto } from 'src/clients/api.generated.clients';
import { Router } from '@angular/router';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { UserClient } from 'src/clients/is4.generated.clients';
import { ContextService } from 'src/app/services/context.service';

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

  constructor(private trainingsModuleClient: TrainingsModuleClient,
    private trainingsClient: TrainingsAppointmentClient,
    private trainingsGroupClient: TrainingsGroupClient,
    private userClient: UserClient,
    private contextService: ContextService,
    private authorizationService: AuthorizationService,
    private router: Router) { }

  async ngOnInit() {
    this.groups = await this.trainingsGroupClient.getAllGroups().toPromise();
    if (this.groups.length > 0)
      this.training.trainingsGroupId = this.groups[0].id;

    let user = await (this.userClient.getUserByName(await this.authorizationService.getCurrentUserName()).toPromise());
    this.myModules = await (this.trainingsModuleClient.getTrainingsModulesByUserId(user.id).toPromise())
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

    this.training = await (this.trainingsClient.createAppointment(this.training).toPromise());
    console.log(this.selectedModules)
    await this.trainingsClient.addModuleToAppointment(this.training.id, this.selectedModules).toPromise();

    this.contextService.setAppointmentId(this.training.id);
    this.contextService.setGroupId(this.training.trainingsGroupId);
    this.router.navigateByUrl('calender');

  }

}

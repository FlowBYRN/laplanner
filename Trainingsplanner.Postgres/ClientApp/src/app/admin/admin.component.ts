import { Component, OnInit } from '@angular/core';
import { ApplicationUser, TrainingsGroupDto, TrainingsGroupClient, UserClient, RegisterViewModel, TrainingsGroupApplicationUser, TrainingsGroupApplicationUserDto, TrainingsGroupUserClient } from '../../clients/api.generated.clients';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  public newTrainer: RegisterViewModel = new RegisterViewModel();
  public newGroup: TrainingsGroupDto = new TrainingsGroupDto();
  public groups: TrainingsGroupDto[] = [];
  public trainers: ApplicationUser[] = [];

  //Add Trainer to Group
  public email: string;
  public groupId: number;
  constructor(public userClient: UserClient, private trainingsGroupClient: TrainingsGroupClient,private trainingsGroupUserClient: TrainingsGroupUserClient  ) { }

  async ngOnInit() {
    
    this.groups = await this.trainingsGroupClient.getAllGroups().toPromise();
    this.trainers = await this.userClient.getTrainers().toPromise();
  }

  public async createNewGroup() {
    this.groups.push(this.newGroup);
    await this.trainingsGroupClient.createGroup(this.newGroup).toPromise();
  }

  async createTrainer() {
    await this.userClient.registerTrainer(this.newTrainer).toPromise();
  }

  async deleteGroup(id: number) {
    await this.trainingsGroupClient.deleteGroup(this.groups.find(g => g.id == id)).toPromise();
    this.groups = this.groups.filter(g => g.id != id);
  }

  async deleteTrainer(id: string) {
    await this.userClient.deleteUser(id).toPromise();
    this.trainers = this.trainers.filter(g => g.id != id);
  }

  async addTrainerToGroup() {
    if (this.groupId != 0 && this.email.length > 0) {
      let userId: string = this.trainers.find(t => t.email == this.email).id;
      let newTrainerGroup: TrainingsGroupApplicationUserDto = new TrainingsGroupApplicationUserDto({
        applicationUserId: userId,
        trainingsGroupId: this.groupId,
        isTrainer: true
      });
      await this.trainingsGroupUserClient.addTrainerToGroup(newTrainerGroup).toPromise();
      await this.userClient.allowEditGroup(newTrainerGroup.trainingsGroupId, newTrainerGroup.applicationUserId).toPromise();
      await this.userClient.allowReadGroup(newTrainerGroup.trainingsGroupId, newTrainerGroup.applicationUserId).toPromise();
    }
  }

  async rmTrainerToGroup() {
    let userId: string = this.trainers.find(t => t.email == this.email).id;
    await this.trainingsGroupUserClient.deleteMemberFromGroup(this.groupId, userId).toPromise();
    await this.userClient.disallowEditGroup(this.groupId, userId).toPromise();
    await this.userClient.disallowReadGroup(this.groupId, userId).toPromise();
  }
}

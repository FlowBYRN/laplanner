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
    console.log(this.newGroup, "grupppe erstellen");
    this.groups.push(this.newGroup);
    await this.trainingsGroupClient.createGroup(this.newGroup).toPromise();
  }

  async createTrainer() {
    console.log(this.newTrainer)
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
      let newTrainerGroup: TrainingsGroupApplicationUserDto = new TrainingsGroupApplicationUserDto({
        applicationUserId: this.trainers.find(t => t.email == this.email).id,
        trainingsGroupId: this.groupId,
        isTrainer: true
      });
      console.log("addTrainerToGroup", newTrainerGroup);
      await this.trainingsGroupUserClient.addTrainerToGroup(newTrainerGroup).toPromise();
    }
  }

  async rmTrainerToGroup() {
    await this.trainingsGroupUserClient.deleteMemberFromGroup(this.groupId, this.trainers.find(t => t.email == this.email).id).toPromise();
  }
}

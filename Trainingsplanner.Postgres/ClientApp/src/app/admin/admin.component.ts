import { Component, OnInit } from '@angular/core';
import { ApplicationUser, TrainingsGroupDto, TrainingsGroupClient, UserClient } from '../../clients/api.generated.clients';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  public newTrainer: ApplicationUser = new ApplicationUser();
  public newGroup: TrainingsGroupDto = new TrainingsGroupDto();
  public groups: TrainingsGroupDto[] = [];

  constructor(public userClient: UserClient, private trainingsGroupClient: TrainingsGroupClient) { }

  async ngOnInit() {
    
    //this.groups = await this.trainingsGroupClient.getAllGroups().toPromise();
  }

  public async createNewGroup() {
    console.log(this.newGroup, "grupppe erstellen");
    await this.trainingsGroupClient.createGroup(this.newGroup).toPromise();
  }

  createTrainer() {

  }
}

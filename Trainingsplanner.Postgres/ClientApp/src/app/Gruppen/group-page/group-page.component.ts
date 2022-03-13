import { Component, EventEmitter, Input, OnInit, Output, TemplateRef } from '@angular/core';
import { ApplicationUser, TrainingsGroupApplicationUserDto, TrainingsGroupClient, TrainingsGroupDto, TrainingsGroupUserClient, UserClient } from 'src/clients/api.generated.clients';

@Component({
  selector: 'app-group-page',
  templateUrl: './group-page.component.html',
  styleUrls: ['./group-page.component.scss']
})
export class GroupPageComponent implements OnInit {

  currentGroup: number = 1;
  @Input() group: TrainingsGroupDto;
  @Output() exitGroup = new EventEmitter();
  users: { user: ApplicationUser, isTrainer: boolean }[] = [];
  //newRegistrationUser: RegisterViewModel = new RegisterViewModel();
  showRegisterFormular: boolean = false;


  searchedUserName: string;
  searchedUserisTrainer: boolean = false;
  public AddUserisCollapsed = true;
  public RegisterisCollapsed = true;

  constructor(private trainingsGroupClient: TrainingsGroupClient,
    private userClient: UserClient,
    private trainingsGroupUserClient: TrainingsGroupUserClient) { }

  async ngOnInit() {


    this.getUserInformations();

  }

  async getUserInformations() {
    if (!this.group.trainingsGroupsApplicationUsers)
      this.group.trainingsGroupsApplicationUsers = [];
    this.group.trainingsGroupsApplicationUsers.forEach(async element => {
      const user = await this.userClient.getUserById(element.applicationUserId).toPromise();
      this.users.push({ user: user, isTrainer: element.isTrainer });
    });
  }

  async searchForUser() {
    const checked = this.users.find(u => u.user.userName.toLocaleLowerCase() == this.searchedUserName.toLocaleLowerCase());
    if (checked)
      return;

    let user = await this.userClient.getUserByName(this.searchedUserName).toPromise();
    if (user) {
    //  this.userClient.allowCreatContent(user.id);
      this.userFound(user, this.searchedUserisTrainer);
    }
  }
  async userFound(user: ApplicationUser, isTrainer: boolean) {
    const groupuser: TrainingsGroupApplicationUserDto = new TrainingsGroupApplicationUserDto({ trainingsGroupId: this.currentGroup, applicationUserId: user.id, isTrainer: isTrainer });
    console.log(groupuser);
    if (isTrainer)
      this.accessTrainerToGroup(groupuser);
    else
      this.accessAthleteToGroup(groupuser);

    this.users.push({ user: user, isTrainer: this.searchedUserisTrainer });
    this.AddUserisCollapsed = true;
    this.RegisterisCollapsed = true;
  }
  back() {
    this.exitGroup.emit();
  }

  async accessTrainerToGroup(groupuser: TrainingsGroupApplicationUserDto) {
    await this.trainingsGroupUserClient.addTrainerToGroup(groupuser).toPromise();
    await this.userClient.allowEditGroup(groupuser.trainingsGroupId, groupuser.applicationUserId).toPromise();
  }

  async accessAthleteToGroup(groupuser: TrainingsGroupApplicationUserDto) {
    await this.trainingsGroupUserClient.addUserToGroup(groupuser).toPromise();
    await this.userClient.allowReadGroup(groupuser.trainingsGroupId, groupuser.applicationUserId).toPromise();
  }

  async delete(member: { user: ApplicationUser, isTrainer: boolean }) {
    await this.trainingsGroupUserClient.deleteMemberFromGroup(this.currentGroup, member.user.id).toPromise()
    if (member.isTrainer)
      await this.userClient.disallowEditGroup(this.currentGroup, member.user.id).toPromise();
    else
      await this.userClient.disallowReadGroup(this.currentGroup, member.user.id).toPromise();
    this.users = this.users.filter(u => u.user.id != member.user.id);
  }
  getRole(isTrainer: boolean): string {
    return isTrainer ? "Trainer" : "Athlete";
  }
}

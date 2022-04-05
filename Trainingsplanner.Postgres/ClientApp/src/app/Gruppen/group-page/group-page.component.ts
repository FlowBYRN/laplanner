import { Component, EventEmitter, Input, OnInit, Output, TemplateRef } from '@angular/core';
import { ApplicationUser, RegisterViewModel, TrainingsAppointmentClient, TrainingsDayDto, TrainingsGroupApplicationUserDto, TrainingsGroupClient, TrainingsGroupDto, TrainingsGroupUserClient, UserClient, WeekDto } from 'src/clients/api.generated.clients';

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
  newRegistrationUser: RegisterViewModel = new RegisterViewModel();
  showRegisterFormular: boolean = false;
  week: WeekDto = new WeekDto();

  searchedUserName: string;
  searchedUserisTrainer: boolean = false;
  public AddUserisCollapsed = true;
  public RegisterisCollapsed = true;

  constructor(private trainingsGroupClient: TrainingsGroupClient,
    private userClient: UserClient,
    private trainingsGroupUserClient: TrainingsGroupUserClient,
    private trainingsAppointmetnClient: TrainingsAppointmentClient) { }

  async ngOnInit() {
    this.week.monday = new TrainingsDayDto();
    this.week.tuesday = new TrainingsDayDto();
    this.week.wednesday = new TrainingsDayDto();
    this.week.thursday = new TrainingsDayDto();
    this.week.friday = new TrainingsDayDto();
    this.week.saturday = new TrainingsDayDto();
    this.week.sunday = new TrainingsDayDto();

    this.currentGroup = this.group.id;
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
    console.log(groupuser)
    await this.trainingsGroupUserClient.addUserToGroup(groupuser).toPromise();
    await this.userClient.allowReadGroup(groupuser.trainingsGroupId, groupuser.applicationUserId).toPromise();
  }

  async delete(user: ApplicationUser) {
    await this.trainingsGroupUserClient.deleteMemberFromGroup(this.currentGroup, user.id).toPromise()
    await this.userClient.disallowReadGroup(this.currentGroup, user.id).toPromise();
    this.users = this.users.filter(u => u.user.id != user.id);
  }
  getRole(isTrainer: boolean): string {
    return isTrainer ? "Trainer" : "Athlete";
  }

  saveAppointments() {
    console.log(this.week);
    this.trainingsAppointmetnClient.sheduleTrainingsWeek(this.currentGroup,this.week).toPromise();
  }
}

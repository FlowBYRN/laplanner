import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ApplicationUser, RegisterViewModel, TrainingsGroupApplicationUserDto, TrainingsGroupUserClient, UserClient } from 'src/clients/api.generated.clients';

@Component({
  selector: 'app-registration-page',
  templateUrl: './registration-page.component.html',
  styleUrls: ['./registration-page.component.css']
})
export class RegistrationPageComponent implements OnInit {

  @Input() user: RegisterViewModel;

  @Output() userFound = new EventEmitter<ApplicationUser>();

  constructor(private userClient: UserClient,
    private trainingsGroupUserClient: TrainingsGroupUserClient) { }

  ngOnInit(): void {
  }
  async processRegister() {
    let retUser: ApplicationUser;
    if (this.user.isTrainer) {
      let retUser = await this.userClient.registerTrainer(this.user).toPromise(); //todo trainer auswählen
    }
    else {
      let retUser = await this.userClient.registerAthlete(this.user).toPromise(); //todo trainer auswählen
    }

    const groupuser: TrainingsGroupApplicationUserDto = new TrainingsGroupApplicationUserDto({ trainingsGroupId: 1, applicationUserId: retUser.id });
    this.userFound.emit(retUser);
  }

  getRole(): string {
    return this.user.isTrainer ? "Trainer" : "Athlet";
  }
}

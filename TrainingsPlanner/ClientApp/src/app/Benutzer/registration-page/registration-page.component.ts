import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TrainingsGroupApplicationUserDto, TrainingsGroupUserClient } from 'src/clients/api.generated.clients';
import { ApplicationUser, RegisterViewModel, RegistrationClient } from 'src/clients/is4.generated.clients';

@Component({
  selector: 'app-registration-page',
  templateUrl: './registration-page.component.html',
  styleUrls: ['./registration-page.component.css']
})
export class RegistrationPageComponent implements OnInit {

  @Input() user: RegisterViewModel;

  @Output() userFound = new EventEmitter<ApplicationUser>();

  constructor(private registerClient: RegistrationClient,
    private trainingsGroupUserClient: TrainingsGroupUserClient) { }

  ngOnInit(): void {
  }
  async processRegister() {
    let retUser: ApplicationUser;
    if (this.user.isTrainer) {
      let retUser = await this.registerClient.registerTrainer(this.user).toPromise(); //todo trainer auswählen
    }
    else {
      let retUser = await this.registerClient.registerAthlete(this.user).toPromise(); //todo trainer auswählen
    }

    const groupuser: TrainingsGroupApplicationUserDto = new TrainingsGroupApplicationUserDto({ trainingsGroupId: 1, applicationUserId: retUser.id });
    this.userFound.emit(retUser);
  }

  getRole(): string {
    return this.user.isTrainer ? "Trainer" : "Athlet";
  }
}

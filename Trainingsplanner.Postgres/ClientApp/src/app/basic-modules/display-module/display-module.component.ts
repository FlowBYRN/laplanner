import { Component, Input, OnInit } from '@angular/core';
import { AuthorizeService } from '../../../api-authorization/authorize.service';
import { ApplicationUser, FollowClient, TrainingsExerciseClient, TrainingsExerciseDto, TrainingsModuleDto, TrainingsModuleFollowDto, UserClient } from '../../../clients/api.generated.clients';

@Component({
  selector: 'app-display-module',
  templateUrl: './display-module.component.html',
  styleUrls: ['./display-module.component.css']
})
export class DisplayModuleComponent implements OnInit {

  @Input() module: TrainingsModuleDto;
  exercises: TrainingsExerciseDto[] = [];
  currentUser: ApplicationUser;
  public isAuthenticated: Observable<boolean>;

  constructor(private exerciseClient: TrainingsExerciseClient, private followClient: FollowClient, private userClient: UserClient, private authorizationService: AuthorizeService) { }

  async ngOnInit() {
    this.authorizationService.getUser().subscribe(async u => {
      this.currentUser = await this.userClient.getUserByEmail(u.email).toPromise();
    });
    this.isAuthenticated = this.authorizationService.isAuthenticated();
  }

  async follow() {
    await this.followClient.followModule(new TrainingsModuleFollowDto({ trainingsModuleId: this.module.id, userId: this.currentUser.id })).toPromise();
  }
}

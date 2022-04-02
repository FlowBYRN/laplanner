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
  @Input() followEnabled: boolean;

  exercises: TrainingsExerciseDto[] = [];
  currentUser: ApplicationUser;
  moduleDuration: number;
  constructor(private exerciseClient: TrainingsExerciseClient, private followClient: FollowClient, private userClient: UserClient, private authorizationService: AuthorizeService) { }

  async ngOnInit() {
    this.authorizationService.getUser().subscribe(async u => {
      this.currentUser = await this.userClient.getUserByEmail(u.email).toPromise();
    });
    if (this.module.trainingsModulesTrainingsExercises)
      this.moduleDuration = this.module.trainingsModulesTrainingsExercises.map(tmte => tmte.trainingsExercise.duration).reduce((sum: number, d: number) => sum + d, 0);
  }

  async follow() {
    await this.followClient.followModule(this.module.id).toPromise();
    this.module.isFollowed = true;
  }

  async unfollow() {
    await this.followClient.unFollowModule(this.module.id).toPromise();
    this.module.isFollowed = false;
  }
}

import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../../../api-authorization/authorize.service';
import { ApplicationUser, FollowClient, TrainingsExerciseClient, TrainingsModule, TrainingsModuleClient, TrainingsModuleFollowDto, TrainingsModuleTrainingsExercise, UserClient } from '../../../clients/api.generated.clients';

@Component({
  selector: 'app-module-browse',
  templateUrl: './module-browse.component.html',
  styleUrls: ['./module-browse.component.css']
})
export class ModuleBrowseComponent implements OnInit {

  trainingsModules: TrainingsModule[] = [];
  selectedModule: TrainingsModule = new TrainingsModule();
  searchText: string = '';
  currentUser: ApplicationUser;

  constructor(private moduleClient: TrainingsModuleClient, private followClient: FollowClient, private userClient: UserClient, private exerciseClient: TrainingsExerciseClient, private authorizationService: AuthorizeService) { }

  async ngOnInit() {
    this.authorizationService.getUser().subscribe(async u => {
      this.currentUser = await this.userClient.getUserByEmail(u.email).toPromise();
    });

    this.trainingsModules = await this.moduleClient.getAllPublicTrainingsModules().toPromise();
    if (this.trainingsModules.length > 0)
      this.selectedModule = this.trainingsModules[0];
  }

  async selectCurrentModule(module: TrainingsModule) {
    let exercises = await this.exerciseClient.readExercisesByModuleId(module.id).toPromise();
    if (!module.trainingsModulesTrainingsExercises)
      module.trainingsModulesTrainingsExercises = [];
    exercises.forEach(e => {
      module.trainingsModulesTrainingsExercises.push(new TrainingsModuleTrainingsExercise({ trainingsExercise: e, trainingsModuleId:module.id }))
    })
    this.selectedModule = module;

    console.log(this.selectedModule)
  }


}

import { Component, OnInit } from '@angular/core';
import { TrainingsExerciseClient, TrainingsModule, TrainingsModuleClient, TrainingsModuleTrainingsExercise, UserClient } from '../../../clients/api.generated.clients';

@Component({
  selector: 'app-module-browse',
  templateUrl: './module-browse.component.html',
  styleUrls: ['./module-browse.component.css']
})
export class ModuleBrowseComponent implements OnInit {

  trainingsModules: TrainingsModule[] = [];
  selectedModule: TrainingsModule = new TrainingsModule();
  searchText: string = '';

  constructor(private moduleClient: TrainingsModuleClient, private userClient: UserClient, private exerciseClient: TrainingsExerciseClient) { }

  async ngOnInit() {
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

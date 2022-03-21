import { Component, OnInit } from '@angular/core';
import { TrainingsModule, TrainingsModuleClient, UserClient } from '../../../clients/api.generated.clients';

@Component({
  selector: 'app-module-browse',
  templateUrl: './module-browse.component.html',
  styleUrls: ['./module-browse.component.css']
})
export class ModuleBrowseComponent implements OnInit {

  trainingsModules: TrainingsModule[] = [];
  selectedModule: TrainingsModule = new TrainingsModule();
  searchText: string = '';

  constructor(private moduleClient: TrainingsModuleClient, private userClient: UserClient) { }

  async ngOnInit() {
    this.trainingsModules = await this.moduleClient.getAllPublicTrainingsModules().toPromise();
    //this.trainingsModules.forEach(tm => {
    //  this.userClient.getUserById(tm.userId);
    //})
  }

  selectCurrentModule(module: TrainingsModule) {
    this.selectedModule = module;
    console.log(this.selectedModule)
  }

}

import { Component, OnInit } from '@angular/core';
import { filter } from 'rxjs/operators';
import { TrainingsAppointmentClient, TrainingsAppointmentDto, TrainingsExerciseClient, TrainingsModuleTrainingsExerciseDto, TrainingsExerciseDto, TrainingsModuleClient, TrainingsModuleDto, TrainingsModuleTagDto, TrainingsModuleTagClient, TrainingsModuleTrainingsModuleTag, UserClient, ApplicationUser } from 'src/clients/api.generated.clients';
import { AuthorizeService, IUser } from '../../../api-authorization/authorize.service';

@Component({
  selector: 'app-moduleplanner-page',
  templateUrl: './moduleplanner-page.component.html',
  styleUrls: ['./moduleplanner-page.component.scss']
})
export class ModuleplannerPageComponent implements OnInit {

  trainingsModules: TrainingsModuleDto[] = [];
  searchText: string = '';

  currentModule: TrainingsModuleDto = new TrainingsModuleDto();
  currentModuleEdited: boolean = false;
  currentUser: ApplicationUser;
  modulesFetched: boolean = false;
  constructor(private trainingsModuleClient: TrainingsModuleClient,
    private authService: AuthorizeService,
    private userClient: UserClient,
    private trainingsExerciseClient: TrainingsExerciseClient,
    private trainingsModuleTagClient: TrainingsModuleTagClient) {
  }

  async ngOnInit() {
    await this.authService.getUser().subscribe(async u => {
      this.currentUser = await this.userClient.getUserByEmail(u.email).toPromise();
      if (!this.modulesFetched) {
        this.modulesFetched = true;
        this.trainingsModules = await this.trainingsModuleClient.getTrainingsModulesByUserId(this.currentUser.id).toPromise();
        this.currentModule = this.trainingsModules[0];
        if (!this.currentModule) this.addModule();
        console.log("fetched", this.currentModule);
      }
    });
  }

  async addExercise() {
    const exercise = new TrainingsExerciseDto({ title: "Neue Übung", description: "Beschreibung hinzufügen" });
    const ret = await this.trainingsExerciseClient.createExercise(exercise).toPromise();
    await this.trainingsModuleClient.addExerciseToModule(this.currentModule.id, ret.id).toPromise();
    this.currentModule.trainingsModulesTrainingsExercises.push(new TrainingsModuleTrainingsExerciseDto({ trainingsModuleId: this.currentModule.id, trainingsExerciesId: ret.id, trainingsExercise: ret }))
  }

  async addTag() {
    const tag = new TrainingsModuleTagDto({ title: "new tag" });
    const ret = await this.trainingsModuleTagClient.createTag(tag).toPromise();
    this.currentModule.trainingsModulesTrainingsModuleTags.push(new TrainingsModuleTrainingsModuleTag({ trainingsModuleId: this.currentModule.id, trainingsModuleTag: ret }))
  }

  async deleteTag(tag: TrainingsModuleTagDto) {
    await this.trainingsModuleClient.deleteTagByModuleId(this.currentModule.id, tag.id).toPromise();
    this.currentModule.trainingsModulesTrainingsModuleTags = this.currentModule.trainingsModulesTrainingsModuleTags.filter(tmte => tmte.trainingsModuleTag.id != tag.id);
  }

  selectCurrentModule(trainingsModule: TrainingsModuleDto) {
    this.currentModuleEdited = false;
    this.currentModule = trainingsModule;
    if (!this.currentModule.trainingsModulesTrainingsExercises) {
      this.currentModule.trainingsModulesTrainingsExercises = []
    }
  }

  async addModule() {
    if (this.currentModule != null && this.currentModule.title == "neues Modul") { alert("Bitte ändere zuerst den Namen des aktuellen Moduls!"); }
    else {
      this.currentModuleEdited = true;
      const user = this.currentUser;
      this.currentModule = new TrainingsModuleDto();
      this.currentModule.trainingsModulesTrainingsExercises = []
      this.currentModule.userId = user.id;
      this.currentModule.title = "neues Modul";
      console.log(this.currentModule);
      this.currentModule = await this.trainingsModuleClient.createTrainingsModule(this.currentModule).toPromise();
      await this.userClient.allowEditModule(this.currentModule.id, this.currentUser.id).toPromise();
      console.log(this.currentModule);
      this.trainingsModules.push(this.currentModule);
    }
  }

  async saveModule() {
    if (this.currentModule.title == "neues Modul") alert("Bitte ändere zuerst den Modulnamen!");
    else {
      this.currentModuleEdited = false;
      //save module
      console.log("current", this.currentModule);
      const ret = await this.trainingsModuleClient.updateTrainingsModule(this.currentModule).toPromise();
      await this.userClient.allowEditModule(ret.id, this.currentUser.id).toPromise();
      console.log(ret);

      //save exercises
      console.log("exercises to update", this.currentModule.trainingsModulesTrainingsExercises);
      this.currentModule.trainingsModulesTrainingsExercises.forEach(async tmte => {
        const ret = await this.trainingsExerciseClient.updateExercise(tmte.trainingsExercise).toPromise();
        console.log("updated", ret);
      });
    }
  }

  async deleteTrainingsModule(module: TrainingsModuleDto) {
    if (module.id != 0) {

      module.trainingsModulesTrainingsExercises.forEach(async tmte => {
        await this.trainingsModuleClient.deleteExerciseByModuleId(module.id, tmte.trainingsExerciesId).toPromise(); //n:m table
      });
      module.trainingsModulesTrainingsModuleTags.forEach(async tmtt => {
        await this.trainingsModuleClient.deleteTagByModuleId(module.id, tmtt.trainingsModuleTagId).toPromise(); //n:m table

      });

      console.log("Vor setzen von Leeren Arrays");
      module.trainingsModulesTrainingsModuleTags = [];
      module.trainingsModulesTrainingsExercises = [];
      console.log("Nach setzen von Leeren Arrays");

      const temp = await this.trainingsModuleClient.deleteTrainingsModule(module).toPromise();
      console.log(temp);
      await this.userClient.disallowEditModule(module.id, this.currentUser.id).toPromise();
      this.trainingsModules = this.trainingsModules.filter(tm => tm.id != module.id);
    } else {
      this.trainingsModules = this.trainingsModules.filter(tm => tm.title != module.title && tm.description != module.description);
    }
    this.currentModule = this.trainingsModules[0];
    this.currentModuleEdited = false;
  }

  async deleteExercise(exercise: TrainingsExerciseDto) {
    await this.trainingsModuleClient.deleteExerciseByModuleId(this.currentModule.id, exercise.id).toPromise(); //n:m table
    const ret = await this.trainingsExerciseClient.deleteExercise(exercise).toPromise();
    console.log("delete", ret);
    this.currentModule.trainingsModulesTrainingsExercises = this.currentModule.trainingsModulesTrainingsExercises.filter(tmte => tmte.trainingsExercise.id != exercise.id);
  }
}

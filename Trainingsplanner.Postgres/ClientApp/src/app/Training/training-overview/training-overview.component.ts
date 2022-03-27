import { Component, ElementRef, ModuleWithComponentFactories, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ContextService } from 'src/app/services/context.service';
import { TrainingsAppointment, TrainingsAppointmentClient } from 'src/clients/api.generated.clients';
import { AuthorizeService } from '../../../api-authorization/authorize.service';

@Component({
  selector: 'app-training-overview',
  templateUrl: './training-overview.component.html',
  styleUrls: ['./training-overview.component.css']
})
export class TrainingOverviewComponent implements OnInit {

  currentTraining: TrainingsAppointment = new TrainingsAppointment();
  hasAccess: Observable<boolean> = new Observable<boolean>();

  constructor(private trainingsAppointmentClient: TrainingsAppointmentClient, private router: Router, private contextService: ContextService, private authService: AuthorizeService) { }

  async ngOnInit() {
    const id = this.contextService.getAppointmentId();
    if (id > 0)
      this.currentTraining = await (this.trainingsAppointmentClient.getFullAppointmentById(id).toPromise())
      this.hasAccess = this.authService.hasAccess(id);
  }

  edit() {
    this.contextService.editAppointment = true;
    this.contextService.setAppointmentId(this.currentTraining.id);
    this.router.navigateByUrl("/trainingplanner");
  }

  async delete() {
    await this.trainingsAppointmentClient.deleteAppointment(this.currentTraining).toPromise();
    this.contextService.setGroupId(this.currentTraining.trainingsGroupId);
    this.router.navigateByUrl("/calender");

  }
}

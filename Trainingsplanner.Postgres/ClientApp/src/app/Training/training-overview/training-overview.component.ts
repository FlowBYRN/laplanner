import { Component, ElementRef, ModuleWithComponentFactories, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ContextService } from 'src/app/services/context.service';
import { TrainingsAppointment, TrainingsAppointmentClient } from 'src/clients/api.generated.clients';

@Component({
  selector: 'app-training-overview',
  templateUrl: './training-overview.component.html',
  styleUrls: ['./training-overview.component.css']
})
export class TrainingOverviewComponent implements OnInit {

  currentTraining: TrainingsAppointment = new TrainingsAppointment();

  constructor(private trainingsAppointmentClient: TrainingsAppointmentClient, private router: Router, private contextService: ContextService) { }

  async ngOnInit() {
    const id = this.contextService.getAppointmentId();
    if (id > 0)
      this.currentTraining = await (this.trainingsAppointmentClient.getFullAppointmentById(id).toPromise())
  }

  edit() {
    this.contextService.editAppointment = true;
    this.contextService.setAppointmentId(this.currentTraining.id);
    this.router.navigateByUrl("/trainingplanner");
  }
}

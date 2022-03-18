import { Component, Input, OnInit } from '@angular/core';
import { TrainingsDifficulty, TrainingsModuleDto } from 'src/clients/api.generated.clients';

@Component({
  selector: 'app-draggable-trainings-module',
  templateUrl: './draggable-trainings-module.component.html',
  styleUrls: ['./draggable-trainings-module.component.scss']
})
export class DraggableTrainingsModuleComponent implements OnInit {

  constructor() { }

  @Input() module: TrainingsModuleDto;
  public isCollapsed = true;


  ngOnInit(): void {
  }


  getTrainingsDifficulty(focus: number): string {
    return TrainingsDifficulty[focus];
  }
}

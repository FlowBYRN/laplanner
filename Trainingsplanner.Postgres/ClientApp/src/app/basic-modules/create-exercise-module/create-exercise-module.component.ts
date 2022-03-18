import { EventEmitter } from '@angular/core';
import { Component, Input, OnInit, Output } from '@angular/core';
import { TrainingsExerciseDto } from 'src/clients/api.generated.clients';

@Component({
  selector: 'app-create-exercise-module',
  templateUrl: './create-exercise-module.component.html',
  styleUrls: ['./create-exercise-module.component.css']
})
export class CreateExerciseModuleComponent implements OnInit {

  @Input() exercise: TrainingsExerciseDto;
  @Input() currentModuleEdited: boolean;
  @Output() deleteExercise: EventEmitter<TrainingsExerciseDto> = new EventEmitter<TrainingsExerciseDto>();

  constructor() { }

  ngOnInit(): void {
  }
}

import { Component, Input, OnInit } from '@angular/core';
import { TrainingsExerciseClient, TrainingsExerciseDto, TrainingsModuleDto } from '../../../clients/api.generated.clients';

@Component({
  selector: 'app-display-module',
  templateUrl: './display-module.component.html',
  styleUrls: ['./display-module.component.css']
})
export class DisplayModuleComponent implements OnInit {

  @Input() module: TrainingsModuleDto;
  exercises: TrainingsExerciseDto[] = [];
  constructor(private exerciseClient: TrainingsExerciseClient) { }

  async ngOnInit() {
  }
}

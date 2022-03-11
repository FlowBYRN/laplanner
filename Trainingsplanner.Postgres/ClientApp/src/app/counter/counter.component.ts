import { Component } from '@angular/core';
import { TrainingsExerciseDto, TrainingsExerciseClient } from '../../clients/api.generated.clients';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent {
  public currentCount = 0;
  public ex: TrainingsExerciseDto[] = [];

  constructor(public trainingsExerciseClient: TrainingsExerciseClient) {
  }

  async ngOnInit() {
    this.ex = await this.trainingsExerciseClient.readAllExercises().toPromise();
  }

  public incrementCounter() {
    this.currentCount++;
  }
}

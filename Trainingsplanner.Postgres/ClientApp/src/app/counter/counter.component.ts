import { Component } from '@angular/core';
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { TrainingsExerciseDto, TrainingsExerciseClient } from '../../clients/api.generated.clients';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent {
  public currentCount = 0;
  public ex: TrainingsExerciseDto[] = [];

  constructor(public trainingsExerciseClient: TrainingsExerciseClient, public authService: AuthorizeService) {
  }

  async ngOnInit() {
    this.ex = await this.trainingsExerciseClient.readAllExercises().toPromise();
    this.authService.getUser()
  }

  public incrementCounter() {
    this.currentCount++;
  }
}

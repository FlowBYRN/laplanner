import { Input } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { TrainingsDayDto } from '../../../clients/api.generated.clients';

@Component({
  selector: 'app-weekly-shedule',
  templateUrl: './weekly-shedule.component.html',
  styleUrls: ['./weekly-shedule.component.css']
})
export class WeeklySheduleComponent implements OnInit {

  @Input() day: TrainingsDayDto = new TrainingsDayDto();
  @Input() title: string;
  constructor() { }

  ngOnInit(): void {
  }

}

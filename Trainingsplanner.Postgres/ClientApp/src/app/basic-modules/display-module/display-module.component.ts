import { Component, Input, OnInit } from '@angular/core';
import { TrainingsModule } from '../../../clients/api.generated.clients';

@Component({
  selector: 'app-display-module',
  templateUrl: './display-module.component.html',
  styleUrls: ['./display-module.component.css']
})
export class DisplayModuleComponent implements OnInit {

  @Input() module: TrainingsModule;

  constructor() { }

  ngOnInit(): void {
  }

}

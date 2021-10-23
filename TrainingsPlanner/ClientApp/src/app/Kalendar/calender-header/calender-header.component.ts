import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CalendarView } from 'angular-calendar';

@Component({
  selector: 'app-calender-header',
  templateUrl: './calender-header.component.html',
  styleUrls: ['./calender-header.component.scss']
})
export class CalenderHeaderComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  @Input() view: CalendarView;

  @Input() viewDate: Date;

  @Input() locale: string = 'en';

  @Output() viewChange = new EventEmitter<CalendarView>();

  @Output() viewDateChange = new EventEmitter<Date>();

  CalendarView = CalendarView;
}

import { Component, ChangeDetectionStrategy, OnInit, AfterContentInit, Input, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { CalendarEvent, CalendarView } from 'angular-calendar';


import { map } from 'rxjs/operators';
import {
  isSameMonth,
  isSameDay,
  startOfMonth,
  endOfMonth,
  startOfWeek,
  endOfWeek,
  startOfDay,
  endOfDay,
  format,
} from 'date-fns';
import { Observable } from 'rxjs';
import { TrainingsAppointmentClient, TrainingsAppointmentDto, TrainingsGroupClient, TrainingsGroupDto } from 'src/clients/api.generated.clients';
import { ContextService } from 'src/app/services/context.service';
import { UserClient } from 'src/clients/is4.generated.clients';
import { AuthorizationService } from 'src/app/services/authorization.service';


interface Event {
  title: string;
  start: Date;
}

@Component({
  selector: 'app-sheduler-page',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './sheduler-page.component.html',
  styleUrls: ['./sheduler-page.component.scss']
})
export class ShedulerPageComponent implements OnInit {
  view: CalendarView = CalendarView.Month;

  @ViewChild('calenderview') calenderview: ElementRef;

  @Input() group: TrainingsGroupDto;
  @Output() exitGroup = new EventEmitter();

  viewDate: Date = new Date();

  events$: Observable<CalendarEvent<{ event: Event }>[]>;

  activeDayIsOpen: boolean = false;

  constructor(
    private router: Router,
    private contextService: ContextService,
    private userClient: UserClient,
    private authService: AuthorizationService,
    private trainingsGroupClient: TrainingsGroupClient,
    private trainingsAppointmentClient: TrainingsAppointmentClient
  ) { }

  async ngOnInit() {
    await this.getAllAppointments();
  }

  public async getAllAppointments() {
    const appointments = this.trainingsGroupClient.getAppointmentsByGroupId(this.group.id);
    this.events$ = appointments.pipe(
      map((results: TrainingsAppointmentDto[]) => {
        return results.map((appointment: TrainingsAppointmentDto) => {
          return {
            id: appointment.id,
            title: appointment.title,
            start: appointment.startTime,
            end: appointment.endTime,
            desc: appointment.description,
            color: colors.yellow
          };
        });
      })
    );
    console.log(this.events$);
  }

  dayClicked({
    date,
    events,
  }: {
    date: Date;
    events: CalendarEvent<{ appointment: TrainingsAppointmentDto }>[];
  }): void {
    if (isSameMonth(date, this.viewDate)) {
      if (
        (isSameDay(this.viewDate, date) && this.activeDayIsOpen === true) ||
        events.length === 0
      ) {
        this.activeDayIsOpen = false;
      } else {
        this.activeDayIsOpen = true;
        this.viewDate = date;
        console.log(date);
      }
    }
  }

  eventClicked(event: CalendarEvent): void {
    console.log('Event clicked', event);
    //TODO: Link anpassen
    this.contextService.setAppointmentId(event.id);
    this.router.navigateByUrl('trainingoverview');

  }
}

export const colors: any = {
  red: {
    primary: '#ad2121',
    secondary: '#FAE3E3',
  },
  blue: {
    primary: '#1e90ff',
    secondary: '#D1E8FF',
  },
  yellow: {
    primary: '#e3bc08',
    secondary: '#FDF1BA',
  },
};


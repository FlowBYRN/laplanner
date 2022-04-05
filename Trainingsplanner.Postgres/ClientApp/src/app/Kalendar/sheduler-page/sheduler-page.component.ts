import { Component, ChangeDetectionStrategy, OnInit, AfterContentInit, Input, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { CalendarView } from 'angular-calendar';


import { map, tap } from 'rxjs/operators';
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
import { CalenderAppointmentDto, TrainingsAppointmentClient, TrainingsAppointmentDto, TrainingsAppointmentTrainingsModuleDto, TrainingsGroupClient, TrainingsGroupDto, UserClient } from 'src/clients/api.generated.clients';
import { ContextService } from 'src/app/services/context.service';
import { AuthorizeService } from '../../../api-authorization/authorize.service';


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
  view: CalendarView = CalendarView.Week;

  @ViewChild('calenderview') calenderview: ElementRef;

  @Input() group: TrainingsGroupDto;
  @Output() exitGroup = new EventEmitter();

  viewDate: Date = new Date();

  events$: Observable<CalendarEvent<{ event: Event }>[]>;

  activeDayIsOpen: boolean = false;

  constructor(
    private router: Router,
    private contextService: ContextService,
    private trainingsAppointmentClient: TrainingsAppointmentClient
  ) { }

  async ngOnInit() {
    await this.getAllAppointments(new Date());
  }

  public async getAllAppointments(viewDate: Date) {
    console.log(this.viewDate);
    console.log(viewDate);
    console.log(this.contextService.getAppointmentDate());

    if (this.contextService.getAppointmentDate() != undefined) {
      this.viewDate = this.contextService.getAppointmentDate();
      this.contextService.setAppointmentId(0, undefined);
    }
    const appointments = this.trainingsAppointmentClient.getCalenderAppointments(this.group.id, this.getStartDate(), this.getEndDate());
    this.events$ = appointments.pipe(
      map((results: CalenderAppointmentDto[]) => {
        return results.map((appointment: CalenderAppointmentDto) => {
          appointment.startTime.setHours(appointment.startTime.getHours() - appointment.startTime.getTimezoneOffset() / 60);
          appointment.endTime.setHours(appointment.endTime.getHours() - appointment.endTime.getTimezoneOffset() / 60);
          return {
            id: appointment.id,
            title: appointment.title,
            start: appointment.startTime,
            end: appointment.endTime,
            desc: appointment.modulelist,
            color: colors.red,
          };
        });
      })
      //, tap(results => console.log("finished", results))
    )
    //appointments.subscribe(appointments => {
    //  const currentApp = appointments.find(a => a.id == this.contextService.getAppointmentId());
    //  if (currentApp) {
    //    this.viewDate = currentApp.startTime;
    //    //this.contextService.setAppointmentId(0,new Date());
    //  }
    //})
  }
  getStartDate(): Date {
    if (this.view == CalendarView.Week)
      return startOfWeek(this.viewDate);
    else if (this.view == CalendarView.Month)
      return startOfMonth(this.viewDate);
    return startOfDay(this.viewDate);
  }

  getEndDate(): Date {
    if (this.view == CalendarView.Week)
      return endOfWeek(this.viewDate);
    else if (this.view == CalendarView.Month)
      return endOfMonth(this.viewDate);
    return endOfDay(this.viewDate);
  }

  generateDescription(tatm: TrainingsAppointmentTrainingsModuleDto[]): string {
    let result: string = "";
    tatm.forEach(element => {
      result = result.concat(" - ").concat(element.trainingsModule.title).concat(" <br /> ");
    })
    return result;
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
      }
    }
  }

  eventClicked(event: CalendarEvent): void {
    //TODO: Link anpassen
    this.contextService.setAppointmentId(Number(event.id), event.start);
    this.router.navigateByUrl('training');
  }
}

export const colors: any = {
  red: {
    primary: '#FFFFFF',
    secondary: '#ee232a',
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

//E: \WS\wadltrainer\Trainingsplanner.Postgres\ClientApp\node_modules\calendar - utils\calendar - utils.d.ts
export interface CalendarEvent<MetaType = any> {
  id?: string | number;
  start: Date;
  end?: Date;
  title: string;
  color?: EventColor;
  actions?: EventAction[];
  allDay?: boolean;
  hovered?: boolean;
  cssClass?: string;
  resizable?: {
    beforeStart?: boolean;
    afterEnd?: boolean;
  };
  draggable?: boolean;
  meta?: MetaType;
}
export interface EventColor {
  primary: string;
  secondary: string;
}
export interface EventAction {
  id?: string | number;
  label: string;
  cssClass?: string;
  a11yLabel?: string;
  onClick({ event, sourceEvent, }: {
    event: CalendarEvent;
    sourceEvent: MouseEvent | KeyboardEvent;
  }): any;
}

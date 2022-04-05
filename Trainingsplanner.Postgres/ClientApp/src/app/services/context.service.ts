import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ContextService {

  constructor() { }

  private appointmentId: number = 0;
  private appointmentDate: Date;
  public editAppointment: boolean = false;

  setAppointmentId(id:number,date:Date) {
    this.appointmentId = id;
    this.appointmentDate = date;
  }

  getAppointmentId() {
    return this.appointmentId;
  }

  getAppointmentDate() {
    return this.appointmentDate;
  }

  private groupId: number = 0;

  setGroupId(id) {
    this.groupId = id;
  }

  getGroupId() {
    return this.groupId;
  }
}

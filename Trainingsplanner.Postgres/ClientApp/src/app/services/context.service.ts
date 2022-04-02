import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ContextService {

  constructor() { }

  private appointmentId: number = 0;
  public editAppointment: boolean = false;

  setAppointmentId(id) {
    this.appointmentId = id;
  }

  getAppointmentId() {
    return this.appointmentId;
  }

  private groupId: number = 0;

  setGroupId(id) {
    this.groupId = id;
  }

  getGroupId() {
    return this.groupId;
  }
}

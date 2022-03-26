import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ContextService {

  constructor() { }

  private appointmentId: number;
  public editAppointment: boolean = false;

  setAppointmentId(id) {
    this.appointmentId = id;
  }

  getAppointmentId() {
    const id = this.appointmentId;
    this.appointmentId = 0;
    return id;
  }

  private groupId: number = 0;

  setGroupId(id) {
    this.groupId = id;
  }

  getGroupId() {
    const id = this.groupId;
    return id;
  }
}

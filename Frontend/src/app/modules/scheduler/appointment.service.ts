import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/shared/services/api-service/api.service';
import { Observable, of } from 'rxjs';
import { IGetAppointmentResponse, IAddAppointmentResponse } from 'src/app/shared/models/appointment.interface';
import { UtilsService } from 'src/app/shared/services/utils/utils.service';

@ Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  constructor(private apiService: ApiService, private utilsService: UtilsService) { }

  getAppointmentInfo(body: any): Observable< IGetAppointmentResponse> {
    return this.apiService.postRequest(`${this.utilsService.getAppointmentURL()}/api/appointments/search`, body);
  }

  getAppointmentTypes(): Observable<any> {
    return this.apiService.getRequest(`${this.utilsService.getAppointmentURL()}/api/appointments/types`);
  }

  getReasons(): Observable<any> {
    return this.apiService.getRequest(`${this.utilsService.getAppointmentURL()}/api/appointments/reasons/codes`);
  }

  addAppointment(body: any): Observable<IAddAppointmentResponse> {
    // return of(body);
    const appointmentBody = {
      appointments: body
    };
    return this.apiService.postRequest(`${this.utilsService.getAppointmentURL()}/api/appointments`, appointmentBody);
  }

  getPreferredTimeslot(body: any): Observable< any> {
    return this.apiService.postRequest(`${this.utilsService.getAppointmentURL()}/api/appointments/timeslots/search`, body);
  }

}

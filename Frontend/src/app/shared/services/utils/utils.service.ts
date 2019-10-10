import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UtilsService {

  constructor() { }

  getAuthURL(): string {
    return environment.authUrl;
  }

  getAppointmentURL(): string {
    return environment.appointmentUrl;
  }

  getResourceURL(): string {
    return environment.resourceUrl;
  }

  getCustomerURL(): string {
    return environment.customerUrl;
  }
}

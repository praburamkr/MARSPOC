import { Action } from '@ngrx/store';
import { IGetAppointmentResponse, IAddAppointmentResponse, IAddAppointmentRequest, ISearchAppointments, Appointment } from 'src/app/shared/models/appointment.interface';

export enum AppointmentActionTypes {
  Get = '[Appointment] Get',
  GetSuccess = '[Appointment] Get Success',
  Add = '[Appointment] Add',
  AddSuccess = '[Appointment] Add Success',
  Search = '[Appointment] Search',
  SearchSuccess = '[Appointment] Search Success',
  StoreNewAppointment = '[Appointment] Store New Appointment',
  GetNewAppointment = '[Appointment] Get New Appointment',
  GetPreferredTimeslot = '[Appointment] Get Preferred Timeslot',
  GetPreferredTimeslotSuccess = '[Appointment] Get Preferred Timeslot Success'
}

export class AppointmentActionEx implements Action {
  readonly type: string;
  payload?: any;
  newAppointments?: Appointment[];
}

export class GetAppointment implements AppointmentActionEx {
  readonly type = AppointmentActionTypes.Get;

  constructor(public payload: ISearchAppointments) { }
}

export class GetAppointmentSuccess implements AppointmentActionEx {
  readonly type = AppointmentActionTypes.GetSuccess;
  constructor(public payload: IGetAppointmentResponse) {
  }
}

export class AddAppointment implements AppointmentActionEx {
  readonly type = AppointmentActionTypes.Add;
  constructor(public payload: Array<IAddAppointmentRequest>) { }
}

export class AddAppointmentSuccess implements AppointmentActionEx {
  readonly type = AppointmentActionTypes.AddSuccess;
  constructor(public payload: Array<IAddAppointmentResponse>) { }
}

export class SearchAppointment implements AppointmentActionEx {
  readonly type = AppointmentActionTypes.Search;
  constructor(public payload: ISearchAppointments) { }
}

export class SearchAppointmentSuccess implements AppointmentActionEx {
  readonly type = AppointmentActionTypes.SearchSuccess;
  constructor(public payload: Array<IGetAppointmentResponse>) { }
}

export class GetPreferredTimeslot implements AppointmentActionEx {
  readonly type = AppointmentActionTypes.GetPreferredTimeslot;
  constructor(public payload: any) { }
}

export class GetPreferredTimeslotSuccess implements AppointmentActionEx {
  readonly type = AppointmentActionTypes.GetPreferredTimeslotSuccess;
  constructor(public payload: Array<any>) { }
}

export class StoreNewAppointment implements AppointmentActionEx {
  readonly type = AppointmentActionTypes.StoreNewAppointment;
  constructor(public newAppointments: Array<Appointment>) { }
}

export class GetNewAppointment implements AppointmentActionEx {
  readonly type = AppointmentActionTypes.GetNewAppointment;
  // constructor(public newAppointments: Array<Appointment>) { }
}

export type AppointmentActions = GetAppointment | GetAppointmentSuccess | AddAppointment | AddAppointmentSuccess
  | SearchAppointment | SearchAppointmentSuccess | StoreNewAppointment | GetNewAppointment | GetPreferredTimeslot | GetPreferredTimeslotSuccess;

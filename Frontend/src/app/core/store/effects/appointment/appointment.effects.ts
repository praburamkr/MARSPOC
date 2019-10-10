import { Injectable } from '@angular/core';
import { Effect, Actions, ofType } from '@ngrx/effects';
import { switchMap } from 'rxjs/operators';
import { of } from 'rxjs';

import {
  GetAppointment,
  GetAppointmentSuccess,
  AppointmentActionTypes,
  AddAppointment,
  AddAppointmentSuccess,
  SearchAppointment,
  SearchAppointmentSuccess,
  GetPreferredTimeslotSuccess,
  GetPreferredTimeslot
} from '../../actions/appointment/appointment.actions';
import { AppointmentService } from 'src/app/modules/scheduler/appointment.service';
import { IGetAppointmentResponse } from 'src/app/shared/models/appointment.interface';

@Injectable()
export class AppointmentEffect {
  @Effect()
  $getAppointments = this._actions$.pipe(
    ofType<GetAppointment>(AppointmentActionTypes.Get),
    switchMap((action) => this._appointmentService.getAppointmentInfo(action.payload)),
    switchMap((client: IGetAppointmentResponse) => {
      return of(new GetAppointmentSuccess(client));
    })
  );

  @Effect()
  $addAppointment = this._actions$.pipe(
    ofType<AddAppointment>(AppointmentActionTypes.Add),
    switchMap(action => this._appointmentService.addAppointment(action.payload)),
    switchMap((appointmentAdd: any) => of(new AddAppointmentSuccess(appointmentAdd)))
  );

  @Effect()
  $searchAppointment = this._actions$.pipe(
    ofType<SearchAppointment>(AppointmentActionTypes.Search),
    switchMap(action => this._appointmentService.addAppointment(action.payload)),
    switchMap((searchedAppointments: any) => of(new SearchAppointmentSuccess(searchedAppointments)))
  );

  @Effect()
  $getPreferredTimeslot = this._actions$.pipe(
    ofType<GetPreferredTimeslot>(AppointmentActionTypes.GetPreferredTimeslot),
    switchMap(action => this._appointmentService.getPreferredTimeslot(action.payload)),
    switchMap((preferredTimeSlot: any) => of(new GetPreferredTimeslotSuccess(preferredTimeSlot)))
  );

  // tslint:disable-next-line: variable-name
  constructor(private _actions$: Actions, private _appointmentService: AppointmentService) { }
}

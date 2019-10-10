import { Injectable } from '@angular/core';
import { Effect, Actions, ofType } from '@ngrx/effects';
import { switchMap } from 'rxjs/operators';
import { of } from 'rxjs';

import {
  GetDoctor,
  GetDoctorSuccess
} from '../../actions/doctor/doctor.actions';
import { DoctorActionTypes } from '../../actions/doctor/doctor.actions';
import { DoctorService } from 'src/app/modules/scheduler/doctor.service';
import { IGetDoctorResponse } from 'src/app/shared/models/doctor.interface';

@Injectable()
export class DoctorEffect {
  @Effect()
  $getDoctors = this._actions$.pipe(
    ofType<GetDoctor>(DoctorActionTypes.Get),
    switchMap((action) => this._doctorService.getAllDoctors(action.payload)),
    switchMap((client: IGetDoctorResponse) => {
      return of(new GetDoctorSuccess(client));
    })
  );

  constructor(private _actions$: Actions, private _doctorService: DoctorService) { }
}

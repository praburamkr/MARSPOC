import { createSelector } from '@ngrx/store';

import { IAppState } from '../../state/app.state';
import { IDoctorState } from '../../state/doctor/doctor.state';

const doctorState = (state: IAppState) => state.doctor;

export const getDoctorData = createSelector(
  doctorState,
  (state: IDoctorState) => state
);

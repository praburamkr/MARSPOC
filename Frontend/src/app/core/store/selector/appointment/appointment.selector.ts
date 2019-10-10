import { createSelector } from '@ngrx/store';

import { IAppState } from '../../state/app.state';
import { IAppointmentState } from 'src/app/core/store/state/appointment/appointment.state';

const appointmentState = (state: IAppState) => state.appointment;

export const getAppointmentData = createSelector(
  appointmentState,
  (state: IAppointmentState) => state
);

export const getPreferredTimeslotData = createSelector(
  appointmentState,
  (state: IAppointmentState) => state
);

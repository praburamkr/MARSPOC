import { ActionReducerMap } from '@ngrx/store';

import { IAppState } from '../state/app.state';
import { clientReducer } from './client/client.reducer';
import { appointmentReducer } from './appointment/appointment.reducer';
import { miscReducer } from './misc/misc.reducer';
import { doctorReducer } from './doctor/doctor.reducer';

export const appReducer: ActionReducerMap<IAppState, any> = {
    client: clientReducer,
    appointment: appointmentReducer,
    misc: miscReducer,
    doctor: doctorReducer
};

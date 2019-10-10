import { IClientState } from './client/client.state';
import { IAppointmentState } from 'src/app/core/store/state/appointment/appointment.state';
import { Appointment } from 'src/app/shared/models/appointment.interface';
import { IMiscState } from './misc/misc.state';
import { IDoctorState } from './doctor/doctor.state';

export interface IAppState {
    client: IClientState;
    appointment: IAppointmentState;
    misc: IMiscState;
    doctor: IDoctorState;
}

export const intialAppState: IAppState = {
    client: null,
    appointment: null,
    misc: null,
    doctor: null
};

export function getIntialState(): IAppState {
    return intialAppState;
}

import { Appointment } from 'src/app/shared/models/appointment.interface';

export interface IAppointmentState {
  appointment: {};
  newAppointments: Array<Appointment>;
}

export const intialClientState: IAppointmentState = {
  appointment: {},
  newAppointments: []
};

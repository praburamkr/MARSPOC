import {
    AppointmentActionEx,
    AppointmentActionTypes
} from 'src/app/core/store/actions/appointment/appointment.actions';
import {
    IAppointmentState,
    intialClientState
} from 'src/app/core/store/state/appointment/appointment.state';
import { actionComplete } from '../../../../../../node_modules/@syncfusion/ej2-schedule';

export const initialState: IAppointmentState = intialClientState;

function formatAMPM(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}
export function appointmentReducer(
    state = initialState,
    action: AppointmentActionEx
): IAppointmentState {
    switch (action.type) {
        case AppointmentActionTypes.SearchSuccess:
            return {
                ...state,
                appointment: action.payload
            };
        case AppointmentActionTypes.GetSuccess:
            const appointments = [];
            if (action.payload != null && action.payload.data != null) {
                action.payload.data.forEach(element => {

                    const apptStartTimeTemp = new Date(element.appt_start_time);
                    const apptStartTime = formatAMPM(apptStartTimeTemp);

                    appointments.push({
                        Id: element.appointment_id,
                        Subject: `<div class="icon ${element.appt_type.appt_type_text.toLowerCase()}"><img src="${element.appt_type.img_url}" /></div>
                                    <div class="name">${element.client_name}</div>
                                    <div class="pet-name">${element.patient_name}</div>
                                    <div class="seperator"></div>
                                    <div class="type">${element.appt_type.appt_type_text}</div>
                                    <div class="start-time">${apptStartTime}</div>
                                    <div class="appointment-user"><img src="${element.client_img_url}" /></div>
                                    <div class="appointment-complete"><img src="../../../../assets/icons/${element.appt_status}.png" /></div>
                                    <div class="reasons">${element.reason ? element.reason.reason_text : ''}</div>
                                    <div class="reason-description">${element.note_for_doctor}</div>
                                    <div class="tooltiptext">
                                        <div class="footer-icon"><img src="../../../../assets/icons/footer-icon-1.png" /></div>
                                        <div class="footer-icon"><img src="../../../../assets/icons/footer-icon-2.png" /></div>
                                        <div class="footer-icon"><img src="../../../../assets/icons/footer-icon-3.png" /></div>
                                        <div class="footer-icon"><img src="../../../../assets/icons/footer-icon-4.png" /></div>
                                        <div class="footer-icon"><img src="../../../../assets/icons/footer-icon-5.png" /></div>
                                    </div>`,
                        StartTime: new Date(element.appt_start_time),
                        EndTime: new Date(element.appt_end_time),
                        IsAllDay: false,
                        DoctorId: element.doctor_id
                    });
                });
            }
            return {
                ...state,
                appointment: appointments
            };
        case AppointmentActionTypes.AddSuccess:
            return {
                ...state,
                appointment: action.payload
            };
        case AppointmentActionTypes.StoreNewAppointment:
            return {
                ...state,
                newAppointments: action.newAppointments
            }
        case AppointmentActionTypes.GetNewAppointment:
            return {
                ...state
            }
        case AppointmentActionTypes.GetPreferredTimeslot:
            return {
                ...state,
                appointment: action.payload
            };
        case AppointmentActionTypes.GetPreferredTimeslotSuccess:
            return {
                ...state,
                appointment: action.payload
            };
        default:
            return { ...state };
    }
}
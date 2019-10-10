export interface IGetAppointmentResponse {
    appointment_id: number;
    appt_scan_id: string;
    appt_link_id: number;
    appt_type_id: number;
    appt_sub_type_id: number;
    patient_id: number;
    patient_name: string;
    patient_img_url: string;
    client_id: number;
    client_name: string;
    client_img_url: string;
    appt_date: string;
    appt_start_time: number;
    appt_end_time: number;
    doctor_id: number;
    appt_status: number;
    appt_workflow_state: number;
}

export interface ISearchAppointments {
    appt_link_id: number;
    appt_type_id: number;
    appt_sub_type_id: number;
    patient_id: number;
    client_id: number;
    appt_start_time: string;
    appt_end_time: string;
    doctor_ids?: Array<number>;
}

export interface IPreferredTimeslot {
    appt_type_id: number,
    appt_sub_type_id: number,
    patient_id: number,
    client_id: number,
    appt_date: string,
    default_slot: boolean
}

export interface IAddAppointmentResponse {
    appointment_id: number;
}

export interface IAddAppointmentRequest {
    appointments: Array<Appointment>;
}

export interface Appointment {
    dept_id: number;
    appt_type_id: number;
    appt_sub_type_id: number;
    patient_id: number;
    patient_img_url: string;
    client_id: number;
    appt_date: string;
    appt_start_time: number;
    appt_end_time: number;
    duration: number;
    reason_code_id: number;
    doctor_id: number;
    note_for_doctor: string;
    is_email: boolean;
    email: string
}

export interface IClient {
    client_id?: number;
    client_name?: string;
    address?: string;
    phone?: string;
    email?: string;
    patients?: IClientPatient[];
    img_url?: string;
}

export interface IClientByName {
    client_id?: number;
    client_name?: string;
    address?: string;
    phone?: string;
    email?: string;
    patients?: IClientPatient[];
    img_url?: string;
}

export interface IClientPatient {
    patient_id: number;
    client_id: number;
    patient_name: string;
    species_id: number;
    species_name: string;
    breed: string;
    color_pattern: string;
    sex: string;
    age: number;
    birth_date: string;
    weight: number;
    weight_uom: string;
    pref_doctor_id: number;
    img_url: string;
}

export interface IGetClientPatient {
    client_id: number;
}

export interface IGetClientPatientResponse {
    patient_id: number;
    client_id: number;
    patient_name: string;
    species_id: number;
    species_name: string;
    breed: string;
    color_pattern: string;
    sex: string;
    age: number;
    birth_date: string;
    weight: number;
    weight_uom: string;
    pref_doctor: number;
    img_url: string;
}

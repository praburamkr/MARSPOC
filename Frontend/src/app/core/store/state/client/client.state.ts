import { IClient, IClientPatient } from '../../../../shared/models/client.interface';

export interface IClientState {
    client: {
        patients: Array<IClientPatient>
    };
    patientClient: {};
}

export interface IClientsState {
    client: {
        patients: Array<IClientPatient>
    };
    patientClient: {};
}

export const intialClientState: IClientState = {
    client: {
        patients: null
    },
    patientClient: {}
};

import { ClientActionEx, ClientActionTypes } from '../../actions/client/client.actions';
import { IClientState, intialClientState } from '../../state/client/client.state';
import { IClientPatient } from 'src/app/shared/models/client.interface';

export const initialState: IClientState = intialClientState;

export function clientReducer(state = initialState, action: ClientActionEx): IClientState {
    switch (action.type) {
        case ClientActionTypes.AddClientPatient:
            state.patientClient = action.payload;
            return {
                ...state
            };
        case ClientActionTypes.AddClientPatientSuccess:
            return {
                ...state,
                client: action.payload
            };
        case ClientActionTypes.GetSuccess:
            return {
                ...state,
                client: action.payload
            };
        case ClientActionTypes.GetClientPatientSuccess:
            return {
                ...state,
                client: action.payload
            };
        case ClientActionTypes.GetAll:
            return {
                ...state,
                client: action.payload
            };
        default:
            return { ...state };
    }
}

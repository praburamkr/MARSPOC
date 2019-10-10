import { intialDoctorState, IDoctorState } from '../../state/doctor/doctor.state';
import { DoctorActionEx, DoctorActionTypes } from '../../actions/doctor/doctor.actions';

export const initialState: IDoctorState = intialDoctorState;

export function doctorReducer(
    state = initialState,
    action: DoctorActionEx
): IDoctorState {
    switch (action.type) {
        case DoctorActionTypes.GetSuccess:
            return {
                ...state,
                doctor: action.payload
            };
        default:
            return { ...state };
    }
}
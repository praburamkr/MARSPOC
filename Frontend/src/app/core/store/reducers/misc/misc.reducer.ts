import { IMiscState, intialClientState } from '../../state/misc/misc.state';
import { MiscActionEx, MiscActionTypes } from '../../actions/misc/misc.actions';

export const intialState: IMiscState = intialClientState;

export function miscReducer(state = intialState, action: MiscActionEx): IMiscState {
    switch (action.type) {
        case MiscActionTypes.Add:
            return {
                ...state,
                misc: action.payload
            };
        case MiscActionTypes.Get:
            return {
                ...state
            };
        default:
            return { ...state };
    }
}

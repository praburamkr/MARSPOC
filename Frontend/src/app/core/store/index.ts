import { clientReducer } from './reducers/client/client.reducer';

export interface State {
    clients: any
}

export const reducers = {
    clients: clientReducer
};

export const getClients = (state: State) => state.clients;

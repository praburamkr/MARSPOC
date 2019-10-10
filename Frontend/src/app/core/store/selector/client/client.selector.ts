import { createSelector } from '@ngrx/store';

import { IAppState } from '../../state/app.state';
import { IClientState, IClientsState } from '../../state/client/client.state';

const clientState = (state: IAppState) => state.client;

export const selectClientData = createSelector(
    clientState,
    (state: IClientState) => state.client
);

export const searchClientData = createSelector(
    clientState,
    (state: IClientState) => state.client
);

export const addPatientData = createSelector(
    clientState,
    (state: IClientState) => state.client
);

export const getClientData = createSelector(
    clientState,
    (state: IClientState) => state.client
);

export const getClientDataByName = createSelector(
    clientState,
    (state: IClientsState) => state.client
);

import { Injectable } from '@angular/core';
import { Effect, Actions, ofType } from '@ngrx/effects';
import { switchMap, map } from 'rxjs/operators';
import { of } from 'rxjs';

import {
    GetClient,
    GetClients,
    ClientActionTypes,
    GetClientSuccess,
    AddClientPatient,
    AddClientPatientSuccess,
    GetClientPatient,
    GetClientPatientSuccess,
    GetClientByName
} from '../../actions/client/client.actions';
import { IClient, IClientByName } from 'src/app/shared/models/client.interface';
import { ClientService } from 'src/app/modules/shared/client/client.service';

@Injectable()
export class ClientEffect {
    @Effect()
    $getClients = this._actions$.pipe(
        ofType<GetClients>(ClientActionTypes.Get),
        switchMap((action) => this._clientService.getAllClientsByName(action.payload)),
        switchMap((client: IClient) => {
            return of(new GetClientSuccess(client));
        })
    );

    @Effect()
    $addClientPatient = this._actions$.pipe(
        ofType<AddClientPatient>(ClientActionTypes.AddClientPatient),
        switchMap(action => this._clientService.addClientPetInfo(action.payload)),
        switchMap((clientAdd: any) => {
            return of(new AddClientPatientSuccess(clientAdd));
        })
    );

    @Effect()
    $getClientPatients = this._actions$.pipe(
        ofType<any>(ClientActionTypes.GetClientPatient),
        switchMap(action => this._clientService.getClientPatients(action.clientId)),
        switchMap((clientPatient: any) => {
            return of(new GetClientPatientSuccess(clientPatient));
        })
    );

    @Effect()
    $getAllClients = this._actions$.pipe(
        ofType<GetClient>(ClientActionTypes.Get),
        switchMap((action) => this._clientService.getAllClients()),
        switchMap((client: IClient) => {
            return of(new GetClientSuccess(client));
        })
    );

    @Effect()
    $getAllClientsByName = this._actions$.pipe(
        ofType<GetClientByName>(ClientActionTypes.GetAll),
        switchMap((action) => this._clientService.getAllClientsByName(action.payload)),
        switchMap((client: IClientByName) => {
            return of(new GetClientSuccess(client));
        })
    );


    // tslint:disable-next-line: variable-name
    constructor(private _actions$: Actions, private _clientService: ClientService) { }
}

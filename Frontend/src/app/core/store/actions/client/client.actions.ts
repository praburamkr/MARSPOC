import { Action } from '@ngrx/store';
import { IClient, IClientPatient, IGetClientPatient, IClientByName } from 'src/app/shared/models/client.interface';

export enum ClientActionTypes {
  AddClientPatient = '[Client] Add Patient',
  AddClientPatientSuccess = '[Client] Add Patient Success',
  Get = '[Client] Get',
  GetAll = '[Client] GetAll',
  GetSuccess = '[Client] Get Success',
  GetClientPatient = '[Client] Get Client Patient',
  GetClientPatientSuccess = '[Client] Get Client Patient Success'
}

export class ClientActionEx implements Action {
  readonly type: string;
  payload?: any;
}

export class AddClientPatient implements ClientActionEx {
  readonly type = ClientActionTypes.AddClientPatient;
  constructor(public payload: IClient) {
  }
}

export class AddClientPatientSuccess implements ClientActionEx {
  readonly type = ClientActionTypes.AddClientPatientSuccess;
  constructor(public payload: IClientPatient) {
  }
}

export class GetClient implements ClientActionEx {
  readonly type = ClientActionTypes.Get;
}

export class GetClients implements ClientActionEx {
  readonly type = ClientActionTypes.Get;
  constructor(public payload: IClient) { }
}

export class GetClientByName implements ClientActionEx {
  readonly type = ClientActionTypes.GetAll;
  constructor(public payload: IClientByName) { }
}


export class GetClientSuccess implements ClientActionEx {
  readonly type = ClientActionTypes.GetSuccess;
  constructor(public payload: IClient) {
  }
}

export class GetClientPatient implements ClientActionEx {
  readonly type = ClientActionTypes.GetClientPatient;
  constructor(public clientId: number) { }
}

export class GetClientPatientSuccess implements ClientActionEx {
  readonly type = ClientActionTypes.GetClientPatientSuccess;
  constructor(public payload: IClient) {
  }
}

export type ClientActions = AddClientPatient | AddClientPatientSuccess | GetClient | GetClientSuccess | GetClientPatient | GetClientPatientSuccess | GetClientByName;

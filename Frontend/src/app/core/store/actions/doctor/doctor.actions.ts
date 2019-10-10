import { Action } from '@ngrx/store';
import { IGetDoctorResponse, IGetDoctor } from 'src/app/shared/models/doctor.interface';

export enum DoctorActionTypes {
  Get = '[Doctor] Get',
  GetSuccess = '[Doctor] Get Success',
}

export class DoctorActionEx implements Action {
  readonly type: string;
  payload?: any;
}

export class GetDoctor implements DoctorActionEx {
  readonly type = DoctorActionTypes.Get;

  constructor(public payload: IGetDoctor) { }
}

export class GetDoctorSuccess implements DoctorActionEx {
  readonly type = DoctorActionTypes.GetSuccess;
  constructor(public payload: IGetDoctorResponse) {
  }
}

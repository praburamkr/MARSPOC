import {Action} from '@ngrx/store';
import { IMisc } from 'src/app/shared/models/misc.interface';

export enum MiscActionTypes {
    Add = '[Misc] AddMisc',
    Get = '[Misc] Get',
}

export class MiscActionEx implements Action {
    readonly type: string;
    payload?: any;
}

export class AddMisc implements MiscActionEx {
    readonly type = MiscActionTypes.Add;
    constructor(public payload: IMisc) {
    }
}

export class GetMisc implements MiscActionEx {
    readonly type = MiscActionTypes.Get;
}

export type MiscActions = AddMisc | GetMisc;

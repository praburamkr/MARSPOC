import { createSelector } from '@ngrx/store';

import { IAppState } from '../../state/app.state';
import { IMiscState } from '../../state/misc/misc.state';

const miscState = (state: IAppState) => state.misc;

export const selectMiscData = createSelector(
    miscState,
    (state: IMiscState) => state.misc
);

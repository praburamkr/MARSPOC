import { NgModule } from '@angular/core';

import { TreeViewModule } from '@syncfusion/ej2-angular-navigations';
import { DropDownListAllModule, MultiSelectAllModule } from '@syncfusion/ej2-angular-dropdowns';
import { MaskedTextBoxModule, UploaderAllModule } from '@syncfusion/ej2-angular-inputs';
import { ToolbarAllModule, ContextMenuAllModule } from '@syncfusion/ej2-angular-navigations';
import { ButtonAllModule } from '@syncfusion/ej2-angular-buttons';
import { CheckBoxAllModule } from '@syncfusion/ej2-angular-buttons';
import { DatePickerAllModule, TimePickerAllModule, DateTimePickerAllModule } from '@syncfusion/ej2-angular-calendars';
import { NumericTextBoxAllModule } from '@syncfusion/ej2-angular-inputs';
import { ScheduleAllModule, RecurrenceEditorAllModule } from '@syncfusion/ej2-angular-schedule';

// import { SharedModule } from 'src/app/shared/shared.module';
import { AppointmentSliderComponent } from './appointment-slider/appointment-slider.component';
import { FilterComponent } from './filter/filter.component';
import { CalendarComponent } from './calendar/calendar.component';
// import { ClientModule } from '../client/client.module';
import { ClientModule } from '../shared/client/client.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';

import {SharedModule} from '../shared/shared.module'
import { CommonModule } from '@angular/common';
import { PhoneCallPluginComponent } from './phone-call-plugin/phone-call-plugin.component';

@NgModule({
    declarations: [
        AppointmentSliderComponent,
        FilterComponent,
        CalendarComponent,
        PhoneCallPluginComponent
    ],
    imports: [
        TreeViewModule,
        DropDownListAllModule,
        MultiSelectAllModule,
        MaskedTextBoxModule,
        UploaderAllModule,
        ToolbarAllModule,
        ContextMenuAllModule,
        ButtonAllModule,
        CheckBoxAllModule,
        DatePickerAllModule,
        TimePickerAllModule,
        DateTimePickerAllModule,
        NumericTextBoxAllModule,
        // SharedModule,
        ClientModule,
        NgMultiSelectDropDownModule,
        SharedModule,
        ScheduleAllModule,
        RecurrenceEditorAllModule,
        CommonModule
    ],
    exports: [
        AppointmentSliderComponent,
        FilterComponent,
        CalendarComponent
    ],
})

export class SchedulerModule { }

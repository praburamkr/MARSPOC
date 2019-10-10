import { NgModule } from '@angular/core';
import { ApiService } from './services/api-service/api.service';
import { ScheduleAllModule, RecurrenceEditorAllModule } from '@syncfusion/ej2-angular-schedule';
import { PatientDetailsComponent } from '../modules/shared/client/patient-details/patient-details.component';
import { ClientPatientContainerComponent } from '../modules/shared/client/client-patient-container.component';
import { CommonModule } from '@angular/common';
import { BookAppointmentComponent } from '../modules/shared/appointment/book-appointment/book-appointment.component';

@NgModule({
  declarations: [
    // PatientDetailsComponent,
    // ClientPatientContainerComponent,
    // BookAppointmentComponent
  ],

  imports: [
    // ScheduleAllModule,
    // RecurrenceEditorAllModule,
    // CommonModule
  ],
  exports: [
    //ScheduleAllModule,
    //RecurrenceEditorAllModule,
    // PatientDetailsComponent,
    // ClientPatientContainerComponent,
    //CommonModule,
    // BookAppointmentComponent
  ],
  providers: []
})

export class SharedModule { }

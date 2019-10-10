import { NgModule } from '@angular/core';
import { ClientInfoComponent } from './client-info/client-info.component';
import { ClientRoutingModule } from './client-routing';
// import { SharedModule } from 'src/app/shared/shared.module';
import { PetDetailsComponent } from './pet-details/pet-details.component';
import { AddPetComponent } from './add-pet/add-pet.component';
import { FormsModule } from '@angular/forms';
import { AppointmentModule } from '../appointment/appointment.module';
import { CommonModule } from '@angular/common';
import { PatientDetailsComponent } from './patient-details/patient-details.component';
import { ClientPatientContainerComponent } from './client-patient-container.component';
import { ApiService } from 'src/app/shared/services/api-service/api.service';
// import { SharedModule } from '../shared.module';

@NgModule({
    declarations: [
        ClientInfoComponent,
        PetDetailsComponent,
        AddPetComponent,
        PatientDetailsComponent,
        ClientPatientContainerComponent,
    ],
    imports: [
        // SharedModule,
        CommonModule,
        ClientRoutingModule,
        FormsModule,
        AppointmentModule
    ],
    exports: [
        ClientInfoComponent
    ],
    providers:[ApiService]
})

export class ClientModule { }

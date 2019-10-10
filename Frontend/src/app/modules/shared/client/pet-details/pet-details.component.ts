import { Component, OnInit, Input, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
// import { ClientInfoModel } from 'src/app/shared/models/client-info.model';
// import { ClientService } from '../client.service';
import { IClient, IClientPatient } from 'src/app/shared/models/client.interface';
import { ClientService } from '../client.service';
import { IAppState } from 'src/app/core/store/state/app.state';
import { Store, select } from '@ngrx/store';
import { AddClientPatient } from 'src/app/core/store/actions/client/client.actions';
import { addPatientData } from 'src/app/core/store/selector/client/client.selector';
import { isNullOrUndefined } from '@syncfusion/ej2-base';


@Component({
  selector: 'app-pet-details',
  templateUrl: './pet-details.component.html',
  styleUrls: ['./pet-details.component.scss']
})
export class PetDetailsComponent {
  @Output() onAddPet: EventEmitter<any> = new EventEmitter<any>();
  @Input() isExpandCollapse: boolean;
  @Input() isBack: boolean;
  addClientPatient$ = this._store.pipe(select(addPatientData));

  isNewPet: boolean = false;
  newPatient: boolean = true;
  clientInfo: IClient;
  showAddAppointment: boolean = false;


  @Input() clientId: number;
  @Input() pets: Array<IClientPatient>;

  selectedPet: IClientPatient;
  // tslint:disable-next-line: variable-name
  constructor(private clientService: ClientService, private _store: Store<IAppState>) {
    console.log(this.pets)
  }

  ngOnInit() {
  }

  addPatientDetails(newPatientDetail: any) {
    // this.clientService.addClientPetInfo(newPatient);
    if (newPatientDetail != null) {
      this.isNewPet = false;
      this.newPatient = true;

      newPatientDetail['client_id'] = this.clientId;
      this._store.dispatch(new AddClientPatient(newPatientDetail));
      this.addClientPatient$.subscribe((event: any) => {
        if (!isNullOrUndefined(event.data.patient_id)) {
          this.onAddPet.emit(null);
        }
      });
      this.isNewPet = false;
      //this.onAddPet.emit(null);
    }
  }
  addPatient() {
    this.isNewPet = true;
    this.newPatient = false;
  }

  addAppointment(pet: IClientPatient) {
    // this.isExpandCollapse = false;
    this.showAddAppointment = !this.showAddAppointment;
    this.selectedPet = pet;


    // const container = document.getElementsByClassName('client-information')[0];
    // container.style.display = 'none';
  }

  close(event: any) {
    this.isNewPet = false;
    this.newPatient = true;
  }
}

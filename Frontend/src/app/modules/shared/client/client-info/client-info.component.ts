import { Component, OnInit, Input } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { IAppState } from 'src/app/core/store/state/app.state';
import { selectClientData, searchClientData } from 'src/app/core/store/selector/client/client.selector';
import { GetClient, GetClientPatient } from 'src/app/core/store/actions/client/client.actions';

@Component({
  selector: 'app-client-info',
  templateUrl: './client-info.component.html',
  styleUrls: ['./client-info.component.scss']
})
export class ClientInfoComponent implements OnInit {
  @Input() clientInfo: Object;
  clients$ = this._store.pipe(select(selectClientData));
  getClientPatient$ = this._store.pipe(select(searchClientData));

  // clientId: number;
  clientPatients = new Array();
  isExpanded: boolean = true;
  isExpandCollapse: boolean = true;
  @Input() isBack: boolean;
  @Input() clientId: number;
  
  // @Input() isExpandCollapse: boolean;
  // tslint:disable-next-line: variable-name
  constructor(private _store: Store<IAppState>) { }

  ngOnInit() {
    this.clientId = 1;
    console.log(this.clientId);
    // this.isBack = false;
    // this.isExpandCollapse = true;
    // this.isExpanded = true;

    this.refreshPatientList();
  }

  refreshPatientList() {
    let patients = this.clientInfo['patients'];
          if (patients != null) {
            for (let i = 0; i < patients.length; i++) {
              this.clientPatients.push(patients[i]);
            }
          }

    // const clientPatientRequest = {
    //   client_id: this.clientId
    // }
    // this._store.dispatch(new GetClientPatient(this.clientId));
    // this.getClientPatient$.subscribe((clientPatientDetails: any) => {
    //   if (clientPatientDetails != null && clientPatientDetails.data != null) {
    //     this.clientPatients = new Array();
    //     let data = clientPatientDetails.data;
    //     if (data != null && data.client_name != null) {
    //       this.clientInfo = data;
    //       let patients = data.patients;
    //       if (patients != null) {
    //         for (let i = 0; i < patients.length; i++) {
    //           this.clientPatients.push(patients[i]);
    //         }
    //       }
    //     }
    //   }
    // });
  }

  showClientInformation() {
    this.isExpandCollapse = !this.isExpandCollapse;

    // console.log(this.isExpandCollapse);
    // var arrow = document.getElementsByClassName('pet-details-container')[0] as HTMLElement;
    // if (arrow !== undefined) {
    //   arrow.style.margin = !this.isExpandCollapse ? '65px 0 0 0' : '165px 0 0 0';
    //   arrow.style.height = !this.isExpandCollapse ? 'calc(100vh - 105px)' : 'calc(100vh - 205px)';
    // }

    var arrowD = document.getElementsByClassName('pet-details')[0] as HTMLElement;
    if (arrowD !== undefined) {
      arrowD.style.margin = (!this.isExpandCollapse) ? '65px 0 0 0' : '0 0 0 0';
      // arrowD.style.height = !this.isExpandCollapse ? 'calc(100vh)' : 'calc(100vh - 205px)';
    }
  }

  close(event: any) {
    this.refreshPatientList();
  }
}

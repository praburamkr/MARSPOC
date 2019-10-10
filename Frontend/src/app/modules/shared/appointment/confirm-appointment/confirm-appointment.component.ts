import { Component, OnInit, Input } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { IAppState } from 'src/app/core/store/state/app.state';
import { AddAppointment } from 'src/app/core/store/actions/appointment/appointment.actions';
import { departments, appointmentSubTypes } from 'src/app/app.constants';
import { AppointmentService } from 'src/app/modules/scheduler/appointment.service';
import { AddMisc } from 'src/app/core/store/actions/misc/misc.actions';
import { selectMiscData } from 'src/app/core/store/selector/misc/misc.selector';

@Component({
  selector: 'app-confirm-appointment',
  templateUrl: './confirm-appointment.component.html',
  styleUrls: ['./confirm-appointment.component.scss']
})
export class ConfirmAppointmentComponent implements OnInit {

  @Input() newAppointments

  @Input() showChild: string;
  
  constructor(private _store: Store<IAppState>,private appointmentService: AppointmentService) { }
  $misc = this._store.pipe(select(selectMiscData));
  @Input() appointmentTypes: Array<object>;
  departments: Array<object>;
  @Input() appointmentSubTypes: Array<object>;
  @Input() reasons: Array<object>;
  // public appointmentType0: string = "Medical";
  // public appointmentType: string = "Examination";
  // public reasons: string = "Skin Rashes, Itching";
  @Input() docName: string;
  @Input() slotName: string;
  // public notes: string = "Rashes all over the body from last 2 days";

  saveAppointment(){
    this._store.dispatch(new AddAppointment(this.newAppointments));
    this._store.dispatch(new AddMisc({ showConfirm: true, noOfAppointments: this.newAppointments.length }));
    setTimeout(() => {
      this._store.dispatch(new AddMisc({ showConfirm: false, noOfAppointments: 0 }));
    }, 1500);
  }

  // tslint:disable-next-line: variable-name
  getAppointmentType(appt_type_id: number): object {
    return this.appointmentTypes.filter(at => {
      // tslint:disable-next-line: no-string-literal
      return at['appt_type_id'] === appt_type_id;
    // tslint:disable-next-line: no-string-literal
    })[0];
  }

  getAppointmentSubType(appt_sub_type_id: string): object {
    return this.appointmentSubTypes.filter(at => {
      // tslint:disable-next-line: no-string-literal
      return at['appt_type_id'] === parseInt(appt_sub_type_id);
    // tslint:disable-next-line: no-string-literal
    })[0];
  }

  getReason(reason_code_id: number): object {
    return this.reasons.filter(at => {
      // tslint:disable-next-line: no-string-literal
      return at['reason_code_id'] === reason_code_id;
    // tslint:disable-next-line: no-string-literal
    })[0];
  }
  ngOnInit() {    
    this.departments = departments;
    // this.appointmentSubTypes = appointmentSubTypes;
    // this.reasons = reasons;
  }

}

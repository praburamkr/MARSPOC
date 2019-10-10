import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IClientPatient } from 'src/app/shared/models/client.interface';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { Store, select } from '@ngrx/store';
import { IAppState } from 'src/app/core/store/state/app.state';
import { getAppointmentData, getPreferredTimeslotData } from 'src/app/core/store/selector/appointment/appointment.selector';
import { StoreNewAppointment, GetNewAppointment, GetPreferredTimeslot } from 'src/app/core/store/actions/appointment/appointment.actions';
import { departments } from 'src/app/app.constants';
import { AppointmentService } from 'src/app/modules/scheduler/appointment.service';
import { selectMiscData } from 'src/app/core/store/selector/misc/misc.selector';

@Component({
    selector: 'app-book-appointment',
    templateUrl: './book-appointment.component.html',
    styleUrls: ['./book-appointment.component.scss']
})
export class BookAppointmentComponent implements OnInit {
    // searchAppointments$ = this._store.pipe(select(getAppointmentData));
    getPreferredTimeslot$ = this._store.pipe(select(getPreferredTimeslotData));

    @Input('patient') patient: IClientPatient;
    @Input() isBack: boolean = false;

    @Output() onPrevious: EventEmitter<any> = new EventEmitter<any>()
    noOfAppointments: number = 1;
    constructor(private _store: Store<IAppState>, private appointmentService: AppointmentService) { }
    $newAppointments;
    appointmentForm: FormGroup;
    appointmentTypes: Array<object>;
    departments: Array<object>;
    appointmentSubTypes: Array<object>;
    reasons: Array<object>;
    // $misc = this._store.pipe(select(selectMiscData));


    //public appointments: string = "1 Appointment";
    public bgColor: string = "'#A86CF3'";

    public fColor: string = "'#000000'";

    public docName: string = "Click to select a doctor,";

    public slotName: string = "duration, date and time.";

    public slotString: string = "";



    public show: boolean = true;

    public showConf: boolean = false;

    appointmentFormArray: FormArray;

    public selectedReasons: Array<String>;

    public revVar: boolean = false;

    formatAMPM(date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'PM' : 'AM';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;
        return strTime;
    }

    populateTime(slotDate: Date, slotStartDate: Date, slotEndDate: Date) {
        this.slotName = slotDate.toDateString().substr(0, 3) + ', ' +
            slotDate.toDateString().substr(4, 3) + ' ' + slotDate.getDate() +
            ", " + new Date().getFullYear() + " | " + this.formatAMPM(slotStartDate) +
            " - " + this.formatAMPM(new Date((slotEndDate.getTime() + (30 * 60000))));
        //this.slotName = "Thu, Sept 20, 2019 | 12:00 - 12:30 PM";
    }

    formatDate(date) {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2)
            month = '0' + month;
        if (day.length < 2)
            day = '0' + day;

        return [year, month, day].join('/');
    }

    suggestTimeSlot() {
        // this.docName = `<b>Dr. Maria Rodriguez</b>`;
        const body = {
            appt_type_id: 0,
            appt_sub_type_id: 1,
            patient_id: 1,
            client_id: 0,
            appt_date: this.formatDate(new Date()),
            default_slot: true
        };
        this._store.dispatch(new GetPreferredTimeslot(body));

        this.getPreferredTimeslot$.subscribe((timeSlotDetails: any) => {
            if (this.getPreferredTimeslot$['actionsObserver']['value']['type'] == "[Appointment] Get Preferred Timeslot Success") {
                let obj = timeSlotDetails;
                let timeSlots = obj.appointment.data;
                let preferredDoctor: Object = new Object();
                if (timeSlots != null && timeSlots.length > 0) {
                    preferredDoctor = timeSlots[0];
                }
                this.docName = preferredDoctor["Name"];
                this.populateTime(new Date(preferredDoctor["StartTime"]), new Date(preferredDoctor["StartTime"]), new Date(preferredDoctor["EndTime"]));
                this.revVar = true;
            }
        });

    }

    showHide() {
        this.show = !this.show;
    }

    showConfirmation() {
        this.showConf = !this.showConf;
    }

    onPreviousClicked() {
        this.onPrevious.emit();

        const container = document.getElementsByClassName('client-information')[0] as HTMLElement;
        container.style.display = 'block';
    }

    ngOnInit() {

        this.appointmentService.getAppointmentTypes().subscribe(
            // tslint:disable-next-line: no-string-literal
            (appointmentTypes: object) => {
                this.appointmentTypes = appointmentTypes['data'].filter(at => at.parent_type_id === 0);
                this.appointmentSubTypes = appointmentTypes['data'].filter(at => at.parent_type_id === 1);
            });
        // this.appointmentTypes =  appointmentTypes;
        this.appointmentService.getReasons().subscribe(
            // tslint:disable-next-line: no-shadowed-variable
            // tslint:disable-next-line: no-string-literal
            (reasons: object) => this.reasons = reasons['data']
        );
        this.departments = departments;

        // this.appointmentSubTypes = appointmentSubTypes;
        // this.reasons = reasons;

        this.appointmentFormArray = new FormArray([]);
        this.appointmentFormArray.push(
            new FormGroup({
                appt_type_id: new FormControl(),
                department: new FormControl(),
                appt_sub_type_id: new FormControl(),
                reason_code_id: new FormControl(),
                note_for_doctor: new FormControl()
            })
        );

        this.appointmentForm = new FormGroup({
            appointmentFormArray: this.appointmentFormArray
        });
    }

    getSelectedReasonText(reason_code_id: number): object {
        return this.reasons.filter((rs: object) => rs['reason_code_id'] === reason_code_id)[0];
    }

    addNewAppointment() {
        this.appointmentFormArray.push(
            new FormGroup({
                appt_type_id: new FormControl(),
                department: new FormControl(),
                appt_sub_type_id: new FormControl(),
                reason_code_id: new FormControl(),
                notes: new FormControl()
            })
        );
        this.noOfAppointments++;
        this.showConf = false;
    }
    getControls() {
        return (this.appointmentForm.get('appointmentFormArray') as FormArray).controls;
    }

    submitAppointmentForm() {
        const newAppointments = this.appointmentForm.value.appointmentFormArray.map(appointment => {
            return {
                ...appointment,
                client_id: this.patient.client_id,
                patient_id: this.patient.patient_id,
                appt_start_time: new Date(),
                appt_end_time: new Date((new Date()).getTime() + 30 * 60000),
                reason_code_id: appointment.reason_code_id[0],
                appt_sub_type_id: parseInt(appointment.appt_sub_type_id),
                department: parseInt(appointment.department),
                doctor_id: 3
            };
        });
        this._store.dispatch(new StoreNewAppointment(newAppointments));
        this.$newAppointments = newAppointments;
        this.showConf = true;

    }
}

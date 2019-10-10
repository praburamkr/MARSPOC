import { Component, OnInit, Output, EventEmitter, ViewChild, Input } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { SearchAppointment } from 'src/app/core/store/actions/appointment/appointment.actions';
import { IAppState } from 'src/app/core/store/state/app.state';
import { CheckBoxSelectionService } from '@syncfusion/ej2-angular-dropdowns';
import { getDoctorData } from 'src/app/core/store/selector/doctor/doctor.selector';
import { GetDoctor } from 'src/app/core/store/actions/doctor/doctor.actions';
import { getPreferredTimeslotData } from 'src/app/core/store/selector/appointment/appointment.selector';
import { GetPreferredTimeslot } from 'src/app/core/store/actions/appointment/appointment.actions';
import { NgForm, FormGroup } from '@angular/forms';
import { AddAppointment } from 'src/app/core/store/actions/appointment/appointment.actions';
import { getClientData, getClientDataByName } from 'src/app/core/store/selector/client/client.selector';
import { GetClients, GetClientPatient, GetClientByName } from 'src/app/core/store/actions/client/client.actions';
import { AddMisc } from 'src/app/core/store/actions/misc/misc.actions';
import { selectMiscData } from 'src/app/core/store/selector/misc/misc.selector';
import { AddClientPatient } from 'src/app/core/store/actions/client/client.actions';
import { addPatientData, searchClientData } from 'src/app/core/store/selector/client/client.selector';
import { isNullOrUndefined } from '@syncfusion/ej2-base';
import { AppointmentService } from '../appointment.service';
import { ClientService } from '../../shared/client/client.service';
import { IClient } from 'src/app/shared/models/client.interface';
import { Data } from '@syncfusion/ej2-schedule/src/schedule/actions/data';

@Component({
    selector: 'app-filter',
    templateUrl: './filter.component.html',
    styleUrls: ['./filter.component.scss'],
    providers: [CheckBoxSelectionService]
})
export class FilterComponent implements OnInit {

    getDoctors$ = this._store.pipe(select(getDoctorData));
    getClients$ = this._store.pipe(select(getClientData));
    getAllClientsByName$ = this._store.pipe(select(getClientDataByName));
    $misc: any = this._store.pipe(select(selectMiscData));
    getPreferredTimeslot$ = this._store.pipe(select(getPreferredTimeslotData));

    @Output() onSelectDoctor = new EventEmitter();
    @Output() onCalendarDateChange = new EventEmitter();
    @Output() onDateTypeValueChange = new EventEmitter();
    @Output() onApptTypeValueChange = new EventEmitter();
    @Output() onClientValueChange = new EventEmitter();
    @Output() setClientId = new EventEmitter();
    @Output() refreshAppointments = new EventEmitter();

    @ViewChild('patientForm', { static: false }) patientForm: NgForm;
    // tslint:disable-next-line: no-output-on-prefix
    @Output() onAddPatient: EventEmitter<any> = new EventEmitter<any>();
    @Output() onClose: EventEmitter<any> = new EventEmitter<any>();


    addClientPatient$ = this._store.pipe(select(addPatientData));
    getClientPatient$ = this._store.pipe(select(searchClientData));

    @Input() selectedCurrentCalendarDate: Date;
    @Input() keyedInData: string;
    isBookAppointmentClicked = false;
    isConfirmAppointmentClicked = false;
    isPatientClicked = false;
    isConfirmed = false;
    isAddPatient = false;
    appointmentTypes = new Array();
    clientList = new Array();
    appointmentForm: FormGroup;
    isNew = true;
    selectedReason: any;
    isAssessmentType = false;
    isDepartment = false;
    isAssessmentSubType = false;
    isReason = false;
    isTimeslot = false;
    otherPatients = [];

    appointmentTypeList = [
        { id: 4, name: 'Wellness', color: '#56A7F8', imageUrl: 'https://marspocstorage.blob.core.windows.net/marspoc/AppointmentsType/heart.png' }
        , { id: 5, name: 'Sick', color: '#F08B53', imageUrl: 'https://marspocstorage.blob.core.windows.net/marspoc/AppointmentsType/sick.png' }
        , { id: 6, name: 'Fluids', color: '#C59673', imageUrl: 'https://marspocstorage.blob.core.windows.net/marspoc/AppointmentsType/fluids.png' }
        , { id: 7, name: 'Surgery', color: '#8097A2', imageUrl: 'https://marspocstorage.blob.core.windows.net/marspoc/AppointmentsType/surgery.png' }
        , { id: 8, name: 'Recheck', color: '#A86CF3', imageUrl: 'https://marspocstorage.blob.core.windows.net/marspoc/AppointmentsType/reCheck.png' }
        , { id: 9, name: 'Semi annual wellness', color: '#56A7F8', imageUrl: 'https://marspocstorage.blob.core.windows.net/marspoc/AppointmentsType/heart-weak.png' }
    ]

    constructor(private _store: Store<IAppState>, private appointmentService: AppointmentService
        , private clientService: ClientService) { }

    public dateType: string[] = ['Day', 'Week', 'Month'];
    public medical: string[] = ['Wellness', 'Regular', 'Surgery'];
    public doctors: string[] = ['Dr. Janette Smith', 'Dr. Robert Smith', 'Dr. Maria Rodrigeez', 'Dr. David Allen', 'Dr. Grace Garcia'];
    public data: string[] = [
        'Brad, Mike',
        'Hamilton, Rita',
        'Dezalo, Kenny',
        'Callaway, Melissa',
        'Denison, Natasha'
    ];
    public date: Object = new Date();
    public format: string = 'dd MMM. yyyy';

    // maps the appropriate column to fields property
    public fields: Object = { text: 'Game', value: 'Id' };
    // set the placeholder to MultiSelect input element
    public waterMark: string = 'Doctors (' + this.data.length + ')';
    // set the type of mode for how to visualized the selected items in input element.
    public default: string = 'Default';
    public box: string = 'Box';
    public delimiter: string = 'Delimiter';
    public docName: string = "Click to select a doctor,";
    public slotName: string = "duration, date and time.";
    public slotString: string = "";
    public revVar: boolean = false;

    clientId: number;
    dropdownList = new Array();
    clientdropdownList = new Array();
    selectedItems = [];
    dropdownSettings = {};
    selectedDoctors = new Array();
    public mode: string;
    public doctorFields: Object = { text: 'item_text', value: 'item_id' };
    public apptTypeFields: Object = { text: 'item_text', value: 'item_id' };
    public clientFields: Object = { text: 'item_text', value: 'item_id' };
    isNewSlider = false;
    clients: Array<IClient>;
    dataEntered = '';
    client: IClient;
    header = 'New Appointment';
    selectedPatient: any;
    confirmAppointmentRequestData: any;

    ngOnInit() {
        this.isBookAppointmentClicked = false;
        this.isConfirmAppointmentClicked = false;
        this.isConfirmed = false;
        this.isPatientClicked = false;

        this.selectedItems = [
            { item_id: 1, item_text: 'Dr. Janette Smith' },
            { item_id: 2, item_text: 'Dr. Robert Smith' }
        ];

        this.dropdownSettings = {
            singleSelection: false,
            idField: 'item_id',
            textField: 'item_text',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1
        };

        this.mode = 'CheckBox';
        this.refreshDoctors();
        this.refreshAppointmentTypes();
        this.refreshClientList();
    }

    getAppointmentType(appointmentTypeId) {
        // tslint:disable-next-line: prefer-for-of
        for (let i = 0; i < this.appointmentTypeList.length; i++) {
            const appointmentType = this.appointmentTypeList[i];

            if (appointmentType.id === appointmentTypeId) {
                return appointmentType;
            }
        }
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
        return slotDate.toDateString().substr(0, 3) + ', ' +
            slotDate.toDateString().substr(4, 3) + ' ' + slotDate.getDate() +
            ", " + new Date().getFullYear() + " | " + this.formatAMPM(slotStartDate) +
            " - " + this.formatAMPM(new Date((slotEndDate.getTime())));
    }

    appointmentSuggestedTimeSlot: any;
    appointmentSuggestedFormattedTimeSlot: any;

    suggestTimeSlot() {
        this.isTimeslot = false;
        const body = {
            appt_type_id: 0,
            appt_sub_type_id: 1,
            patient_id: this.selectedPatient.patient_id,
            client_id: 0,
            appt_date: this.formatDate(new Date()),
            default_slot: true//,
            //client_req_time: Date.now()
        };

        this._store.dispatch(new GetPreferredTimeslot(body));
        this.getPreferredTimeslot$.subscribe((timeSlotDetails: any) => {

            if (this.getPreferredTimeslot$['actionsObserver']['value']['type'] == "[Appointment] Get Preferred Timeslot Success") {
                this.revVar = true;

                const data = timeSlotDetails.appointment.data[0];

                this.appointmentSuggestedFormattedTimeSlot = { duration: data["Duration"], isData: true, name: data.Name, date: this.populateTime(new Date(data["StartTime"]), new Date(data["StartTime"]), new Date(data["EndTime"])) };
                this.appointmentSuggestedTimeSlot = timeSlotDetails.appointment.data[0];

                if (data.Name.trim().length > 0) {
                    this.isTimeslot = true;
                }
                this.selectTimeslot();
                this.enableBookAppointment();
            }
        });

    }

    onDepartmentChange(department) {
        this.isDepartment = false;
        if (department.value !== undefined && department.value.trim().length > 0) {
            this.isDepartment = true;
        }

        this.enableBookAppointment();
        this.selectTimeslot();
    }

    onAppointmentSubTypeChange(appointmentType) {
        this.isAssessmentSubType = false;

        if (appointmentType.value !== undefined && appointmentType.value.trim().length > 0) {
            const selectedAppointmentId = appointmentType.value;
            const selectedAppointmentType = this.getAppointmentType(parseInt(selectedAppointmentId));

            // tslint:disable-next-line: prefer-const
            let appointmentTypeDisplay = document.getElementsByClassName('selected-appointment-type-display') as HTMLCollectionOf<HTMLElement>;

            if (appointmentTypeDisplay.length > 0) {
                appointmentTypeDisplay[0].style.background = selectedAppointmentType.color;
                appointmentTypeDisplay[0].style.backgroundImage = 'url(' + selectedAppointmentType.imageUrl + ')';
                appointmentTypeDisplay[0].style.backgroundPosition = 'center';
                appointmentTypeDisplay[0].style.backgroundRepeat = 'no-repeat';
            }

            // dataEntered: { appointmentType: false, department: false, appointmentSubType: false, reasons: false, timeslot: false };
            if (selectedAppointmentType.color.trim().length > 0) {
                this.isAssessmentSubType = true;
            }
        }

        this.enableBookAppointment();
        this.suggestTimeSlot();
    }

    resetPatientData(name, species, color, breed, age, weight) {
        name.value = '';
        color.value = '';
        age.value = '';
        weight.value = '';
        species.value = '';
        breed.value = '';

        const gender = (document.getElementsByName('gender') as NodeListOf<HTMLInputElement>);

        // tslint:disable-next-line: prefer-for-of
        for (let i = 0; i < gender.length; i++) {
            gender[i].checked = false;
        }
    }

    addPatient(name, species, color, breed, age, weight) {

        const gender = (document.getElementsByName('gender') as NodeListOf<HTMLInputElement>);
        const genderData = {};

        // tslint:disable-next-line: prefer-for-of
        for (let i = 0; i < gender.length; i++) {
            if (gender[i] !== undefined && gender[i].checked) {
                genderData['id'] = gender[i].id;
                genderData['name'] = gender[i].value;
            }
        }

        let newPatient: any;

        newPatient = {
            patient_name: name.value,
            color_pattern: color.value,
            age: age.value,
            weight: weight.value,
            species_name: species.value,
            breed: breed.value,
            sex: genderData['name'],
            photo: 'assets/images/default.png'
        };

        newPatient.client_id = this.client['client_id'];
        this._store.dispatch(new AddClientPatient(newPatient));

        this.addClientPatient$.subscribe((event: any) => {
            if (!isNullOrUndefined(event.data.patient_id)) {
                this.refreshPatients();
                this.resetPatientData(name, species, color, breed, age, weight);
                this.isAddPatient = false;
            }
        });

    }

    // tslint:disable-next-line: member-ordering
    clientPatients = new Array();

    refreshPatients() {
        this._store.dispatch(new GetClientPatient(this.clientId));
        this.getClientPatient$.subscribe((clientPatientDetails: any) => {
            if (clientPatientDetails != null && clientPatientDetails.data != null) {

                this.clientPatients = new Array();
                const data = clientPatientDetails.data;

                if (data != null && data.client_name != null) {

                    this.client['patients'] = new Array();
                    this.client['patients'] = data.patients;
                }
            }
        });
    }

    showNewPatient() {
        this.isAddPatient = true;
    }

    closeNewPatient(name, species, color, breed, age, weight) {
        this.resetPatientData(name, species, color, breed, age, weight);
        this.isAddPatient = false;
    }

    onBackClick() {
        const containerSliderRightContainer = document.getElementsByClassName('container-slider-right') as HTMLCollectionOf<HTMLElement>;
        containerSliderRightContainer[0].scrollTop = 0;

        this.isPatientClicked = true;
        this.isBookAppointmentClicked = false;
        this.isConfirmAppointmentClicked = false;
        this.revVar = false;
        this.appointmentSuggestedTimeSlot = { StartTime: '', EndTime: '', ResourceId: 0 };
        this.appointmentSuggestedFormattedTimeSlot = {};
    }

    selectTimeslot() {
        const timeslotContainer = document.getElementsByClassName('timeslot-container') as HTMLCollectionOf<HTMLInputElement>;
        const appointmentSubType = (document.getElementById('appointmentSubType') as HTMLInputElement);

        if (appointmentSubType !== undefined) {
            // tslint:disable-next-line: radix
            const selectedAppointmentType = this.getAppointmentType(parseInt(appointmentSubType.value));

            if (timeslotContainer.length > 0) {
                timeslotContainer[0].style.background = selectedAppointmentType.color;
                timeslotContainer[0].style.color = '#FFFFFF';
            }

        }
    }

    onAssessmentTypeChange() {
        const appointmentTypes = (document.getElementsByName('appointmentType') as NodeListOf<HTMLInputElement>);

        // tslint:disable-next-line: prefer-for-of
        for (let i = 0; i < appointmentTypes.length; i++) {
            if (appointmentTypes[i] !== undefined && appointmentTypes[i].checked) {
                this.isAssessmentType = true;
                this.enableBookAppointment();
            }
        }
    }

    onAddAppointmentClick() {
        const containerSliderRightContainer = document.getElementsByClassName('container-slider-right') as HTMLCollectionOf<HTMLElement>;
        containerSliderRightContainer[0].scrollTop = 0;

        this.isBookAppointmentClicked = false;
        this.isConfirmAppointmentClicked = true;

        const appointmentTypes = (document.getElementsByName('appointmentType') as NodeListOf<HTMLInputElement>);
        const department = (document.getElementById('department') as HTMLInputElement);
        var appointmentSubType = (document.getElementById('appointmentSubType') as HTMLInputElement);
        var notes = (document.getElementById('notes') as HTMLInputElement);

        let newAppointmentDataRequest: any;
        const appointmentType = {};

        for (let i = 0; i < appointmentTypes.length; i++) {
            if (appointmentTypes[i] !== undefined && appointmentTypes[i].checked) {
                appointmentType['id'] = appointmentTypes[i].id;
                appointmentType['name'] = appointmentTypes[i].value;
            }
        }

        const selectedAppointmentType = this.getAppointmentType(parseInt(appointmentSubType.value));

        newAppointmentDataRequest = {
            appointmentTypeId: appointmentType['id'],
            appointmentTypeValue: appointmentType['name'],

            departmentId: department.value,
            departmentValue: department.value,

            appointmentSubTypeId: selectedAppointmentType.id,
            appointmentSubTypeValue: selectedAppointmentType.name,
            appointmentSubTypeColor: selectedAppointmentType.color,
            appointmentSubTypeImageUrl: selectedAppointmentType.imageUrl,

            reasonsId: this.selectedReason.id,
            reasonsValue: this.selectedReason.name,

            notes: notes.value,
        };

        this.confirmAppointmentRequestData = newAppointmentDataRequest;
    }

    searchClients() {
        const clientRequest = {
            client_name: this.values
        };

        this._store.dispatch(new GetClientByName(clientRequest));
        this.getAllClientsByName$.subscribe((clientDetails: any) => {

            if (clientDetails != null && clientDetails.data != null) {
                const data = clientDetails.data;

                this.clients = new Array();

                // tslint:disable-next-line: prefer-for-of
                for (let i = 0; i < data.length; i++) {
                    const client: Object = new Object();

                    client['client_id'] = data[i]['client_id'];
                    client['client_name'] = data[i]['client_name'];
                    client['email'] = data[i]['email'];
                    client['img_url'] = data[i]['img_url'];
                    client['patients'] = data[i]['patients'];
                    client['phone'] = data[i]['phone'];
                    client['address'] = data[i]['address'];

                    this.clients.push(client);
                }

            }
        });
    }

    calculateClasses() {
        return this.isNew ? 'new-appointment-header bg-blue' : 'new-appointment-header bg-green';
    }

    onClientRowClick(client, isNew) {
        if (isNew === false) { this.isNewSlider = true; }
        this.isNew = isNew;

        var menuHeaderClass = document.getElementsByClassName("menuHeader") as HTMLCollectionOf<HTMLElement>;
        var dayScrollerClass = document.getElementsByClassName("day-scroller") as HTMLCollectionOf<HTMLElement>;

        menuHeaderClass[0].style.width = 'calc(100% - 400px)';
        menuHeaderClass[0].style.padding = '10px 0 0 0';
        dayScrollerClass[0].style.width = 'calc(100% - 400px)';

        this.clientId = client.client_id;
        this.client = client;
        this.header = client.client_name;
        this.isPatientClicked = true;
    }

    values = '';
    onKey(value: string) {
        this.values = value;

        if (this.values.length > 2) {
            this.searchClients();
        }
        else {
            this.clients = [];
        }
    }

    onNewAppointmentClick() {
        const menuHeaderClass = document.getElementsByClassName('menuHeader') as HTMLCollectionOf<HTMLElement>;
        const dayScrollerClass = document.getElementsByClassName('day-scroller') as HTMLCollectionOf<HTMLElement>;

        this.isNew = true;

        menuHeaderClass[0].style.width = 'calc(100% - 400px)';
        dayScrollerClass[0].style.width = 'calc(100% - 400px)';
        menuHeaderClass[0].style.padding = '10px 0 0 0';

        this.isNewSlider = true;
    }

    minimizeSlider() {
        const containerSliderRightContainer = document.getElementsByClassName('container-slider-right') as HTMLCollectionOf<HTMLElement>;
        const headerRightContainer = document.getElementsByClassName('new-appointment-header') as HTMLCollectionOf<HTMLElement>;
        const btnMaximizeContainer = document.getElementsByClassName('btn-maximize') as HTMLCollectionOf<HTMLElement>;
        const btnMinimizeContainer = document.getElementsByClassName('btn-minimize') as HTMLCollectionOf<HTMLElement>;
        const btnCloseContainer = document.getElementsByClassName('btn-close') as HTMLCollectionOf<HTMLElement>;

        const appointmentBookingContainer = document.getElementsByClassName('appointment-booking-container') as HTMLCollectionOf<HTMLElement>;
        const bookAppointmentContainer = document.getElementsByClassName('book-appointment-button-container') as HTMLCollectionOf<HTMLElement>;
        const confirmAppointmentContainer = document.getElementsByClassName('confirm-appointment-button-container') as HTMLCollectionOf<HTMLElement>;

        if (bookAppointmentContainer !== undefined && bookAppointmentContainer.length > 0)
            bookAppointmentContainer[0].style.display = 'none';

        if (confirmAppointmentContainer !== undefined && confirmAppointmentContainer.length > 0)
            confirmAppointmentContainer[0].style.display = 'none';

        if (appointmentBookingContainer !== undefined && appointmentBookingContainer.length > 0)
            appointmentBookingContainer[0].style.display = 'none';

        btnMinimizeContainer[0].style.display = 'none';
        btnMaximizeContainer[0].style.display = 'block';

        containerSliderRightContainer[0].style.height = '64px';
        containerSliderRightContainer[0].style.overflow = 'hidden';
        containerSliderRightContainer[0].style.position = 'absolute';
        containerSliderRightContainer[0].style.borderLeft = '0';
        containerSliderRightContainer[0].style.bottom = '10px';
        containerSliderRightContainer[0].style.top = '0';

        headerRightContainer[0].style.height = '55px';
        headerRightContainer[0].style.overflow = 'hidden';
        headerRightContainer[0].style.position = 'absolute';
        headerRightContainer[0].style.bottom = '0';
        headerRightContainer[0].style.top = '0';

        btnMinimizeContainer[0].style.bottom = 'auto';
        btnMinimizeContainer[0].style.top = '3px';

        btnMaximizeContainer[0].style.bottom = 'auto';
        btnMaximizeContainer[0].style.top = '3px';

        btnCloseContainer[0].style.bottom = 'auto';
        btnCloseContainer[0].style.top = '3px';

        const menuHeaderClass = document.getElementsByClassName('menuHeader') as HTMLCollectionOf<HTMLElement>;
        const dayScrollerClass = document.getElementsByClassName('day-scroller') as HTMLCollectionOf<HTMLElement>;

        dayScrollerClass[0].style.width = '100%';
        menuHeaderClass[0].style.width = 'calc(100% - 400px)';
        menuHeaderClass[0].style.padding = '10px 0 0 0';
    }

    maximizeSlider() {
        const containerSliderRightContainer = document.getElementsByClassName('container-slider-right') as HTMLCollectionOf<HTMLElement>;
        const headerRightContainer = document.getElementsByClassName('new-appointment-header') as HTMLCollectionOf<HTMLElement>;
        const btnMaximizeContainer = document.getElementsByClassName('btn-maximize') as HTMLCollectionOf<HTMLElement>;
        const btnMinimizeContainer = document.getElementsByClassName('btn-minimize') as HTMLCollectionOf<HTMLElement>;
        const btnCloseContainer = document.getElementsByClassName('btn-close') as HTMLCollectionOf<HTMLElement>;

        const appointmentBookingContainer = document.getElementsByClassName('appointment-booking-container') as HTMLCollectionOf<HTMLElement>;
        const bookAppointmentContainer = document.getElementsByClassName('book-appointment-button-container') as HTMLCollectionOf<HTMLElement>;
        const confirmAppointmentContainer = document.getElementsByClassName('confirm-appointment-button-container') as HTMLCollectionOf<HTMLElement>;

        if (bookAppointmentContainer !== undefined && bookAppointmentContainer.length > 0)
            bookAppointmentContainer[0].style.display = 'block';

        if (confirmAppointmentContainer !== undefined && confirmAppointmentContainer.length > 0)
            confirmAppointmentContainer[0].style.display = 'block';

        if (appointmentBookingContainer !== undefined && appointmentBookingContainer.length > 0)
            appointmentBookingContainer[0].style.display = 'block';

        btnMinimizeContainer[0].style.display = 'block';
        btnMaximizeContainer[0].style.display = 'none';

        containerSliderRightContainer[0].style.height = '100vh';
        containerSliderRightContainer[0].style.overflow = 'auto';
        containerSliderRightContainer[0].style.position = 'absolute';
        containerSliderRightContainer[0].style.bottom = 'auto';
        containerSliderRightContainer[0].style.top = '0';
        containerSliderRightContainer[0].style.borderLeft = '1px solid #ccc';

        headerRightContainer[0].style.height = '55px';
        headerRightContainer[0].style.overflow = 'hidden';
        headerRightContainer[0].style.position = 'fixed';
        headerRightContainer[0].style.bottom = 'auto';
        headerRightContainer[0].style.top = '0';

        btnMinimizeContainer[0].style.bottom = 'auto';
        btnMinimizeContainer[0].style.top = '3px';

        btnMaximizeContainer[0].style.bottom = 'auto';
        btnMaximizeContainer[0].style.top = '3px';

        btnCloseContainer[0].style.bottom = 'auto';
        btnCloseContainer[0].style.top = '3px';

        var menuHeaderClass = document.getElementsByClassName("menuHeader") as HTMLCollectionOf<HTMLElement>;
        var dayScrollerClass = document.getElementsByClassName("day-scroller") as HTMLCollectionOf<HTMLElement>;

        menuHeaderClass[0].style.width = 'calc(100% - 400px)';
        menuHeaderClass[0].style.padding = '10px 0 0 0';
        dayScrollerClass[0].style.width = 'calc(100% - 400px)';
    }

    closeSlider() {
        var menuHeaderClass = document.getElementsByClassName("menuHeader") as HTMLCollectionOf<HTMLElement>;
        var dayScrollerClass = document.getElementsByClassName("day-scroller") as HTMLCollectionOf<HTMLElement>;

        dayScrollerClass[0].style.width = '100%';
        menuHeaderClass[0].style.width = 'calc(100%)';
        menuHeaderClass[0].style.padding = '10px 120px 0 0';

        this.isNewSlider = false;
        this.isBookAppointmentClicked = false;
        this.isConfirmAppointmentClicked = false;
        this.isConfirmed = false;
        this.clients = new Array();
        this.clientId = 0;
        this.client = {};
        this.header = 'New Appointment';
        this.isPatientClicked = false;
        this.isAddPatient = false;
        this.revVar = false;

        this.appointmentSuggestedTimeSlot = { StartTime: '', EndTime: '', ResourceId: 0 };
        this.appointmentSuggestedFormattedTimeSlot = {};
    }

    refreshDoctors() {
        const doctorRequest = {
            resource_type: 1
        };

        this._store.dispatch(new GetDoctor(doctorRequest));
        this.getDoctors$.subscribe((doctorDetails: any) => {
            if (doctorDetails != null && doctorDetails.doctor != null && doctorDetails.doctor.data != null) {
                const data = doctorDetails.doctor.data;
                this.dropdownList = new Array();
                for (let i = 0; i < data.length; i++) {
                    const obj: Object = new Object();
                    obj['item_id'] = data[i]['resource_id'];
                    obj['item_text'] = data[i]['resource_name'];
                    obj['img_url'] = data[i]['img_url'];
                    obj['resource_type'] = data[i]['resource_type'];
                    this.dropdownList.push(obj);
                }
            }
        });
    }

    refreshClients() {
        const clientRequest = {
            client_name: ''
        };

        this._store.dispatch(new GetClients(clientRequest));
        this.getClients$.subscribe((clientDetails: any) => {

            if (clientDetails != null && clientDetails.data != null) {
                const data = clientDetails.data;
                this.clientdropdownList = new Array();
                for (let i = 0; i < data.length; i++) {
                    this.clientdropdownList.push(data[i].name);
                }
            }
        });
    }

    onItemSelect(item: any) {
        const args = {
            doctorId: item.item_id,
            checked: true
        };

        this.onSelectDoctor.emit(args);
    }

    onDateChange($event: any) {
        const selectedCalenderDate = $event.value;
        this.onCalendarDateChange.emit(selectedCalenderDate);
    }

    onDateTypeChange($event: any) {
        const dateType = $event.value;
        this.onDateTypeValueChange.emit(dateType);
    }

    onApptTypeChange($event: any) {
        const apptType = $event.value;
        this.onApptTypeValueChange.emit(apptType);
    }

    onClientChange($event: any) {
        const client = $event.value;
        this.onClientValueChange.emit(client);
    }

    getAppointments(value: string) {
        this.onSelectDoctor.emit(value);
    }

    onCloseNewAppointmentClick() {
        this.isNewSlider = false;
    }

    searchAppointments(): void {
        const body = {
            appt_link_id: 0,
            appt_type_id: 0,
            appt_sub_type_id: 0,
            patient_id: 0,
            client_id: 0,
            appt_start_time: '2019/09/20 09:00',
            appt_end_time: '2019/09/20 10:00',
            doctor_ids: [1, 5, 8]
        };

        this._store.dispatch(new SearchAppointment(body));
    }

    onSelect(args: any): void {
        let exists: boolean = false;

        for (let i = 0; i < this.selectedDoctors.length; i++) {
            if (this.selectedDoctors[i]['item_id'] == args['itemData']['item_id']) {
                exists = true;
            }
        }

        if (!exists) {
            this.selectedDoctors.push(args['itemData']);
        }

        const doctorIds = this.updateDoctorList();
        this.onSelectDoctor.emit(doctorIds);
    }

    onRemoving(args: any): void {
        this.selectedDoctors = this.selectedDoctors.filter(
            doctor => doctor['item_id'] != args['itemData']['item_id']);
        const doctorIds = this.updateDoctorList();
        this.onSelectDoctor.emit(doctorIds);
    }

    updateDoctorList(): Array<any> {
        const args = new Array();

        for (let i = 0; i < this.selectedDoctors.length; i++) {
            const doc = this.selectedDoctors[i];

            const arg = {
                doctorId: doc['item_id'],
                doctorName: doc['item_text'],
                img_url: doc['img_url'],
                resource_type: doc['resource_type'],
                checked: true
            };

            args.push(arg);
        }
        return args;
    }

    refreshAppointmentTypes() {
        this.appointmentService.getAppointmentTypes().subscribe(
            // tslint:disable-next-line: no-string-literal
            (appointmentTypes: object) => {
                this.appointmentTypes = new Array();
                const apptTypes = appointmentTypes['data'].filter(at => at.parent_type_id === 0);
                for (let i = 0; i < apptTypes.length; i++) {
                    const obj = new Object();
                    obj['item_id'] = apptTypes[i]['appt_type_id'];
                    obj['item_text'] = apptTypes[i]['appt_type_text'];
                    this.appointmentTypes.push(obj);
                }
            });
    }

    refreshClientList() {
        this.clientService.getAllClients().subscribe(
            (clients: object) => {
                this.clientList = new Array();
                const allClients = clients['data'];
                for (let i = 0; i < allClients.length; i++) {
                    const obj = new Object();
                    obj['item_id'] = allClients[i]['id'];
                    obj['item_text'] = allClients[i]['name'];
                    this.clientList.push(obj);
                }
            });
    }

    onConfirmAppointmentClick() {
        const menuHeaderClass = document.getElementsByClassName("menuHeader") as HTMLCollectionOf<HTMLElement>;
        const dayScrollerClass = document.getElementsByClassName("day-scroller") as HTMLCollectionOf<HTMLElement>;

        dayScrollerClass[0].style.width = '100%';
        menuHeaderClass[0].style.width = 'calc(100%)';
        menuHeaderClass[0].style.padding = '10px 120px 0 0';

        this.isNewSlider = false;
        this.isBookAppointmentClicked = true;
        this.isConfirmAppointmentClicked = true;
        this.isConfirmed = true;

        const isEmail = (document.getElementById('isEmail') as HTMLInputElement);
        const email = (document.getElementById('email') as HTMLInputElement);

        const appointmentRequest = {
            client_id: this.selectedPatient.client_id,
            patient_id: this.selectedPatient.patient_id,
            appt_start_time: this.appointmentSuggestedTimeSlot.StartTime,
            appt_end_time: this.appointmentSuggestedTimeSlot.EndTime,
            reason_code_id: this.confirmAppointmentRequestData.reasonsId,
            appt_sub_type_id: this.confirmAppointmentRequestData.appointmentSubTypeId,
            department_id: this.confirmAppointmentRequestData.departmentId,
            doctor_id: this.appointmentSuggestedTimeSlot.ResourceId,
            appt_type_id: this.confirmAppointmentRequestData.appointmentTypeId,
            is_email: isEmail.checked,
            email: email.value,
            note_for_doctor: this.confirmAppointmentRequestData.notes
        }

        const appointments = [];

        appointments.push(appointmentRequest);

        this._store.dispatch(new AddAppointment(appointments));
        this._store.dispatch(new AddMisc({ showConfirm: true, noOfAppointments: appointments.length }));
        // setTimeout(() => {

        //     this.isBookAppointmentClicked = false;
        //     this.isConfirmAppointmentClicked = false;
        //     this.isConfirmed = false;
        //     this.clients = new Array();
        //     this.clientId = 0;
        //     this.client = {};
        //     this.header = 'New Appointment';
        //     this.isPatientClicked = false;
        //     this.isAddPatient = false;
        //     this.revVar = false;

        //     this.appointmentSuggestedTimeSlot = { StartTime: '', EndTime: '', ResourceId: 0 };
        //     this.appointmentSuggestedFormattedTimeSlot = {};
        // }, 1500);
        // this.refreshAppointments.emit();
    }

    onConfirmationClose() {
        this.isBookAppointmentClicked = false;
        this.isConfirmAppointmentClicked = false;
        this.isConfirmed = false;
        this.clients = new Array();
        this.clientId = 0;
        this.client = {};
        this.header = 'New Appointment';
        this.isPatientClicked = false;
        this.isAddPatient = false;
        this.revVar = false;

        this.appointmentSuggestedTimeSlot = { StartTime: '', EndTime: '', ResourceId: 0 };
        this.appointmentSuggestedFormattedTimeSlot = {};

        this.refreshAppointments.emit();
    }

    onReasonsChange() {
        this.isReason = false;
        const selectedReasonValue = (document.getElementById('reasons') as HTMLInputElement).value;
        let selectedReasonText = '';

        const reasonCollection = [
            { id: 1, name: 'GI upset(Vomiting / Diarrhea)' }
            , { id: 2, name: 'Tooth extraction' }
            , { id: 3, name: 'Lethargic Weakness' }
            , { id: 4, name: 'Eyes Tear Staining' }
            , { id: 5, name: 'Eating Less' }
            , { id: 6, name: 'Abdominal Surgery' }
            , { id: 7, name: 'Fracture Repair' }
        ];

        // tslint:disable-next-line: no-conditional-assignment
        for (let i = 0; i < reasonCollection.length; i++) {
            // tslint:disable-next-line: prefer-const
            var reason = reasonCollection[i];

            // tslint:disable-next-line: radix
            if (parseInt(selectedReasonValue) === reason.id) {
                selectedReasonText = reason.name;
            }
        }

        this.selectedReason = { id: selectedReasonValue, name: selectedReasonText };

        const chip = (document.getElementById('chip') as HTMLElement);

        chip.style.padding = '3px 15px';
        chip.style.border = '1px solid #0078F0';
        chip.innerHTML = selectedReasonText;

        if (chip.innerHTML.trim().length > 0) {
            this.isReason = true;
        }
        this.enableBookAppointment();

        // tslint:disable-next-line: prefer-const
        let reasonControl = (document.getElementById('reasons') as HTMLSelectElement);

        // tslint:disable-next-line: prefer-for-of
        for (let i = 0; i < reasonControl.options.length; i++) {
            reasonControl.options[0].selected = true;
            return;
        }

    }

    addAppointment(patient, patients) {
        const containerSliderRightContainer = document.getElementsByClassName('container-slider-right') as HTMLCollectionOf<HTMLElement>;
        containerSliderRightContainer[0].scrollTop = 0;

        this.isPatientClicked = false;
        this.isBookAppointmentClicked = true;

        this.selectedPatient = patient;
        this.selectedReason = {};

        this.isAssessmentType = false;
        this.isDepartment = false;
        this.isAssessmentSubType = false;
        this.isReason = false;
        this.isTimeslot = false;

        this.otherPatients = [];

        // tslint:disable-next-line: prefer-for-of
        for (let i = 0; i < patients.length; i++) {
            let patientData = patients[i];

            if (patient.patient_id !== patientData.patient_id) {
                let patientDataObject = {
                    patient_id: patientData.patient_id,
                    patient_name: patientData.patient_name,
                    img_url: patientData.img_url,
                    age: patientData.age,
                    birth_date: patientData.birth_date,
                    breed: patientData.breed,
                    client_id: patientData.client_id,
                    color_pattern: patientData.color_pattern,
                    pref_doctor: patientData.pref_doctor,
                    sex: patientData.sex,
                    species_id: patientData.species_id,
                    species_name: patientData.species_name,
                    weight: patientData.weight,
                    weight_uom: patientData.weight_uom
                };

                this.otherPatients.push(patientDataObject);
            }
        }
    }

    enableBookAppointment() {
        const bookAppointment = (document.getElementById('bookAppointment') as HTMLButtonElement);

        if (this.isAssessmentType === true && this.isDepartment === true && this.isAssessmentSubType === true
            && this.isReason === true && this.isTimeslot === true) {
            bookAppointment.disabled = false;
            bookAppointment.innerHTML = "Book 1 Appointment";
        }
        else {
            bookAppointment.disabled = true;
            bookAppointment.innerHTML = "Book Appointment";
        }
    }
}

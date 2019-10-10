import { Component, ViewChild, ViewEncapsulation, Input, HostListener } from '@angular/core';
import { doctorData } from './data';
import { extend } from '@syncfusion/ej2-base';
import {
    EventSettingsModel, View, GroupModel, ResourceDetails, TreeViewArgs, PopupOpenEventArgs, ScheduleComponent, ActionEventArgs,
    EventFieldsMapping, RenderCellEventArgs, MonthService, WorkWeekService
} from '@syncfusion/ej2-angular-schedule';
import { GetClient, GetClientPatient } from 'src/app/core/store/actions/client/client.actions';
import { IAppState } from 'src/app/core/store/state/app.state';
import { Store, select } from '@ngrx/store';
import { getAppointmentData } from 'src/app/core/store/selector/appointment/appointment.selector';
import { GetAppointment } from 'src/app/core/store/actions/appointment/appointment.actions';
import { AddMisc } from 'src/app/core/store/actions/misc/misc.actions';
import { selectMiscData } from 'src/app/core/store/selector/misc/misc.selector';
import { GetDoctor } from 'src/app/core/store/actions/doctor/doctor.actions';
import { getDoctorData } from 'src/app/core/store/selector/doctor/doctor.selector';
import { element } from 'protractor';


import { HubConnection } from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { ApiService } from 'src/app/shared/services/api-service/api.service';
import { UtilsService } from 'src/app/shared/services/utils/utils.service';
import { searchClientData } from 'src/app/core/store/selector/client/client.selector';
import { FilterComponent } from '../filter/filter.component';



@Component({
    selector: 'app-calendar',
    templateUrl: './calendar.component.html',
    styleUrls: ['./calendar.component.scss'],
    encapsulation: ViewEncapsulation.None,
    providers: [MonthService, WorkWeekService]
})

export class CalendarComponent {
    isIncoming: boolean = true;
    $misc: any = this._store.pipe(select(selectMiscData));
    searchAppointments$ = this._store.pipe(select(getAppointmentData));
    isLoadSlider: boolean = false;
    isZoomed: boolean = false;
    public data: Object[] = <Object[]>extend([], doctorData, null, true);
    public selectedDate: Date = new Date();
    dateType: string;
    apptType: string;
    public currentView: View = 'Day';
    public allowResizing: boolean = false;
    public allowDragDrop: boolean = false;
    resourceDataSource = new Array();
    public group: GroupModel = { resources: ['Doctors'] };
    public eventSettings: EventSettingsModel; // = { dataSource: this.data };
    @ViewChild('filter', { static: true }) filter: FilterComponent;
    @ViewChild('scheduleObj', { static: true })
    public scheduleObj: ScheduleComponent;
    doctorsId: any = [];
    selectedCalendarDate: Date;
    selectedCurrentCalendarDate: Date;
    allDoctorResource = new Array();
    selectedDoctorIds = new Array();
    clientId: number;
    apptTypeId: number = 0;
    selectedClientId: number = 0;
    @Input() isCrtlShiftOn: boolean = false;
    displayPhonePlugin: boolean;
    private _hubConnection: HubConnection | undefined;
    getClientPatient$ = this._store.pipe(select(searchClientData));
    clientInfo: Object;

    constructor(private _store: Store<IAppState>) {
        this.displayPhonePlugin = false;
    }

    @HostListener('window:keydown', ['$event'])
    onKeydown($event) {
        if ($event.key === "Shift") {
            this.isCrtlShiftOn = true;
        }
    }

    flag: true;
    days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    getDoctors$ = this._store.pipe(select(getDoctorData));

    refreshDoctors() {
        const doctorRequest = {
            resource_type: 1
        };

        this._store.dispatch(new GetDoctor(doctorRequest));
        this.getDoctors$.subscribe((doctorDetails: any) => {
            if (this.getDoctors$['actionsObserver']['value']['type'] == "[Doctor] Get Success") {

                if (doctorDetails != null && doctorDetails.doctor != null && doctorDetails.doctor.data != null) {
                    const data = doctorDetails.doctor.data;
                    this.resourceDataSource = new Array();

                    for (let i = 0; i < data.length; i++) {

                        const resource: Object = new Object();

                        resource['id'] = data[i]['resource_id'];
                        resource['text'] = data[i]['resource_name'];
                        resource['image'] = data[i]['img_url'];
                        resource['resource_type'] = data[i]['resource_type'];
                        resource['startHour'] = '09:00';
                        resource['endHour'] = '17:00';

                        this.resourceDataSource.push(resource);
                        this.allDoctorResource.push(resource);
                    }

                }
            }
        });
    }

    onCalendarChange($event) {
        this.selectedCalendarDate = $event;
        this.selectedDate = this.selectedCalendarDate;
        this.clearTabs();
        this.buildDayRow(this.selectedCalendarDate);

        let selectedStartDateTime = new Date(this.selectedCalendarDate);

        this.formatSelectedDate = selectedStartDateTime.getFullYear() + '/' + (selectedStartDateTime.getMonth() + 1) + '/' + selectedStartDateTime.getDate() + ' 00:00:00';
        this.formatNextDate = selectedStartDateTime.getFullYear() + '/' + (selectedStartDateTime.getMonth() + 1) + '/' + selectedStartDateTime.getDate() + ' 23:59:59';

        this.getDateApppointments();
    }

    onClientClick($event) {
        this.clientId = $event;
    }

    getSundayOfThatWeek(date) {
        const target = new Date(date);
        const dayNr = (target.getDay() + 6) % 7;

        dayNr === 6 ? target : target.setDate(target.getDate() - dayNr - 1);

        return target;
    }

    clearTabs() {
        const sunday = document.getElementsByClassName('sun');
        const monday = document.getElementsByClassName('mon');
        const tuesday = document.getElementsByClassName('tue');
        const wednesday = document.getElementsByClassName('wed');
        const thursday = document.getElementsByClassName('thu');
        const friday = document.getElementsByClassName('fri');
        const saturday = document.getElementsByClassName('sat');

        sunday[0].classList.remove('current-day');
        monday[0].classList.remove('current-day');
        tuesday[0].classList.remove('current-day');
        wednesday[0].classList.remove('current-day');
        thursday[0].classList.remove('current-day');
        friday[0].classList.remove('current-day');
        saturday[0].classList.remove('current-day');
    }

    getDay(dayCounter: number, inputDate: Date, day: HTMLCollectionOf<Element>) {
        let target = this.getSundayOfThatWeek(inputDate);
        let first = target.getDate() - target.getDay() + dayCounter;
        let date = new Date(target.setDate(first)).toUTCString();
        let currentDate = new Date(date);
        const today = new Date(inputDate);

        day[0].classList.add("days");

        if (today.getDate() === currentDate.getDate()
            && today.getMonth() === currentDate.getMonth()
            && today.getFullYear() === currentDate.getFullYear()
        ) {
            day[0].classList.add("current-day");
        } else {
            day[0].classList.add("days");
        }

        day[0].innerHTML = '<div class="date-hidden">' + currentDate + '</div>' + currentDate.getDate() + ' ' + this.days[dayCounter];
    }

    buildDayRow(inputDate: Date) {
        this.refreshDoctors();
        this.populateDate(inputDate);
    }

    populateDate(inputDate: Date) {
        const sunday = document.getElementsByClassName('sun');
        const monday = document.getElementsByClassName('mon');
        const tuesday = document.getElementsByClassName('tue');
        const wednesday = document.getElementsByClassName('wed');
        const thursday = document.getElementsByClassName('thu');
        const friday = document.getElementsByClassName('fri');
        const saturday = document.getElementsByClassName('sat');

        this.getDay(0, inputDate, sunday);
        this.getDay(1, inputDate, monday);
        this.getDay(2, inputDate, tuesday);
        this.getDay(3, inputDate, wednesday);
        this.getDay(4, inputDate, thursday);
        this.getDay(5, inputDate, friday);
        this.getDay(6, inputDate, saturday);
    }

    handleClick(control: string) {
        const dayControl = document.getElementById(control);
        const dateText = dayControl.innerHTML.replace('<div class="date-hidden">', '');
        const dateTextTrim = dateText.replace('</div>', ',');
        const dates = dateTextTrim.split(',');
        const selectedDate = new Date(dates[0]);

        this.selectedDate = selectedDate;

        this.clearTabs();
        this.selectedCalendarDate = selectedDate;
        this.selectedCurrentCalendarDate = this.selectedCalendarDate;

        this.getAppointments();
    }

    populateDateRange() {
        let addDays = 1;
        if (this.dateType == "Week") {
            addDays = 7;
        } else if (this.dateType == "Month") {
            addDays = 30;
        }
        this.formatSelectedDate = this.formatDate(this.selectedCalendarDate);
        this.day = new Date(this.formatSelectedDate);
        this.nextDay = new Date(this.day);
        this.nextDate = this.nextDay.setDate(this.day.getDate() + addDays);
        this.formatNextDate = this.formatDate(this.nextDay);
    }

    formatDate(selectedDate) {
        let month = selectedDate.getMonth() + 1;
        if (month <= 9) {
            month = '0' + month;
        }
        let currentDate = selectedDate.getDate();
        if (currentDate <= 9) {
            currentDate = '0' + currentDate;
        }
        return selectedDate.getFullYear() + '/' + month + '/' + currentDate;
    };

    formatSelectedDate = this.formatDate(this.selectedDate);
    day = new Date(this.formatSelectedDate);
    nextDay = new Date(this.day);
    nextDate = this.nextDay.setDate(this.day.getDate() + 1);
    formatNextDate = this.formatDate(this.nextDay);

    ngOnInit() {
        const curr = new Date;
        this.selectedCalendarDate = curr;
        this.selectedCurrentCalendarDate = curr;
        this.buildDayRow(curr);

        // const request = {
        //     appt_link_id: 0,
        //     appt_type_id: 0,
        //     appt_sub_type_id: 0,
        //     patient_id: 0,
        //     client_id: 0,
        //     appt_start_time: this.formatSelectedDate,
        //     appt_end_time: this.formatNextDate,
        //     doctor_ids: []
        // }

        // const request = this.getRequestData();

        // this._store.dispatch(new GetAppointment(request));
        // this.searchAppointments$.subscribe((appointmentDetails: any) => {
        //     if (this.searchAppointments$['actionsObserver']['value']['type'] == "[Appointment] Get Success") {
        //         this.scheduleObj.eventSettings.dataSource = appointmentDetails.appointment;
        //     }
        // });

        this.getSystemTmezone();
        this.getAppointments();

        const connection = new signalR.HubConnectionBuilder()
            .configureLogging(signalR.LogLevel.Information)
            .withUrl("https://mars-petcare.eastus.cloudapp.azure.com/api/notifications/web")
            .build();
        connection.start().then(function () {
            console.log('Connected!');
        }).catch(function (err) {
            return console.error(err.toString());
        });
        connection.on("BroadcastMessage", (payload: string) => {
            var jsonPayload = JSON.parse(payload);
            var isForActiveUser = false;
            if (jsonPayload.UserID && localStorage.getItem('username').toString() == jsonPayload.UserID) {
                isForActiveUser = true;
            }
            //if its NEWAPPT then just referes the page
            if (jsonPayload.EventID == "NEW_APPT") {
                //this.displayPhonePlugin = true;
                this.getAppointments();
                //rendering calender component          
            }
            //if its CHECK_IN then just referes the page
            else if (jsonPayload.EventID == "CHECKIN") {
                //this.displayPhonePlugin = true;
                this.getAppointments();
                // location.reload();
            }
            //if Incoming call then get all the details and show the popup for phone call
            else if (jsonPayload.EventID == "INCOMING_CALL") {
                this.displayPhonePlugin = true;
                let clientId = jsonPayload.Data.client_id;
                this.displayIncomingCall(clientId);
            }
            else if (isForActiveUser) {
                alert(jsonPayload);
            }
        });
    }

    today = new Date();
    todayDate = this.today.getDay();
    currentDay = this.today.getDate() + ' ' + this.days[this.todayDate];
    previousDay = this.today.getDate() - 1 + ' ' + this.days[this.todayDate - 1];

    displayIncomingCall(clientId: number) {
        this._store.dispatch(new GetClientPatient(clientId));
        this.getClientPatient$.subscribe((clientPatientDetails: any) => {
            if (this.getDoctors$['actionsObserver']['value']['type'] == "[Client] Get Client Patient Success") {
                if (clientPatientDetails != null && clientPatientDetails.data != null) {
                    this.clientInfo = clientPatientDetails.data;
                }
            }
        });
    }

    getLastWeek(date) {
        return new Date(date.getFullYear(), date.getMonth(), date.getDate());
    }

    getNextWeek(date) {
        return new Date(date.getFullYear(), date.getMonth(), date.getDate());
    }

    onPreviousClick($event) {
        var currentDate = new Date(this.selectedCalendarDate);

        currentDate.setDate(currentDate.getDate() - 7);

        this.setDateFilter(currentDate);
        this.buildDayRow(this.getLastWeek(currentDate));
    }

    onNextClick($event) {
        var currentDate = new Date(this.selectedCalendarDate);

        currentDate.setDate(currentDate.getDate() + 7);

        this.setDateFilter(currentDate);
        this.buildDayRow(this.getNextWeek(currentDate));
    }

    setDateFilter(currentDate) {
        this.selectedCurrentCalendarDate = currentDate;
    }

    getRequestData() {

        if (this.selectedDoctorIds.length > 0) {
            return {
                appt_link_id: 0,
                appt_type_id: this.apptTypeId,
                appt_sub_type_id: 0,
                patient_id: 0,
                client_id: this.selectedClientId,
                appt_start_time: this.formatSelectedDate,
                appt_end_time: this.formatNextDate,
                doctor_ids: this.selectedDoctorIds
            }
        }
        else {
            let selectedStartDateTime = new Date(this.selectedCalendarDate);

            return {
                appt_link_id: 0,
                appt_type_id: this.apptTypeId,
                appt_sub_type_id: 0,
                patient_id: 0,
                client_id: this.selectedClientId,
                appt_start_time: selectedStartDateTime.getFullYear() + '/' + (selectedStartDateTime.getMonth() + 1) + '/' + selectedStartDateTime.getDate() + ' 00:00:00',
                appt_end_time: selectedStartDateTime.getFullYear() + '/' + (selectedStartDateTime.getMonth() + 1) + '/' + selectedStartDateTime.getDate() + ' 23:59:59'
            }
        }
    }

    getAppointments() {

        const request = this.getRequestData();

        this._store.dispatch(new GetAppointment(request));
        this.searchAppointments$.subscribe((appointmentDetails: any) => {

            if (this.searchAppointments$['actionsObserver']['value']['type'] == "[Appointment] Get Success") {
                let arr = appointmentDetails.appointment;
                let selectedDocs = new Array();

                if (this.selectedDoctorIds.length === 0) {

                    this.selectedDoctorIds = new Array();
                    // tslint:disable-next-line: prefer-for-of
                    for (let doctor = 0; doctor < this.allDoctorResource.length; doctor++) {
                        this.selectedDoctorIds.push(this.allDoctorResource[doctor].id);
                    }
                }

                for (let i = 0; i < arr.length; i++) {

                    if (this.selectedDoctorIds.indexOf(arr[i]["DoctorId"]) > -1) {
                        selectedDocs.push(arr[i]);
                    }

                }


                this.scheduleObj.eventSettings.dataSource = selectedDocs;
                this.scheduleObj.dataBind();
                this.scheduleObj.refresh();
            }
        });

    }

    getDoctorApppointments(data: any) {

        this.selectedDoctorIds = new Array();

        // tslint:disable-next-line: prefer-for-of
        for (let i = 0; i < data.length; i++) {
            this.selectedDoctorIds.push(data[i].doctorId);
        }

        this.resourceDataSource = new Array();
        let docIds = new Array();
        if (Array.isArray(data)) {
            this.doctorsId = data;
            for (let i = 0; i < this.doctorsId.length; i++) {
                let doc = this.doctorsId[i];
                docIds.push(doc['doctorId']);
                let docObj = new Object();
                docObj['text'] = doc['doctorName'];
                docObj['id'] = doc['doctorId'];
                docObj['workDays'] = '1, 2, 3, 4, 5';
                docObj['startHour'] = '08:00';
                docObj['endHour'] = '17:00';
                docObj['image'] = doc['img_url'];
                docObj['resource_type'] = doc['resource_type'];
                this.resourceDataSource.push(docObj);
            }
        }

        const request = this.getRequestData();

        this._store.dispatch(new GetAppointment(request));
        this.searchAppointments$.subscribe((appointmentDetails: any) => {
            if (this.searchAppointments$['actionsObserver']['value']['type'] == "[Appointment] Get Success") {
                let arr = appointmentDetails.appointment;
                let selectedDocs = new Array();
                for (let i = 0; i < arr.length; i++) {
                    if (docIds.indexOf(arr[i]["DoctorId"]) > -1) {
                        selectedDocs.push(arr[i]);
                    }
                }
                this.scheduleObj.eventSettings.dataSource = selectedDocs;
                this.scheduleObj.dataBind();
                this.scheduleObj.refresh();
            }
        });

    }

    closeSlider($event) {
        var menuHeaderClass = document.getElementsByClassName("menuHeader") as HTMLCollectionOf<HTMLElement>;
        var dayScrollerClass = document.getElementsByClassName("day-scroller") as HTMLCollectionOf<HTMLElement>;

        dayScrollerClass[0].style.width = '100%';
        menuHeaderClass[0].style.width = 'calc(100%)';
        menuHeaderClass[0].style.padding = '10px 120px 0 0';

        // this.isLoadSlider = false;
        this._store.dispatch(new AddMisc({ showSlider: false }));
    }

    getDoctorName(value: ResourceDetails | TreeViewArgs): string {
        return ((value as ResourceDetails).resourceData) ?
            (value as ResourceDetails).resourceData[(value as ResourceDetails).resource.textField] as string
            : (value as TreeViewArgs).resourceName;
    }

    getDoctorImage(value: ResourceDetails): string {
        let image: string = '';
        if (value != null && value.resourceData != null && value.resourceData.image != null) {
            image = value.resourceData.image.toString();
        }
        return image;
    }

    getDoctorLevel(value: ResourceDetails | TreeViewArgs): string {
        let resourceName: string = this.getDoctorName(value);
        return (resourceName === 'Will Smith') ? 'Cardiologist' : (resourceName === 'Alice') ? 'Neurologist' : 'Orthopedic Surgeon';
    }

    onActionBegin(args: ActionEventArgs): void {
        let isEventChange: boolean = (args.requestType === 'eventChange');
        if ((args.requestType === 'eventCreate' && (<Object[]>args.data).length > 0) || isEventChange) {
            let eventData: { [key: string]: Object } = (isEventChange) ? args.data as { [key: string]: Object } :
                args.data[0] as { [key: string]: Object };
            let eventField: EventFieldsMapping = this.scheduleObj.eventFields;

            let startDate: Date = eventData[eventField.startTime] as Date;
            let endDate: Date = eventData[eventField.endTime] as Date;
            let resourceIndex: number = [1, 2, 3].indexOf(eventData.DoctorId as number);
            args.cancel = !this.isValidateTime(startDate, endDate, resourceIndex);
            if (!args.cancel) {
                args.cancel = !this.scheduleObj.isSlotAvailable(startDate, endDate, resourceIndex);
            }
        }
    }

    isValidateTime(startDate: Date, endDate: Date, resIndex: number): boolean {
        let resource: ResourceDetails = this.scheduleObj.getResourcesByIndex(resIndex);
        let startHour: number = parseInt(resource.resourceData.startHour.toString().slice(0, 2), 10);
        let endHour: number = parseInt(resource.resourceData.endHour.toString().slice(0, 2), 10);
        return (startHour <= startDate.getHours() && endHour >= endDate.getHours());
    }

    onPopupOpen(args: PopupOpenEventArgs): void {
        if (args.target && args.target.classList.contains('e-work-cells')) {
            // args.target.innerHTML = '<div class="container-slider-right"><div class="client-details"><button class="btn-close" (click)="closeSlider($event)">X</button><h2>Client name</h2><p>client phone number</p><p> Client email Id</p></div></div>';
            args.cancel = !args.target.classList.contains('e-work-hours');
            this.isLoadSlider = true;
        }
    }

    dbClick(args: any): void {
        args.cancel = false;
        args.event.preventDefault();
    }

    onDataBound(e: { [key: string]: Object }): void {
        this.scheduleObj.scrollTo("8:00");
    }

    cellClick(args: any): void {
        var menuHeaderClass = document.getElementsByClassName("menuHeader") as HTMLCollectionOf<HTMLElement>;
        var dayScrollerClass = document.getElementsByClassName("day-scroller") as HTMLCollectionOf<HTMLElement>;

        menuHeaderClass[0].style.width = 'calc(100% - 400px)';
        dayScrollerClass[0].style.width = 'calc(100% - 400px)';
        menuHeaderClass[0].style.padding = '10px 0 0 0';

        args.cancel = false;
        args.event.preventDefault();
    }

    onRenderCell(args: RenderCellEventArgs): void {
        // if (args.element.classList.contains('e-work-hours') || args.element.classList.contains('e-work-cells')) {
        //     addClass([args.element], ['James', 'Robert', 'Rodrigeez', 'David', 'Maria'][parseInt(args.element.getAttribute('data-group-index'), 10)]);
        // }
    }


    openAppointmentSlider(clientInfo) {
        this.displayPhonePlugin = false;
        this.filter.onClientRowClick(clientInfo, false);

        // this.clientService.addClientPetInfo(newPatient);
        // this._store.dispatch(new GetClient());
        // this._store.dispatch(new AddMisc({ showSlider: true }));
        // this.$misc = this._store.pipe(select(selectMiscData));
        // this.isLoadSlider = true;
    }

    onZooomChange() {
        this.isZoomed = !this.isZoomed;
        if (this.isZoomed) {
            document.querySelector('.container-calendar').classList.add('hasNotes');
        }
        else {
            document.querySelector('.container-calendar').classList.remove('hasNotes');
        }
        this.scheduleObj.dataBind();
        this.scheduleObj.refresh();
    }

    scrollToNextDoctor() {
        const doctorContentWrapper = document.getElementsByClassName('e-content-wrap');
        doctorContentWrapper[0].scrollLeft += 220;
    }

    scrollToPreviousDoctor() {
        const doctorContentWrapper = document.getElementsByClassName('e-content-wrap');
        doctorContentWrapper[0].scrollLeft -= 220;
    }

    getDateApppointments() {
        let docIds = new Array();

        for (let i = 0; i < this.resourceDataSource.length; i++) {
            docIds.push(this.resourceDataSource[i]["id"]);
        }

        const request = {
            appt_link_id: 0,
            appt_type_id: this.apptTypeId,
            appt_sub_type_id: 0,
            patient_id: 0,
            client_id: this.selectedClientId,
            appt_start_time: this.formatSelectedDate,
            appt_end_time: this.formatNextDate,
            doctor_ids: docIds
        }
        this._store.dispatch(new GetAppointment(request));
        this.searchAppointments$.subscribe((appointmentDetails: any) => {
            //"[Appointment] Get Success"
            let arr = appointmentDetails.appointment;
            let selectedDocs = new Array();
            for (let i = 0; i < arr.length; i++) {
                if (docIds.indexOf(arr[i]["DoctorId"]) > -1) {
                    selectedDocs.push(arr[i]);
                }
            }
            this.scheduleObj.eventSettings.dataSource = selectedDocs;
        });
        this.scheduleObj.dataBind();
        this.scheduleObj.refresh();
    }

    onApptTypeChange($event: any) {
        this.apptTypeId = $event;
        this.getAppointments();
    }

    onClientChange($event: any) {
        this.selectedClientId = $event;
        this.getAppointments();
    }

    pad(value) {
        return value < 10 ? '0' + value : value;
    }

    getSystemTmezone() {
        // tslint:disable-next-line: one-variable-per-declaration
        let timezone_offset_min = new Date().getTimezoneOffset(),
            offset_hrs = Math.abs(timezone_offset_min / 60),
            offset_min = Math.abs(timezone_offset_min % 60),
            timezone_standard;

        // Add an opposite sign to the offset
        // If offset is 0, it means timezone is UTC
        if (timezone_offset_min < 0) {
            timezone_standard = '+' + parseInt(offset_hrs.toString()) + ':' + offset_min;
        } else if (timezone_offset_min > 0) {
            timezone_standard = '-' + parseInt(offset_hrs.toString()) + ':' + offset_min;
        } else if (timezone_offset_min === 0) {
            timezone_standard = 'Z';
        }

        // Timezone difference in hours and minutes
        // String such as +5:30 or -6:00 or Z
        console.log(timezone_standard);

        const timeZone = document.getElementsByClassName('systemTimeZone') as HTMLCollectionOf<HTMLElement>;
        timeZone[0].innerHTML = 'GMT ' + timezone_standard;
    }
}

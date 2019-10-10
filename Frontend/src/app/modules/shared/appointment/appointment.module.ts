import { NgModule } from '@angular/core';
import { BookAppointmentComponent } from './book-appointment/book-appointment.component';
import { CommonModule } from '@angular/common';
import { ConfirmAppointmentComponent } from './confirm-appointment/confirm-appointment.component';
import { ReactiveFormsModule } from '@angular/forms';
import { BookingMessageComponent } from './booking-message/booking-message.component';
import { BGDirective } from './book-appointment/change-background';

@NgModule({
    declarations: [BookAppointmentComponent, ConfirmAppointmentComponent, BookingMessageComponent, BGDirective],
    imports: [CommonModule, ReactiveFormsModule],
    exports: [BookAppointmentComponent, BookingMessageComponent],
})

export class AppointmentModule { }

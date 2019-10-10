import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HttpInterceptorService } from './core/services/interceptors/http-interceptor.service';
import { appReducer } from './core/store/reducers/app.reducer';
import { ClientEffect } from './core/store/effects/client/client.effects';
import { AppointmentEffect } from './core/store/effects/appointment/appointment.effects';
import { UploadFileComponent } from './modules/upload-file/upload-file.component';
import { LoginComponent } from './modules/auth/login/login.component';
import { ModulesModule } from './modules/modules.module';
import { ApiService } from './shared/services/api-service/api.service';
import { DoctorEffect } from './core/store/effects/doctor/doctor.effects';

@NgModule({
  declarations: [
    AppComponent,
    UploadFileComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    StoreModule.forRoot(appReducer),
    CommonModule,
    EffectsModule.forRoot([ClientEffect, AppointmentEffect, DoctorEffect]),
    ModulesModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HttpInterceptorService, multi: true },
    ApiService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

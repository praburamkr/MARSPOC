import { NgModule } from '@angular/core';

import { AppointmentModule } from './appointment/appointment.module';
import { ClientModule } from './client/client.module';
import { MenuComponent } from './menu/menu.component';
import { PhoneCallPluginComponent } from '../scheduler/phone-call-plugin/phone-call-plugin.component';
import { ApiService } from 'src/app/shared/services/api-service/api.service';
import { DynamicHeightDirective } from 'src/app/shared/directives/dynamic-height/dynamic-height.directive';

@NgModule({
    declarations: [
        MenuComponent,
        DynamicHeightDirective
        //   PhoneCallPluginComponent
    ],
    imports: [
        AppointmentModule,
        ClientModule
    ],
    exports: [
        AppointmentModule,
        ClientModule,
        MenuComponent,
        DynamicHeightDirective
        // PhoneCallPluginComponent
    ],
    providers: [ApiService]
})

export class SharedModule { }

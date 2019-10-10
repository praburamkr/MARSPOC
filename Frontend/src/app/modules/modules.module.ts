import { NgModule } from '@angular/core';
import { SharedModule } from './shared/shared.module';
import { SchedulerModule } from './scheduler/scheduler.module';
import { HomeComponent } from './home/home.component';

@NgModule({
    imports: [
        SharedModule,
        SchedulerModule,
    ],
    exports: [
        SharedModule,
        SchedulerModule
    ],
    declarations: [HomeComponent]
})

export class ModulesModule {}

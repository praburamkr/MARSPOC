import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ClientInfoComponent } from './client-info/client-info.component';

const routes: Routes = [
    {
        path: '',
        component: ClientInfoComponent,

    }
]


@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ClientRoutingModule { }

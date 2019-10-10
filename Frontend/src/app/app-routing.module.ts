import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './modules/auth/login/login.component';
import { HomeComponent } from './modules/home/home.component';
import { AuthGuardService } from './shared/services/auth-guard/auth-guard.service';

const routes: Routes = [
    { path: '', component: LoginComponent },
    {
        path: 'scheduler',
        component: HomeComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: '',
        loadChildren: () => import('./modules/shared/client/client.module').then(m => m.ClientModule)
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, { initialNavigation: 'enabled' })],
    exports: [RouterModule]
})
export class AppRoutingModule { }

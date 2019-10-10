import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private router: Router) { }

  canActivate(): any {
    const token = localStorage.getItem('access_token');
    if (token) {
      return true;
    } else {
      this.router.navigate(['']);
    }
  }
}

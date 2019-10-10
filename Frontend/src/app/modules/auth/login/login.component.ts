import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/shared/services/api-service/api.service';
import { UtilsService } from 'src/app/shared/services/utils/utils.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  showAllErrors: boolean;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private apiService: ApiService,
    private utilsService: UtilsService) { }

  ngOnInit() {
    this.isLoggedIn();
  }

  isLoggedIn(): void {
    const token = localStorage.getItem('access_token');
    if (token) {
      this.router.navigate(['/scheduler']);
    } else {
      this.createForm();
    }
  }

  createForm(): void {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  canLogin() {
    if (this.loginForm.valid) {
      this.showAllErrors = true;
      this.login();
    } else {
      this.showAllErrors = true;
    }
  }

  login(): void {
    const requestBody = {
      UserName: this.loginForm.value.username,
      Password: this.loginForm.value.password
    };
    this.apiService.postRequest(`${this.utilsService.getAuthURL()}/api/security/login`, requestBody).subscribe(response => {
      if (response) {
        localStorage.setItem('access_token', response.data.access_token);
        localStorage.setItem('username', requestBody.UserName);
        this.router.navigate(['/scheduler']);
      } else {

      }
    }, err => {

    });
  }

}

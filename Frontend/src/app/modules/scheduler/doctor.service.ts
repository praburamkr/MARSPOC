import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/shared/services/api-service/api.service';
import { Observable, of } from 'rxjs';
import { UtilsService } from 'src/app/shared/services/utils/utils.service';
import { IGetDoctorResponse } from 'src/app/shared/models/doctor.interface';

@ Injectable({
  providedIn: 'root'
})
export class DoctorService {
  constructor(private apiService: ApiService, private utilsService: UtilsService) { }

  getAllDoctors(body: any): Observable<IGetDoctorResponse> {
    return this.apiService.postRequest(`${this.utilsService.getResourceURL()}/api/resources/search`, body);
  }

}

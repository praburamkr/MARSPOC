import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/shared/services/api-service/api.service';
import { Observable, of } from 'rxjs';
import { IClient, IGetClientPatientResponse, IClientByName } from 'src/app/shared/models/client.interface';
import { UtilsService } from 'src/app/shared/services/utils/utils.service';

@Injectable({
  providedIn: 'root'
})
export class ClientService {
  constructor(private apiService: ApiService, private utilsService: UtilsService) { }

  getClientInfo(): Observable<IClient> {
    return this.apiService.getRequest('./assets/client/client-info.json');
  }

  addClientPetInfo(body: any): Observable<any> {
    return this.apiService.postRequest(`${this.utilsService.getCustomerURL()}/api/patients`, body);
  }

  getClientPatients(clientId: number): Observable<any> {
    return this.apiService.getRequest(`${this.utilsService.getCustomerURL()}/api/clients/` + clientId);
  }

  getAllClients(): Observable<IClient> {
    return this.apiService.getRequest(`${this.utilsService.getCustomerURL()}/api/clients/searchall`);
  }

  getAllClientsByName(body: any): Observable<IClientByName> {
    return this.apiService.postRequest(`${this.utilsService.getCustomerURL()}/api/clients/search`, body);
  }

}

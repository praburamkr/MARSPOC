import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { apiUrl, clientId} from '../../../app.constants';
const headers = new HttpHeaders({
  'Content-Type': 'application/json',
  'Content-Security-Policy': 'upgrade-insecure-requests',
  'Access-Control-Allow-Credentials': 'true',
  'Access-Control-Allow-Origin': '*',
  'Access-Control-Allow-Headers': 'Origin, X-Requested-With, Content-Type, Accept, Authorization',
  'Cache-Control': 'no-cache'
});

const httpOptions = {
  headers
};

@Injectable({
  providedIn: 'root'
})

export class ApiService {

  constructor(private http: HttpClient) { }

  getRequest(url: string, pathParams?: string | object, queryParam?: string | object, apiType?): Observable<any> {
    let generatedQueryParam;
    if (pathParams) {
      url = this.replaceDynamicPathValue(url, pathParams);
    }
    if (queryParam) {
      generatedQueryParam = this.generateQueryParam(queryParam);
    }


    return this.http.get(url, { headers, params: generatedQueryParam });
  }

  postRequest(url: string, requestBody: object, pathParams?: string | object): Observable<any> {
    if (pathParams) {
      url = this.replaceDynamicPathValue(url, pathParams);
    }


    return this.http.post(url, requestBody, httpOptions);
  }

  replaceDynamicPathValue(url: string, pathParam: string | object): string {
    let urlWithPathParam = url;
    const keys = Object.keys(pathParam);
    for (const key of keys) {
      urlWithPathParam = urlWithPathParam.replace(`{${key}}`, pathParam[key]);
    }

    return urlWithPathParam;
  }

  generateQueryParam(queryParams: string | object): HttpParams {
    let queryParam = new HttpParams();
    const keys = Object.keys(queryParams);
    for (const key of keys) {
      queryParam = queryParam.set(key, queryParams[key]);
    }

    return queryParam;

  }

  // postRequest(url: string, body: any): Observable<any> {
  //   return this.http.post(`${apiUrl}/${url}`, body);
  // }
}

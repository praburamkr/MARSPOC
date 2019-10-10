import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '@environments/environment';

export class HttpHelpers {
  constructor(private httpClient: HttpClient) { }

  public getAction<T>(apiEndpoint: string, param?: any): Observable<T> {
    // return this.httpClient.get<T>(`${environment.apiUrl}/` + apiEndpoint + param).pipe(map(response => { return response }));
    return this.httpClient.get<T>(`https://jsonplaceholder.typicode.com/todos/1`).pipe(map(response => response));
  }
}


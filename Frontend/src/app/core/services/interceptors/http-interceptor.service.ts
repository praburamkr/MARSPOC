import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class HttpInterceptorService implements HttpInterceptor {
  constructor(private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // add authorization header with basic auth credentials if available
    request = this.updateHeader(request);
    // request = request.clone({
    //   headers: request.headers.append('Content-Type', 'application/json')
    // });

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error instanceof HttpErrorResponse) {
          // server-side error
          if (error.status === 401) {
            localStorage.removeItem('token');
            this.router.navigate(['']); // in case of unauthorized user clear cookies and redirect to login page
          }

          // console.log(error);
          return throwError(error);
        } else {
          // client-side error
          return throwError(error);
        }
      })
    );
  }

  updateHeader(request): HttpRequest<any> {
    const token = localStorage.getItem('access_token');
    if (token) {
      request = request.clone({
        headers: request.headers.append('Content-Type', 'application/json'),
        // withCredentials: true
      });
    } else {
      request = request.clone({
        headers: request.headers.append('Content-Type', 'application/json')
      });
    }


    return request;
  }
}

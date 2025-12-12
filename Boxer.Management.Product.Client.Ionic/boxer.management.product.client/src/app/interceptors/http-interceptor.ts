import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import {  Login } from '../service/loginService';

@Injectable()
export class HttpInterceptorService implements HttpInterceptor {
  constructor(public login: Login) {}

  

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {

      if (req.url.includes('/login') || !this.login.accessToken) {
      return next.handle(req);
    }

    const clonedRequest = req.clone({
      setHeaders: {
        Authorization: `Bearer ${this.login.accessToken}`, // Replace with a dynamic token
      },
    });

    return next.handle(clonedRequest).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error('HTTP Error:', error.message);
        return throwError(() => new Error(error.message));
      })
    );
  }
}

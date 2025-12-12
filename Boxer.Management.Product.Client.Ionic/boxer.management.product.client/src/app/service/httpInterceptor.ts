import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import {  Login } from './loginService';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private login: Login) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // Skip for auth requests or if no token exists
    if (request.url.includes('/login') || request.url.includes('/refresh-token') || !this.login.accessToken) {
      return next.handle(request);
    }

    // Clone the request and add the authorization header
    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${this.login.accessToken}`
      }
    });

    return next.handle(request);
  }
}
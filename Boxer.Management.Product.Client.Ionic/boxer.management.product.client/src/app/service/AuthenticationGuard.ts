import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Login } from './loginService'

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private loginService: Login ,
     private router: Router) {}

  canActivate(): boolean {
    const token = this.loginService.accessToken;
    if (token) {
      return true;
    } else {
      this.router.navigate(['/login-page']);
      return false;
    }
  }
}
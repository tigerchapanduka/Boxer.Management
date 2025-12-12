import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import * as jwt_decode from 'jwt-decode';
import { loginResponse, } from '../Model/loginResponse';
import { userAccount} from '../Model/userAccount';
import { environment} from 'src/environments/environment'
@Injectable({
  providedIn: 'root',
})
export class Login {


   private token:string;
   public loggedIn = new BehaviorSubject<boolean>(false);
   public hasLoginError = new BehaviorSubject<boolean>(false);
    

    constructor(private router: Router,private http: HttpClient) { 
       const _token = localStorage.getItem('token');
    if (_token) {
       console.log('Logged in from memory');
       this.token = _token;
       this.loggedIn.next(true);
    }
    this.token = '';
      
   }

  public get accessToken(): string {
         const _token = localStorage.getItem('token');
    if (_token) {
      return _token;
    }
    return '';
  }

    public set accessToken(token:string) {
      localStorage.setItem('token', token);
    }

      public get userId(): string {
         const _token = localStorage.getItem('userId');
    if (_token) {
      return _token;
    }
    return '';
  }

    public set userId(Id:string) {
      localStorage.setItem('userId', Id);
    }

  signupService(email: string, password: string) {
  
        //https://localhost:7168/
        return this.http.post(environment.productBaseURL+'userAccounts/LoginUserAccount',{ UserName:email, Password: password }).subscribe(
        {
            next: (response) => {
                let tokenVal:string = (<loginResponse>response).token;
                let _userId:string = (<loginResponse>response).userId;
                this.userId = _userId;
                this.accessToken=(tokenVal);
                this.loggedIn.next(true);
             
          },
          error: (err: any) => {
              this.hasLoginError.next(true);
          },
          complete: () => {
              console.log('Observable completed.');
          }

        }

        );
  
  }
  
  logout() {
    localStorage.removeItem('token');
      this.router.navigateByUrl('/login-page');
  }

}

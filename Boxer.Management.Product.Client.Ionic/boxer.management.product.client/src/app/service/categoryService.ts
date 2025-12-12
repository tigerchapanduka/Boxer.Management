import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import * as jwt_decode from 'jwt-decode';
import { category } from '../Model/category';
@Injectable({
  providedIn: 'root',
})
export class CategoryService {

   public saveSuccess = new BehaviorSubject<boolean>(false);
   public saveError = new BehaviorSubject<boolean>(false);
    constructor(private router: Router,private http: HttpClient) { 

    }
  
    save(name: string , userId:string){
      return this.http.post('https://localhost:7168/category',{ name:name, createdById:userId }).subscribe({
          next: (response) => {
                  this.saveSuccess.next(true);
                  },
          error: (e) => {
                     this.saveError.next(false);
                },
          complete() {
                console.log("Completed.");
          },
      });
    }

    getCategories() {
      return this.http.get('https://localhost:7168/category');  
  
    }
  }
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { product } from '../Model/product';
import { environment} from 'src/environments/environment'
@Injectable({
  providedIn: 'root',
})
export class ProductService {
  
  public saveSuccess = new BehaviorSubject<boolean>(false);
  public saveNewError = new BehaviorSubject<boolean>(false);
  public savedProduct = new BehaviorSubject<product>({id:'',name:'',description:'',categoryId:'',userId:'',code:'',cost:0})
  public updateSuccess = new BehaviorSubject<boolean>(false);
  public updateError = new BehaviorSubject<boolean>(false);
    constructor(private router: Router,private http: HttpClient) { 

    }
  
    save(_product:product){

      return this.http.post(environment.productBaseURL+'product', _product ).subscribe({
                                    next: (response) => {
                                        this.savedProduct.next(<product>response);
                                        this.saveSuccess.next(true);
                                      },
                                      error: (e) => {
                                        this.saveNewError.next(true);  
                                      },
                              complete() {
                                    console.log("Completed.");
                              },
                          });
    }

    getProducts() {
      return this.http.get(environment.productBaseURL+'product');  
  
    }
    updateProduct(_product:product){
      return this.http.put(environment.productBaseURL+'product/'+_product.id, _product ).subscribe({
                
               next: (response) => {
                        this.updateSuccess.next(true);
                },                
                error: (err: any) => {
                        this.updateError.next(true);
                },
                complete: () => {
                    console.log('Product update completed.');
                }

          })
    }
    getProduct(productId:string){
      return this.http.get(environment.productBaseURL+'product/'+productId);
    }
    deleteProduct(_product:product){
      return this.http.delete(environment.productBaseURL+'product/'+_product.id);
    }
    filterProduct(categoryName:string,productName:string){
      return this.http.get(environment.productBaseURL+'product/'+categoryName+'/'+productName);
    }

}

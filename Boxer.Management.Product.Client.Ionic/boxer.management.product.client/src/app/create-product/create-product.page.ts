import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {IonSelectOption , IonSelect, IonMenu, IonMenuButton,IonContent, IonHeader, IonTitle, IonToolbar, IonButtons, IonItem, IonAlert, IonList, IonButton, IonInput, IonListHeader, IonLabel, IonLoading } from '@ionic/angular/standalone';
import { Login } from '../service/loginService';
import { Router } from '@angular/router';
import { category,categoryList } from '../Model/category';
import { CategoryService } from '../service/categoryService';
import { ProductService } from '../service/product-service';
import { product } from '../Model/product';

@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.page.html',
  styleUrls: ['./create-product.page.scss'],
  standalone: true,
  imports: [IonLoading, IonSelectOption,IonSelect ,IonAlert,IonInput,IonLabel, IonListHeader, IonButton, IonList, IonItem, IonButtons, IonMenu, IonMenuButton,IonContent, IonHeader, IonTitle, IonToolbar, CommonModule, FormsModule]
})


export class CreateProductPage implements OnInit {

  isAlertOpen = false;
  isBusy=false;
  alertButtons = ['OK'];
  alertMessage!:any;
  categoryList!:Array<category>;
  selectedCategory:category={id:'',name:'',errorMessage:''};
  categoryId:string='';
  _product:product= {id:'',name:'',description:'',categoryId:'',userId:'',code:'',cost:0} ;

    constructor(
    private categoryService: CategoryService ,
    private productService: ProductService,
    private router:Router,
    private loginService:Login

  ) { }

 
  ngOnInit() {

    this.isAlertOpen=false;
   this.productService.savedProduct.subscribe({
             next:(response)=>{
                 this._product=<product>response;
                 this.isBusy=false;
             }
    });


    
    this.productService.saveNewError.subscribe({
      next:(response)=>{
        if(response){
                        this.isBusy=false;
                        this.alertMessage="There was an exception saving product. Please contact Admin";
                        this.setOpen(true);                 
        }
      }
    })

        this.productService.saveSuccess.subscribe({
      next:(response)=>{
        if(response){
                      this.isBusy=false;
                      this.alertMessage='Product saved';
                      this.setOpen(true);               
        }
      }
    })
                                        

    this.categoryService.getCategories().subscribe({
          next: (response) => {
                               this.isBusy=false;
                               this.categoryList = (<categoryList>response).categories;
                  },
          error: (e) => {
                          this.isBusy=false;
                },
          complete() {
                
          },
      });
    

  }

  setOpen(isOpen: boolean) {
    this.isAlertOpen = isOpen;
  }
  save(){
    this.isBusy=true;
    this._product.categoryId = this.selectedCategory.id;
    this._product.userId = this.loginService.userId;
    let errorMessage:string='';
    if(!this._product.categoryId ){
       errorMessage+='Category, \n';
    }
    if(!this._product.name ){
       errorMessage+=' Name, \n';
    }
    if(!this._product.description ){
       errorMessage+=' Description, \n';
    }
    if(!this._product.code ){
       errorMessage+=' Code, \n';
    }
    if(this._product.cost ==0 ){
       errorMessage+=' Cost \n';
    }
    
    if(errorMessage.length>1){
      this.isBusy=false;
      this.alertMessage=errorMessage +' is required';
      this.setOpen(true);
    }else{
          this.productService.save(this._product)
  }

  }

}

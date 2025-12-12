import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {IonInput, IonSelect, IonGrid,IonRow,IonCol, IonButton,IonLabel,IonList,IonItem,IonAlert,IonSelectOption,IonMenuButton, IonContent, IonHeader, IonTitle, IonToolbar, IonButtons } from '@ionic/angular/standalone';
import { Login } from '../service/loginService';
import { Router,NavigationExtras } from '@angular/router';
import { category,categoryList } from '../Model/category';
import { CategoryService } from '../service/categoryService';
import { ProductService } from '../service/product-service';
import { product,productList } from '../Model/product';
@Component({
  selector: 'app-view-product',
  templateUrl: './view-product.page.html',
  styleUrls: ['./view-product.page.scss'],
  standalone: true,
  imports: [IonInput,IonSelect,IonGrid,IonRow,IonCol,
    IonButton,IonLabel,IonList,
    IonItem,IonAlert,IonSelectOption,
    IonMenuButton,IonButtons, IonContent,
     IonHeader, IonTitle, IonToolbar, 
     CommonModule, FormsModule]
})
export class ViewProductPage implements OnInit {

  isAlertOpen = false;
  alertButtons = ['OK'];
  alertMessage!:any;
  categoryList!:Array<category>;
  productList!:Array<product>;
  selectedCategory:category={id:'',name:'',errorMessage:''};
  categoryId:string='';
  _product:product= {id:'',name:'',description:'',categoryId:'',userId:'',code:'',cost:0} ;
  searchCategory:string='';
  searchProduct:string='';

    constructor(
    private categoryService: CategoryService ,
    private productService: ProductService,
    private router:Router,
    private loginService:Login

  ) { }

 
  ngOnInit() {


      this.productService.updateError.subscribe({
      next:(response)=>{
        if(response){
              this.alertMessage="There was an exception saving product. Please contact Admin";
              this.setOpen(true);                 
        }
      }
    })

    this.productService.updateSuccess.subscribe({
      next:(response)=>{
        if(response){
                      this.alertMessage='Product saved';
                      this.setOpen(true);               
        }
      }
    })

    this.productService.getProducts().subscribe({
          next: (response) => {
         this.productList = (<productList>response).products;
                  },
          error: (e) => {
                      console.error(e);
                },
          complete() {
                console.log("Completed.");
          },
      });
    

          this.categoryService.getCategories().subscribe({
          next: (response) => {
         this.categoryList = (<categoryList>response).categories;
                  },
          error: (e) => {
                      console.error(e);
                },
          complete() {
                console.log("Completed.");
          },
      });
    

  }

  setOpen(isOpen: boolean) {
    this.isAlertOpen = isOpen;
  }

  delete(_product:product){
     
              this.productService.deleteProduct(_product).subscribe({
                
               next: (response) => {
                           this.alertMessage= 'Product has been deleted';
                           this.setOpen(true);
                           this.router.navigateByUrl('/view-product');
                },                
                error: (err: any) => {
                         this.alertMessage= 'The following error occured deleting' +err;
                           this.setOpen(true);
                },
                complete: () => {
                    console.log('Observable completed.');
                }

          })

  }
    
  updateProduct(_product:product){
     
      _product.userId = this.loginService.userId;
          this._product.categoryId = this.selectedCategory.id;
    _product.userId = this.loginService.userId;
    let errorMessage:string='';
    if(!_product.categoryId ){
       errorMessage+='Category, \n';
    }
    if(!_product.name ){
       errorMessage+=' Name, \n';
    }
    if(!_product.description ){
       errorMessage+=' Description \n';
    }
    if(!_product.code ){
       errorMessage+=' Code, \n';
    }
    if(_product.cost ==0 ){
       errorMessage+=' Cost \n';
    }
    
    if(errorMessage.length>1){
      this.alertMessage=errorMessage +' is required';
      this.setOpen(true);
    }else{
              this.productService.updateProduct(_product);
    }

  }

  viewProduct(_product:product){

    this.router.navigate(['/product-detail'],{
          queryParams: { productId: _product.id }
      });

  }
   filterProduct(){

    if(!this.searchCategory){
      this.searchCategory='-1';
    }
    if(!this.searchProduct){
       this.searchProduct='-1';
    }
        this.productService.filterProduct(this.searchCategory,this.searchProduct).subscribe({
          next: (response) => {
         this.productList = (<productList>response).products;
                        if(this.searchCategory=='-1'){
                          this.searchCategory='';
                        }
                        if(this.searchProduct=='-1'){
                          this.searchProduct='';
                        }
                  },
          error: (e) => {
                   
                        if(this.searchCategory=='-1'){
                          this.searchCategory='';
                        }
                        if(this.searchProduct=='-1'){
                          this.searchProduct='';
                        }
                      this.alertMessage="An exception has been raised. Please contact Admin";
                      this.setOpen(true);
                },
          complete() {
                console.log("Completed.");
          },
      });
  }


}

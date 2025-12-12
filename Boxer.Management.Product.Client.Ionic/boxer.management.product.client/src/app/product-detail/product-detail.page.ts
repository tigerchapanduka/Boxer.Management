import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {IonInput, IonSelect, IonGrid,IonRow,IonCol, IonButton,IonLabel,IonList,IonItem,IonAlert,IonSelectOption,IonMenuButton, IonContent, IonHeader, IonTitle, IonToolbar, IonButtons } from '@ionic/angular/standalone';
import {  Login } from '../service/loginService';
import { ActivatedRoute,Router,NavigationExtras } from '@angular/router';
import { category,categoryList } from '../Model/category';
import { CategoryService } from '../service/categoryService';
import { ProductService } from '../service/product-service';
import { product,productList } from '../Model/product';
@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.page.html',
  styleUrls: ['./product-detail.page.scss'],
  standalone: true,
  imports: [IonInput,IonSelect,IonGrid,IonRow,IonCol,
    IonButton,IonLabel,IonList,
    IonItem,IonAlert,IonSelectOption,
    IonMenuButton,IonButtons, IonContent,
     IonHeader, IonTitle, IonToolbar, 
     CommonModule, FormsModule]
})
export class ProductDetailPage implements OnInit {
  isAlertOpen = false;
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
    private loginService:Login,
    private activeRoute:ActivatedRoute

  ) { }

  ngOnInit() {

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

      this.activeRoute.queryParams.subscribe({
          next: (response) => {
                
                                let productId = response['productId'];
                                this.productService.getProduct(productId).subscribe({
                                    next: (response) => {
                                        this._product = (<product>response);
                                      },
                                      error: (e) => {
                                          this.alertMessage='An exception was raised getting Product. Please contact Admin'
                                        },
                              complete() {
                                    console.log("Completed.");
                              },
                          });
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

}

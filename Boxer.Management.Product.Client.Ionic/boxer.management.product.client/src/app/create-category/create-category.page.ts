import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {IonSelectOption , IonMenu, IonMenuButton,IonContent, IonHeader, IonTitle, IonToolbar, IonButtons, IonItem, IonAlert, IonList, IonButton, IonInput, IonListHeader, IonLabel } from '@ionic/angular/standalone';
import { CategoryService} from '../service/categoryService'
import { Router } from '@angular/router';
import { category } from '../Model/category';
import {  Login } from '../service/loginService';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
@Component({
  selector: 'app-create-category',
  templateUrl: './create-category.page.html',
  styleUrls: ['./create-category.page.scss'],
  standalone: true,
  imports: [IonButtons,  IonMenuButton,IonAlert,IonInput,IonButton, IonList, IonItem, IonContent, IonHeader, IonTitle, IonToolbar, CommonModule, FormsModule]
})
export class CreateCategoryPage implements OnInit {

   isAlertOpen = false;
   alertButtons = ['OK'];
   alertMessage!:any;
   categoryList!:Array<category>;
   selectedCategory!:category;
   name!:string;

    
    constructor(
    private catService:CategoryService,
    private router:Router,
    private login: Login

  ) { }

  ngOnInit() {
    this.setOpen(false);
    this.catService.saveSuccess.subscribe({
      next:(resp)=>{
         if(resp){
                this.alertMessage = 'Category saved';
                this.setOpen(true);
         }
      }
    });
        this.catService.saveError.subscribe({
      next:(resp)=>{
         if(resp){
                this.alertMessage = 'There was an exception saving category';
                this.setOpen(true);
         }
      }
    })

  }

    setOpen(isOpen: boolean) {
    this.isAlertOpen = isOpen;
  }
  save(){

    if(this.name){
     this.catService.save(this.name,this.login.userId);
     }else{
      this.alertMessage = 'Category is required';
      this.setOpen(true);
     }
  }

}

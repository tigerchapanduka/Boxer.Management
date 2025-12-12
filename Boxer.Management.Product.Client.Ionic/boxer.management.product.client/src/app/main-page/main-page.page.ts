import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {  IonMenu, IonMenuButton,IonContent, IonHeader, IonTitle, IonToolbar, IonButtons, IonItem, IonAlert, IonList, IonButton, IonInput, IonListHeader, IonLabel } from '@ionic/angular/standalone';
import { Router } from '@angular/router';
import { Login } from '../service/loginService'
@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.page.html',
  styleUrls: ['./main-page.page.scss'],
  standalone: true,
  imports: [IonTitle, IonButtons,  IonMenuButton,IonContent, IonHeader,  IonToolbar, CommonModule, FormsModule]
})
export class MainPagePage implements OnInit {

  constructor(
    private loginService: Login ,
    private router: Router) { }

  ngOnInit() {
   
  }

  logUserOut(){
    this.loginService.logout();
  
  }
  newProduct(){
     this.router.navigateByUrl('/create-product');
  }
  manageProduct(){
     this.router.navigateByUrl('/view-product');
  
  }
  newContact(){
     this.router.navigateByUrl('/new-contact');
  }

}

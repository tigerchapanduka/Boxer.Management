import { Component } from '@angular/core';
import { IonSelect, IonSelectOption,IonApp,IonMenu,IonButtons,IonMenuButton, IonRouterOutlet, IonHeader, IonToolbar, IonTitle, IonContent, IonList, IonListHeader, IonLabel, IonItem, IonButton } from '@ionic/angular/standalone';
import { Router } from '@angular/router';
import { Login } from './service/loginService';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  imports: [IonMenu,IonButtons,IonMenuButton,IonButton, IonItem, IonLabel, IonListHeader, IonList, IonContent, IonTitle, IonToolbar, IonHeader, IonApp, IonRouterOutlet],
})
export class AppComponent {
  constructor(    private loginService: Login ,
    private router: Router) {}

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
  newCategory(){
     this.router.navigateByUrl('/create-category');
  }
}

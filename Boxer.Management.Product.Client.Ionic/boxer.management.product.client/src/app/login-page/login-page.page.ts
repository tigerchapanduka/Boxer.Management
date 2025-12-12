import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LoadingController, LoadingOptions,IonAlert, IonInput,IonContent, IonHeader, IonTitle, IonToolbar, IonList, IonItem,IonButton, IonLoading } from '@ionic/angular/standalone';
import { Login } from '../service/loginService'
import { Router } from '@angular/router';
import {  userAccount } from '../Model/userAccount'
@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.page.html',
  styleUrls: ['./login-page.page.scss'],
  standalone: true,
  imports: [IonLoading, IonAlert, IonInput,IonItem, IonList, IonContent, IonHeader, IonTitle, IonToolbar, CommonModule, FormsModule,IonButton]
})
export class LoginPagePage implements OnInit {

  isAlertOpen = false;
  isBusy=false;
  alertButtons = ['OK'];
  alertMessage!:any;
  userName!:string;
  password!:string;
  private initialOPts: LoadingOptions = { message: 'Loading...' };
  private loading!: HTMLIonLoadingElement;
  constructor( 
    private loginService: Login ,
    private router:Router,
    private loadingCtrl: LoadingController) { }

  async ngOnInit() {

    this.loading = await this.loadingCtrl.create(this.initialOPts);
    this.loginService.hasLoginError.subscribe({
      next:(hasError)=>{
       this.dismiss();
        if(hasError){
                this.alertMessage = 'An Exception has occured. Please contact Admin';
                this.isAlertOpen=true;
        }
      },
      error: (err: any) => {
            this.dismiss();
      }
    })

    this.loginService.loggedIn.subscribe({
      next:(islogged)=>{

        if(islogged){
             this.dismiss();
              this.router.navigateByUrl('/main-page');
        }
      },
      error: (err: any) => {
            this.dismiss();
      }
    })
      
  }
  setIsBusy(issusy: boolean) {
    this.isBusy = issusy;
  }
   async logUser(){

    //this.setIsBusy(true);
     this.present();
     if(this.userName && this.password){
     this.loginService.signupService(this.userName, this.password);
    

     }else{
      this.alertMessage = 'Username and Password are requried';
      this.isAlertOpen=true;
       this.dismiss();
     }
  }

  setOpen(isOpen: boolean) {
    this.isAlertOpen = isOpen;
  }
    present(): void {
        //this.isBusy = true;
        this.loading.present();
    }

    dismiss(): void {
  
            this.loading.dismiss();

    }
}

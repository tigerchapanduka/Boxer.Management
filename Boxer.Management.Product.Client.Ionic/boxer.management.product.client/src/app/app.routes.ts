import { Routes } from '@angular/router';
import { AuthGuard } from './service/AuthenticationGuard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./login-page/login-page.page').then( m => m.LoginPagePage),
  },
  {
    path: 'create-product',
    loadComponent: () => import('./create-product/create-product.page').then( m => m.CreateProductPage), canActivate: [AuthGuard]
  },
  {
    path: 'view-product',
    loadComponent: () => import('./view-product/view-product.page').then( m => m.ViewProductPage), canActivate: [AuthGuard]
  },
  {
    path: 'login-page',
    loadComponent: () => import('./login-page/login-page.page').then( m => m.LoginPagePage)
  },
  {
    path: 'main-page',
    loadComponent: () => import('./main-page/main-page.page').then( m => m.MainPagePage), canActivate: [AuthGuard]
  },
  {
    path: 'new-contact',
    loadComponent: () => import('./new-contact/new-contact.page').then( m => m.NewContactPage), canActivate: [AuthGuard]
  },
  {
    path: 'create-category',
    loadComponent: () => import('./create-category/create-category.page').then( m => m.CreateCategoryPage)
  },
  {
    path: 'product-detail',
    loadComponent: () => import('./product-detail/product-detail.page').then( m => m.ProductDetailPage)
  },
];

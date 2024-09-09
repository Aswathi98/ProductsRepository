
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductDetailsComponent } from './product-details/product-details.component';


const routes: Routes = [
       { path: '', redirectTo: '/productdetails', pathMatch: 'full' },
       { path: 'productdetails', component: ProductListComponent },  // Define the route for product list
      
      { path: 'product/:id', component: ProductDetailsComponent },  // Default route
   ];

@NgModule({
  imports: [RouterModule.forRoot(routes)], // Register routes in the application
  exports: [RouterModule] // Export RouterModule to make it available throughout the app
})
export class AppRoutingModule { }
export { routes };
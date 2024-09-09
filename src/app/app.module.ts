import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { RouterModule } from '@angular/router'; 
import { ProductListComponent } from './product-list/product-list.component';  // Import ProductListComponent
import { ProductDetailsComponent } from './product-details/product-details.component';

@NgModule({
  declarations: [
    AppComponent,
    ProductListComponent,
    ProductDetailsComponent
  ],
  imports: [
    BrowserModule
 ,RouterModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

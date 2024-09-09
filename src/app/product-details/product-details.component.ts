import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../services/product.service';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Product } from '../Models/product.model';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  
  product$: Observable<Product> = new Observable<Product>();  // Use empty observable

  constructor(
    private productService: ProductService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.product$ = this.route.paramMap.pipe(
      switchMap(params => {
        const id = params.get('id');  // Get the product ID from route
        if (id) {
          return this.productService.getProductById(id);  // Fetch product details
        } else {
          throw new Error('Product ID is missing');
        }
      })
    );
  }
}

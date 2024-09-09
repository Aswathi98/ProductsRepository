import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Product } from '../Models/product.model';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {

  products: Product[] = [];  // Define an array to hold products

  constructor(private productService: ProductService, private router: Router) {}

  ngOnInit(): void {
    // Fetch products from the service and handle the data
    this.productService.getProducts().subscribe(
      data => {
        this.products = data;  // Assign the fetched data to the products array
      },
      error => {
        console.error('Error fetching products:', error); // Log error if any
      }
    );
  }

  onSelectProduct(id: string): void {
    // Navigate to the product detail view
    this.router.navigate(['/product', id]);
  }
}

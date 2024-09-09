import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'https://localhost:44361/api/products';  

  constructor(private http: HttpClient) { }

  getProducts(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl).pipe(
      catchError(error => {
        console.error('Error fetching product', error);
        return throwError(() => new Error('Error fetching product'));
      }));
  }
  
  getProductById(id: string): Observable<any> {  
    return this.http.get<any>(`${this.apiUrl}/${id}`).pipe(
      catchError(error => {
        console.error('Error fetching product details', error);
        return throwError(() => new Error('Error fetching product details'));
      })
    );
  }
}


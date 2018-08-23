import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProductsServiceService {

  constructor(private http: HttpClient) {
  }



  getProducts(apikey:string) {

    return this.http.get<any>('http://localhost:51680/api/products',
      {
        headers: new HttpHeaders(
          { 'X-API-Key': apikey }
        )
      });
  }
}

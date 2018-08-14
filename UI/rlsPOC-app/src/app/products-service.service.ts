import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest } from '@angular/common/http';

const headers = new HttpHeaders()
  .set("X-API-Key", "081FF61A-E688-4DC2-84E7-6CC8FFED4D69");

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

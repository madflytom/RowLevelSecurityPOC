import { Component, OnInit } from '@angular/core';
import { ProductsServiceService } from '../products-service.service';

@Component({
  selector: 'app-products-view',
  templateUrl: './products-view.component.html',
  styleUrls: ['./products-view.component.css']
})
export class ProductsViewComponent implements OnInit {
  productArray: any;
  apiKey: string;

  constructor(private productsService: ProductsServiceService) { }

  ngOnInit() {  }

  getProducts(){
    this.productsService.getProducts(this.apiKey).subscribe(result =>
      {
        this.productArray = result;
        console.log(this.productArray);
      })
  }

}

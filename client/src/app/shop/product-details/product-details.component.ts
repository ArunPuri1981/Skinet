import { Component, OnInit } from '@angular/core';
import { Products } from 'src/app/shared/models/products';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product?: Products;

  constructor(private shopService: ShopService, private activcatedRoute: ActivatedRoute,
    private bcService:BreadcrumbService) { }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    const id = this.activcatedRoute.snapshot.paramMap.get('id');
    if(id) this.shopService.getProduct(+id).subscribe({
      next: product=> {this.product=product;
      this.bcService.set('@productDetails',product.name)},
      error: error=> console.log(error),
    });
  }


}

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../shared/models/pagination';
import { Products } from '../shared/models/products';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/types';
import { ShopParams } from '../shared/models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5000/api/';

  constructor(private http: HttpClient) { }

  getProducts(shopParams:ShopParams) {
    let params = new HttpParams();

    if (shopParams.brandId) params = params.append('brandid', shopParams.brandId);
    if (shopParams.typeId) params = params.append("typeid", shopParams.typeId);
    params = params.append("sort", shopParams.sort);
    params = params.append("pageIndex", shopParams.pageNumber);
    params = params.append("pageSize", shopParams.pageSize);
    if(shopParams.search) params=params.append("search",shopParams.search);

    return this.http.get<Pagination<Products[]>>(this.baseUrl + 'products', { params });
  }

  getBrand() {
    return this.http.get<Brand[]>(this.baseUrl + "products/brands");
  }

  getTypes() {
    return this.http.get<Type[]>(this.baseUrl + "products/types");
  }
}

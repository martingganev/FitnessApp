import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Recipe } from '../models/Recipe';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable()
export class ProductService {

  private allProductsByNamesPath = environment.apiUrl + "/Products/AllProductsByNames";

  constructor(private http: HttpClient) { }

  getProductsByNames(): Observable<Array<Recipe>> {
    return this.http.get<Array<Recipe>>(this.allProductsByNamesPath);
  }
}

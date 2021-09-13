import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Recipe } from '../models/Recipe';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { RecipeByName } from '../models/RecipeByName';

@Injectable()
export class RecipeService {

  private allRecipesPath = environment.apiUrl + "/Recipes/AllRecipes";
  private recipeDetailsPath = environment.apiUrl + "/Recipes/";
  private addRecipeToUserPath = environment.apiUrl + "/Recipes/AddRecipeToUser";
  private createRecipePath = environment.apiUrl + "/Recipes/Create";
  private allRecipesByNamesPath = environment.apiUrl + "/Recipes/AllRecipesByNames";
  private editRecipePath = environment.apiUrl + "/Recipes/";
  private deleteRecipeLogPath = environment.apiUrl + "/Recipes/DeleteRecipeLog";

  constructor(private http: HttpClient, private authService: AuthService) { }

  getRecipes(): Observable<Array<Recipe>> {
    return this.http.get<Array<Recipe>>(this.allRecipesPath);
  }

  getRecipe(id: number): Observable<Recipe> {
    return this.http.get<Recipe>(this.recipeDetailsPath + id);
  }

  getRecipesByNames(): Observable<Array<RecipeByName>> {
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken()};
    return this.http.get<Array<RecipeByName>>(this.allRecipesByNamesPath, {headers});
  }

  addRecipeToUser(id: number) {
      const headers = { 'Authorization': 'Bearer ' + this.authService.getToken()};
      return this.http.post<any>(this.addRecipeToUserPath, {"id": id}, { headers });
  }

  createRecipe(data){
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken()};
    console.log("bearer: " + headers.Authorization)
    return this.http.post<any>(this.createRecipePath, data, {headers});
  }

  editRecipe(id, data) {
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken()};
    console.log("bearer: " + headers.Authorization)
    return this.http.put<any>(this.editRecipePath + id, data, {headers});
  }

  deleteRecipe(id) {
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken()};
    console.log("bearer: " + headers.Authorization)
    return this.http.delete<any>(this.editRecipePath + id, {headers});
  }

  deleteRecipeLog(data) {
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken()};
    return this.http.put<any>(this.deleteRecipeLogPath, data, { headers });
  }
}

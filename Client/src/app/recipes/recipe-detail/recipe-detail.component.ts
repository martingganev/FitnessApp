import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Recipe } from 'src/app/models/Recipe';
import { RecipeService } from 'src/app/services/recipe.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-recipe-detail',
  templateUrl: './recipe-detail.component.html',
  styleUrls: ['./recipe-detail.component.css']
})
export class RecipeDetailComponent implements OnInit {
  recipe: Recipe;
  id: number;

  constructor(private recipeService: RecipeService,
              private toastr: ToastrService,
              private authService: AuthService,
              private router: Router,
              @Inject(MAT_DIALOG_DATA) data) {
    this.id = data.id;
    this.recipeService.getRecipe(this.id).subscribe(
      recipe => {
        this.recipe = recipe;
      }
    )
  }

  ngOnInit() {
  }

  IsAuthenticated(): boolean{
    return this.authService.isAuthenticated();
  }

  edit(id){
    this.router.navigate([`/recipes/edit/${id}`,]);
  }

  delete(id){
    this.recipeService.deleteRecipe(id).subscribe(res => {
      this.toastr.success("Successfully deleted recipe.");
    })
  }

  addRecipeToUser(id: number){
    this.recipeService.addRecipeToUser(id).subscribe(
      res => {
        console.log("WE DID IT");
        console.log(res);
        if(res.succeeded === true){
          this.toastr.success("Successfully logged recipe");
        } else{
          this.toastr.error("Something went wrong");
          console.log(res.error);
        }
      }
    )
  }
}

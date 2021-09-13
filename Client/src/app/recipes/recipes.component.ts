import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Recipe } from '../models/Recipe';
import { AuthService } from '../services/auth.service';
import { RecipeService } from '../services/recipe.service';
import { RecipeDetailComponent } from './recipe-detail/recipe-detail.component';

@Component({
  selector: 'app-recipes',
  templateUrl: './recipes.component.html',
  styleUrls: ['./recipes.component.css']
})
export class RecipesComponent implements OnInit {

  allRecipes: Array<Recipe>;
  shownRecipes: Array<Recipe>;
  shouldShowOnlyAdmin: boolean = false;
  shouldShowOnlyMine: boolean = false;
  shouldShowOnlyQuick: boolean = false;
  shouldShowOnlyMedium: boolean = false;
  shouldShowOnlySlow: boolean = false;
  constructor(
    private dialog: MatDialog,
    private recipeService: RecipeService,
    private toastr: ToastrService,
    private authService: AuthService) { }

  ngOnInit(): void {
    this.recipeService.getRecipes().subscribe(recipes => {
      this.allRecipes = recipes;
      this.shownRecipes = recipes;
      console.log(this.allRecipes);
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

  IsAuthenticated(): boolean{
    return this.authService.isAuthenticated();
  }

  showAdmin(){
    this.shouldShowOnlyAdmin = !this.shouldShowOnlyAdmin;
    this.shouldShowOnlyMine = false;
    this.shouldShowOnlyMedium = false;
    this.shouldShowOnlyQuick = false;
    this.shouldShowOnlySlow = false;
    this.shownRecipes = this.shouldShowOnlyAdmin ? this.allRecipes.filter(r => r.isAdmin) : this.allRecipes;
  }

  showMine(){
    this.shouldShowOnlyAdmin = false;
    this.shouldShowOnlyMine = !this.shouldShowOnlyMine;
    this.shouldShowOnlyMedium = false;
    this.shouldShowOnlyQuick = false;
    this.shouldShowOnlySlow = false;
    this.shownRecipes = this.shouldShowOnlyMine ? this.allRecipes.filter(r => r.isMine) : this.allRecipes;
  }

  showQuickRecipes(){
    this.shouldShowOnlyAdmin = false;
    this.shouldShowOnlyMine = false;
    this.shouldShowOnlyQuick = !this.shouldShowOnlyQuick;
    this.shouldShowOnlyMedium = false;
    this.shouldShowOnlySlow = false;
    this.shownRecipes = this.shouldShowOnlyQuick ? this.allRecipes.filter(r => r.timeToFinish < 15) : this.allRecipes;
  }

  showMediumRecipes(){
    this.shouldShowOnlyAdmin = false;
    this.shouldShowOnlyMine = false;
    this.shouldShowOnlyQuick = false;
    this.shouldShowOnlyMedium = !this.shouldShowOnlyMedium;
    this.shouldShowOnlySlow = false;
    this.shownRecipes = this.shouldShowOnlyMedium ? this.allRecipes.filter(r => r.timeToFinish >= 15 && r.timeToFinish < 45) : this.allRecipes;
  }

  showSlowRecipes(){
    this.shouldShowOnlyAdmin = false;
    this.shouldShowOnlyMine = false;
    this.shouldShowOnlyQuick = false;
    this.shouldShowOnlyMedium = false;
    this.shouldShowOnlySlow = !this.shouldShowOnlySlow;
    this.shownRecipes = this.shouldShowOnlySlow ? this.allRecipes.filter(r => r.timeToFinish >= 45) : this.allRecipes;
  }

  openDialog(id) {
    const dialogRef = this.dialog.open(RecipeDetailComponent, {data: {"id": id}});

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      this.recipeService.getRecipes().subscribe(recipes => {
        this.allRecipes = recipes;
        this.shownRecipes = recipes;
      })
    });
  }
}

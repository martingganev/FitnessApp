import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ProductListing } from 'src/app/models/ProductListing';
import { Recipe } from 'src/app/models/Recipe';
import { ProductService } from 'src/app/services/product.service';
import { RecipeService } from 'src/app/services/recipe.service';
import * as $ from 'jquery';
@Component({
  selector: 'app-recipe-edit',
  templateUrl: './recipe-edit.component.html',
  styleUrls: ['./recipe-edit.component.css']
})
export class RecipeEditComponent implements OnInit {

  recipe: Recipe;
  productsInRecipe: Array<string>;
  quantitiesInRecipe: Array<number>;
  editForm: FormGroup;
  products: FormArray;
  allProducts: Array<ProductListing>;
  productSelectionDivs: HTMLCollectionOf<Element>;

  constructor(private route: ActivatedRoute, private recipeService: RecipeService, private fb: FormBuilder, private productService: ProductService, private router: Router, private toastr: ToastrService) {
    this.productService.getProductsByNames().subscribe(products => {
      this.allProducts = products;
    });
   }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      console.log(params['id'])
      this.recipeService.getRecipe(+params['id']).subscribe(recipe =>{
        this.recipe = recipe;
        this.productsInRecipe = this.recipe.products.map(p => p.productName);
        this.quantitiesInRecipe = this.recipe.products.map(p => p.quantity);
        console.log(recipe);
        this.editForm = this.fb.group({
          'name': ['', Validators.required],
          'timeToFinish': ['', Validators.required],
          'difficulty': ['', Validators.required],
          'photo': ['', Validators.required],
          'description': ['', Validators.required],
          'notesAndTips': ['', Validators.required],
          'products': this.fb.array(this.createMultipleProductFields(this.recipe.products.length))
        });
        console.log(this.editForm.get('products')['controls']);
      });
    });
  }


  editRecipe(){
    let formValue = this.editForm.value;
    formValue['products'] = this.getAllSelectedProducts();
    formValue['difficulty'] = +$(`#difficultySelect option:selected`).val();
    console.log(this.editForm.value);
    this.recipeService.editRecipe(this.recipe.id, this.editForm.value).subscribe(res => {
      console.log(res);
      this.toastr.success("Successfully edited recipe.");
      this.router.navigate(["/recipes"]);
    });
  }

  createAnotherProductField(): FormGroup{
    return this.fb.group({
      'productId': '',
      'quantity': ''
    })
  }

  createMultipleProductFields(count: number): Array<FormGroup>{
    let fields = [];
    for(let i = 0; i < count; i++){
      fields.push(this.createAnotherProductField());
    }
    return fields;
  }

  addItem(): void {
    this.products = this.editForm.get('products') as FormArray;
    this.products.push(this.createAnotherProductField());
    console.log(this.editForm.value);
  }

  removeItem(i): void{
    this.products = this.editForm.get('products') as FormArray;
    this.products.removeAt(i);
  }

  setDifficultyValue(){
    let formValue = this.editForm.value;
    
  }

  getAllSelectedProducts(): Array<any>{
    this.productSelectionDivs = document.getElementsByClassName('productSelectionDiv');
    let productSelections:  Array<any> = new Array<any>();
    for(let i = 0; i < this.productSelectionDivs.length; i++){
      let selectValue = $(`#productSelectionSelect${i} option:selected`).val();
      let inputValue = $(`#productSelectionInput${i}`).val();
      if(selectValue !== "" && inputValue !== ""){
        productSelections.push({productId: selectValue, quantity: inputValue});
      }
    }
    console.log(JSON.stringify(productSelections));
    return productSelections;
  }

  get name() {
    return this.editForm.get('name');
  }

  get timeToFinish() {
    return this.editForm.get('timeToFinish');
  }

  get difficulty() {
    return this.editForm.get('difficulty');
  }

  get photo() {
    return this.editForm.get('photo');
  }

  get description() {
    return this.editForm.get('description');
  }

  get notesAndTips() {
    return this.editForm.get('notesAndTips');
  }

}

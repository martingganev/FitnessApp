import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Recipe } from 'src/app/models/Recipe';
import { ToastrService } from 'ngx-toastr';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { ProductListing } from 'src/app/models/ProductListing';
import { ProductService } from 'src/app/services/product.service';
import * as $ from 'jquery';
import { RecipeService } from 'src/app/services/recipe.service';


@Component({
  selector: 'app-recipe-create',
  templateUrl: './recipe-create.component.html',
  styleUrls: ['./recipe-create.component.css']
})
export class RecipeCreateComponent implements OnInit {
  createForm: FormGroup;
  products: FormArray;
  allProducts: Array<ProductListing>;
  productSelectionDivs: HTMLCollectionOf<Element>;

  constructor(private fb: FormBuilder, private productService: ProductService, private recipeService: RecipeService, private router: Router, private toastr: ToastrService) {
    this.createForm = this.fb.group({
      'name': ['', Validators.required],
      'timeToFinish': ['', Validators.required],
      'difficulty': ['', Validators.required],
      'photo': ['', Validators.required],
      'description': ['', Validators.required],
      'notesAndTips': ['', Validators.required],
      'products': this.fb.array([this.createAnotherProductField()])
    })

    this.productService.getProductsByNames().subscribe(products => {
      console.log(products);
      this.allProducts = products;
    });
  }

  ngOnInit() {

  }

  createRecipe(){
    let formValue = this.createForm.value;
    formValue['products'] = this.getAllSelectedProducts();
    formValue['difficulty'] = +formValue['difficulty'];
    console.log(this.createForm.value);
    this.recipeService.createRecipe(formValue).subscribe(cb => {
      this.toastr.success("Successfully created recipe");
    })
  }

  createAnotherProductField(): FormGroup{
    return this.fb.group({
      'productId': '',
      'quantity': ''
    })
  }

  addItem(): void {
    this.products = this.createForm.get('products') as FormArray;
    this.products.push(this.createAnotherProductField());
  }

  removeItem(i): void{
    this.products = this.createForm.get('products') as FormArray;
    this.products.removeAt(i);
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

  isInvalidForm(): boolean {
    return this.name.errors?.required ||
      this.notesAndTips.errors?.required || 
      this.timeToFinish.errors?.required ||
      this.difficulty.errors?.required ||
      this.photo.errors?.required ||
      this.description.errors?.required ||
      this.getAllSelectedProducts().length === 0
  }

  get name() {
    return this.createForm.get('name');
  }

  get timeToFinish() {
    return this.createForm.get('timeToFinish');
  }

  get difficulty() {
    return this.createForm.get('difficulty');
  }

  get photo() {
    return this.createForm.get('photo');
  }

  get description() {
    return this.createForm.get('description');
  }

  get notesAndTips() {
    return this.createForm.get('notesAndTips');
  }

}

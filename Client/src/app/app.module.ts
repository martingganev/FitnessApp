import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from './services/auth.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthGuardService } from './services/auth-guard.service';
import { TokenInterceptorService } from './services/token-interceptor.service';
import { ErrorInterceptorService } from './services/error-interceptor.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { HeaderComponent } from './header/header.component';
import { RecipesComponent } from './recipes/recipes.component';
import { WorkoutsComponent } from './workouts/workouts.component';
import { RecipeDetailComponent } from './recipes/recipe-detail/recipe-detail.component';
import { RecipeService } from './services/recipe.service';
import { RouterModule } from '@angular/router';
import { WorkoutDetailComponent } from './workouts/workout-detail/workout-detail.component';
import { WorkoutService } from './services/workout.service';
import { CommonModule } from '@angular/common';
import { LogoutComponent } from './logout/logout.component';
import { HomepageComponent } from './homepage/homepage.component';
import { MatDialogModule } from '@angular/material/dialog';
import { ChartsModule } from 'ng2-charts';
import { UserService } from './services/user.service';
import { RecipeCreateComponent } from './recipes/recipe-create/recipe-create.component';
import { ProductService } from './services/product.service';
import { WorkoutCreateComponent } from './workouts/workout-create/workout-create.component';
import { ExerciseService } from './services/exercise.service';
import { PersonalInfoComponent } from './profile/personal-info/personal-info.component';
import { ChangePasswordComponent } from './profile/change-password/change-password.component';
import { RecipeEditComponent } from './recipes/recipe-edit/recipe-edit.component';
import { WorkoutEditComponent } from './workouts/workout-edit/workout-edit.component';


@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    LoginComponent,
    LogoutComponent,
    HeaderComponent,
    RecipesComponent,
    WorkoutsComponent,
    RecipeDetailComponent,
    RecipeCreateComponent,
    RecipeEditComponent,
    WorkoutCreateComponent,
    WorkoutDetailComponent,
    WorkoutEditComponent,
    HomepageComponent,
    PersonalInfoComponent,
    ChangePasswordComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatDialogModule,
    CommonModule,
    BrowserAnimationsModule,
    ChartsModule,
    RouterModule,
    ToastrModule.forRoot()
  ],
  exports: [RouterModule, CommonModule],
  providers: [
    AuthService,
    AuthGuardService,
    RecipeService,
    ExerciseService,
    UserService,
    ProductService,
    WorkoutService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptorService,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptorService,
      multi: true
    }
],
  bootstrap: [AppComponent]
})
export class AppModule { }

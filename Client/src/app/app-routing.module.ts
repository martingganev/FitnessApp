import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';
import { HomepageComponent } from './homepage/homepage.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { ChangePasswordComponent } from './profile/change-password/change-password.component';
import { PersonalInfoComponent } from './profile/personal-info/personal-info.component';
import { RecipeCreateComponent } from './recipes/recipe-create/recipe-create.component';
import { RecipeDetailComponent } from './recipes/recipe-detail/recipe-detail.component';
import { RecipeEditComponent } from './recipes/recipe-edit/recipe-edit.component';
import { RecipesComponent } from './recipes/recipes.component';
import { RegisterComponent } from './register/register.component';
import { AuthGuardService } from './services/auth-guard.service';
import { WorkoutCreateComponent } from './workouts/workout-create/workout-create.component';
import { WorkoutDetailComponent } from './workouts/workout-detail/workout-detail.component';
import { WorkoutEditComponent } from './workouts/workout-edit/workout-edit.component';
import { WorkoutsComponent } from './workouts/workouts.component';


const routes: Routes = [
  { path: '', component: HomepageComponent},
  { path: 'register', component: RegisterComponent},
  { path: 'login', component: LoginComponent },
  { path: 'logout', component: LogoutComponent, canActivate: [AuthGuardService]},
  { path: 'profile/changePassword', component: ChangePasswordComponent, canActivate: [AuthGuardService]},
  { path: 'profile/personalInfo', component: PersonalInfoComponent, canActivate: [AuthGuardService]},
  { path: 'recipes/create', component: RecipeCreateComponent, canActivate: [AuthGuardService] },
  { path: 'recipes/edit/:id', component: RecipeEditComponent },
  { path: 'recipes', component: RecipesComponent, 
    children: [
      { path: '', component: RecipesComponent },
      { path: ':id', component: RecipeDetailComponent }
    ]
  },
  { path: 'workouts/create', component: WorkoutCreateComponent, canActivate: [AuthGuardService]},
  { path: 'workouts/edit/:id', component: WorkoutEditComponent },
  { path: 'workouts', component: WorkoutsComponent,
    children: [
      { path: '', component: WorkoutsComponent },
      { path: ':id', component: WorkoutDetailComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

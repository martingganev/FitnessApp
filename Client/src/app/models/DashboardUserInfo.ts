import { DashboardLoggedRecipe } from "./DashboardLoggedRecipe";
import { DashboardLoggedWorkout } from "./DashboardLoggedWorkout";

export class DashboardUserInfo{

    public targetCalories : number;
    public currentCalories : number;
    public currentProteins : number;
    public currentCarbs : number;
    public currentFats : number;
    public targetProteins : number;
    public targetCarbs : number;
    public targetFats : number;
    public currentSugars : number;
    public currentSodium : number;
    public burnedCalories : number;
    public timeSpentTraining : number;
    public isRestDay : boolean;
    public isSuccess : boolean;
    public error : string;
    public loggedRecipes: Array<DashboardLoggedRecipe>;
    public loggedWorkouts: Array<DashboardLoggedWorkout>;
}
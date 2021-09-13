import { Component, OnInit } from '@angular/core';
import { ChartOptions, ChartType } from 'chart.js';
import { MultiDataSet, Label, PluginServiceGlobalRegistrationAndOptions, Color } from 'ng2-charts';
import { DashboardUserInfo } from '../models/DashboardUserInfo';
import { UserService } from '../services/user.service';
import * as moment from 'moment';
import { RecipeService } from '../services/recipe.service';
import { RecipeByName } from '../models/RecipeByName';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { WorkoutByName } from '../models/WorkoutByName';
import { WorkoutService } from '../services/workout.service';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css']
})
export class HomepageComponent implements OnInit {

  quickAddRecipeForm: FormGroup;
  quickAddWorkoutForm: FormGroup;
  recipesByNames: Array<RecipeByName>;
  workoutsByNames: Array<WorkoutByName>;
  dashboardUserInfo: DashboardUserInfo;
  hasMetTarget: boolean;
  currentDate: moment.Moment;
  constructor(public userService: UserService, public recipeService: RecipeService, public workoutService: WorkoutService, private fb: FormBuilder, private toastr: ToastrService) {}
  public doughnutChartLabels: Label[] = ['Current Proteins', 'Remaining Proteins'];
  public doughnutChartData: MultiDataSet = [];
  public doughnutChartType: ChartType = 'doughnut';
  public doughnutChartColors: Color[] = 
  [
      {
          backgroundColor: ['rgba(69, 130, 165, 0.61)', '#C4C4C4']
      }
  ]

  ngOnInit() {
    this.quickAddRecipeForm = this.fb.group({
      'id': ['', Validators.required]
    });
    this.quickAddWorkoutForm = this.fb.group({
      'id': ['', Validators.required]
    });
    this.recipeService.getRecipesByNames().subscribe(recipes => {
      this.recipesByNames = recipes;
      console.log(recipes);
    });
    this.workoutService.getWorkoutsByNames().subscribe(workouts => {
      this.workoutsByNames = workouts;
      console.log(workouts);
    });
    this.currentDate = moment(new Date(), 'DD-MM');
    console.log(this.currentDate.format('DD-MM-YYYY'))
    this.loadDashboardInfo();
  }

  loadDashboardInfo(){
    this.userService.getDashboardInfo().subscribe(data => {
      console.log(data);
      this.dashboardUserInfo = data;
      if(this.dashboardUserInfo.currentProteins >= this.dashboardUserInfo.targetProteins){
        this.hasMetTarget = true;
        this.doughnutChartLabels = [ 'Current Proteins' ]
        this.doughnutChartData = [[this.dashboardUserInfo.currentProteins]];
        this.doughnutChartColors = [ { backgroundColor: 'rgb(130,194,79,1)'} ]
      } else{
        this.hasMetTarget = false;
        this.doughnutChartData = [[this.dashboardUserInfo.currentProteins, this.dashboardUserInfo.targetProteins - this.dashboardUserInfo.currentProteins]];
      }
    });
  }

  addRecipeToUser(){
    console.log(this.quickAddRecipeForm.value['id']);
    this.recipeService.addRecipeToUser(+this.quickAddRecipeForm.value['id']).subscribe(res => {
      this.toastr.success('Successfully added recipe.')
    })
    this.quickAddRecipeForm = this.fb.group({
      'id': ['', Validators.required]
    });
    this.loadDashboardInfo();
  }

  removeRecipeLog(recipeId: number, dateLogged: Date){
    console.log(recipeId);
    console.log(dateLogged);
    this.recipeService.deleteRecipeLog({recipeId: recipeId, dateLogged: dateLogged}).subscribe(res => {
      this.toastr.success('Successfully deleted recipe log.');
      this.loadDashboardInfo();
    });
  }

  removeWorkoutLog(workoutId: number, dateLogged: Date){
    console.log(workoutId);
    console.log(dateLogged);
    this.workoutService.deleteWorkoutLog({workoutId: workoutId, dateLogged: dateLogged}).subscribe(res => {
      this.toastr.success('Successfully deleted workout log.');
      this.loadDashboardInfo();
    });
  }

  addWorkoutToUser(){
    console.log(this.quickAddWorkoutForm.value['id']);
    this.workoutService.addWorkoutToUser(+this.quickAddWorkoutForm.value['id']).subscribe(res => {
      this.toastr.success('Successfully added workout.')
    })
    this.quickAddWorkoutForm = this.fb.group({
      'id': ['', Validators.required]
    });
    this.loadDashboardInfo();
  }

  public doughnutChartPlugins: PluginServiceGlobalRegistrationAndOptions[] = [{
    afterDraw(chart) {
      this.doughnutChart = chart;
      const ctx = chart.ctx;
      var text = "";
      ctx.textAlign = 'center';
      ctx.textBaseline = 'middle';
      const centerX = ((chart.chartArea.left + chart.chartArea.right) / 2);
      const centerY = ((chart.chartArea.top + chart.chartArea.bottom) / 2);

      const fontSizeToUse = 40;
      ctx.font = fontSizeToUse + 'px Montserrat';
      ctx.fillStyle = 'black';

      ctx.fillText(text, centerX, centerY - 10);
      ctx.font = fontSizeToUse + 'px Montserrat';
    },
    beforeDraw(chart){
      //TODO: make this work!
      
    }
  }]

  public chartClicked({ event, active }: { event: MouseEvent, active: {}[] }): void {
    console.log(event, active);
  }

  public chartHovered({ event, active }: { event: MouseEvent, active: {}[] }): void {
    console.log(event, active);
  }

  get isRecipeIdPicked() {
    return this.quickAddRecipeForm.value['id'] !== '';
  }

  get isWorkoutIdPicked() {
    return this.quickAddWorkoutForm.value['id'] !== '';
  }

}

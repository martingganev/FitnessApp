import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Workout } from '../models/Workout';
import { AuthService } from '../services/auth.service';
import { WorkoutService } from '../services/workout.service';
import { WorkoutDetailComponent } from './workout-detail/workout-detail.component';

@Component({
  selector: 'app-workouts',
  templateUrl: './workouts.component.html',
  styleUrls: ['./workouts.component.css']
})
export class WorkoutsComponent implements OnInit {

  allWorkouts: Array<Workout>;
  shownWorkouts: Array<Workout>;
  shouldShowOnlyAdmin: boolean = false;
  shouldShowOnlyMine: boolean = false;
  shouldShowOnlyQuick: boolean = false;
  shouldShowOnlyMedium: boolean = false;
  shouldShowOnlySlow: boolean = false;

  constructor(
    private workoutService: WorkoutService,
    private dialog: MatDialog,
    private toastr: ToastrService,
    private authService: AuthService) { }

  ngOnInit(): void {
    this.workoutService.getWorkouts().subscribe(workouts => {
      this.allWorkouts = workouts;
      this.shownWorkouts = workouts;
      console.log(this.allWorkouts);
    })
  }

  addWorkoutToUser(id: number){
    this.workoutService.addWorkoutToUser(id).subscribe(
      res => {
        console.log("WE DID IT");
        console.log(res);
        if(res.succeeded === true){
          this.toastr.success("Successfully logged workout");
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
    this.shownWorkouts = this.shouldShowOnlyAdmin ? this.allWorkouts.filter(r => r.isAdmin) : this.allWorkouts;
  }

  showMine(){
    this.shouldShowOnlyAdmin = false;
    this.shouldShowOnlyMine = !this.shouldShowOnlyMine;
    this.shouldShowOnlyQuick = false;
    this.shouldShowOnlyMedium = false;
    this.shouldShowOnlySlow = false;
    this.shownWorkouts = this.shouldShowOnlyMine ? this.allWorkouts.filter(r => r.isMine) : this.allWorkouts;
  }

  showQuickWorkouts(){
    this.shouldShowOnlyAdmin = false;
    this.shouldShowOnlyMine = false;
    this.shouldShowOnlyQuick = !this.shouldShowOnlyQuick;
    this.shouldShowOnlyMedium = false;
    this.shouldShowOnlySlow = false;
    this.shownWorkouts = this.shouldShowOnlyQuick ? this.allWorkouts.filter(r => r.timeToFinish < 15) : this.allWorkouts;
  }

  showMediumWorkouts(){
    this.shouldShowOnlyAdmin = false;
    this.shouldShowOnlyMine = false;
    this.shouldShowOnlyQuick = false;
    this.shouldShowOnlyMedium = !this.shouldShowOnlyMedium;
    this.shouldShowOnlySlow = false;
    this.shownWorkouts = this.shouldShowOnlyMedium ? this.allWorkouts.filter(r => r.timeToFinish >= 15 && r.timeToFinish < 45) : this.allWorkouts;
  }

  showSlowWorkouts(){
    this.shouldShowOnlyAdmin = false;
    this.shouldShowOnlyMine = false;
    this.shouldShowOnlyQuick = false;
    this.shouldShowOnlyMedium = false;
    this.shouldShowOnlySlow = !this.shouldShowOnlySlow;
    this.shownWorkouts = this.shouldShowOnlySlow ? this.allWorkouts.filter(r => r.timeToFinish >= 45) : this.allWorkouts;
  }

  openDialog(id) {
    const dialogRef = this.dialog.open(WorkoutDetailComponent, {data: {"id": id}});

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      this.workoutService.getWorkouts().subscribe(workouts => {
        this.allWorkouts = workouts;
        this.shownWorkouts = workouts;
      });
    });
  }

}

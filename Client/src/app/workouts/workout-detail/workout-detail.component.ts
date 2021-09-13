import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Workout } from 'src/app/models/Workout';
import { WorkoutService } from 'src/app/services/workout.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-workout-detail',
  templateUrl: './workout-detail.component.html',
  styleUrls: ['./workout-detail.component.css']
})
export class WorkoutDetailComponent implements OnInit {

  workout: Workout;
  id: number;

  constructor(private workoutService: WorkoutService,
              private toastr: ToastrService,
              private authService: AuthService,
              private router: Router,
              @Inject(MAT_DIALOG_DATA) data) {
    this.id = data.id
    this.workoutService.getWorkout(this.id).subscribe(
      workout => {
        this.workout = workout;
      }
    );
  }

  ngOnInit() {
    
  }

  IsAuthenticated(): boolean{
    return this.authService.isAuthenticated();
  }

  edit(id){
    this.router.navigate([`/workouts/edit/${id}`,]);
  }

  delete(id){
    this.workoutService.deleteWorkout(id).subscribe(res => {
      this.toastr.success("Successfully deleted workout.");
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
}


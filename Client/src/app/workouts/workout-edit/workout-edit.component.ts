import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ExerciseListing } from 'src/app/models/ExerciseListing';
import { Workout } from 'src/app/models/Workout';
import { ExerciseService } from 'src/app/services/exercise.service';
import { WorkoutService } from 'src/app/services/workout.service';
import * as $ from 'jquery';

@Component({
  selector: 'app-workout-edit',
  templateUrl: './workout-edit.component.html',
  styleUrls: ['./workout-edit.component.css']
})
export class WorkoutEditComponent implements OnInit {

  workout: Workout;
  exercisesInWorkout: Array<string>;
  editForm: FormGroup;
  exercises: FormArray;
  allExercises: Array<ExerciseListing>;
  exerciseSelectionDivs: HTMLCollectionOf<Element>;
  
  constructor(
    private fb: FormBuilder,
    private workoutService: WorkoutService,
    private exerciseService: ExerciseService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
    ) {
    this.exerciseService.getExercisesByNames().subscribe(exercises => {
      console.log(exercises);
      this.allExercises = exercises;
    });
   }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      console.log(params['id'])
      this.workoutService.getWorkout(+params['id']).subscribe(workout =>{
        this.workout = workout;
        this.exercisesInWorkout = this.workout.exercises.map(p => p.name);
        console.log(this.exercisesInWorkout);
        console.log(workout);
        this.editForm = this.fb.group({
          'name': ['', Validators.required],
          'timeToFinish': ['', Validators.required],
          'difficulty': ['', Validators.required],
          'photo': ['', Validators.required],
          'description': ['', Validators.required],
          'caloriesBurned': ['', Validators.required],
          'exercises': this.fb.array(this.createMultipleExerciseFields(this.workout.exercises.length))
        })
        console.log(this.editForm.get('exercises')['controls']);
      });
    });
  }

  editWorkout(){
    let formValue = this.editForm.value;
    formValue['exercises'] = this.getAllSelectedExercises();
    formValue['difficulty'] = +$(`#difficultySelect option:selected`).val();
    console.log(this.editForm.value);
    this.workoutService.editWorkout(this.workout.id, this.editForm.value).subscribe(res => {
      console.log(res);
      this.toastr.success("Successfully edited workout.");
      this.router.navigate(["/workouts"]);
    });
  }

  createAnotherExerciseField(): FormGroup{
    return this.fb.group({
      'exerciseId': '',
      'sets': '',
      'repetitions': ''
    })
  }

  createMultipleExerciseFields(count: number): Array<FormGroup>{
    let fields = [];
    for(let i = 0; i < count; i++){
      fields.push(this.createAnotherExerciseField());
    }
    return fields;
  }

  addItem(): void {
    this.exercises = this.editForm.get('exercises') as FormArray;
    this.exercises.push(this.createAnotherExerciseField());
  }

  removeItem(i): void{
    this.exercises = this.editForm.get('exercises') as FormArray;
    this.exercises.removeAt(i);
  }

  getAllSelectedExercises(): Array<any>{
    this.exerciseSelectionDivs = document.getElementsByClassName('exerciseSelectionDiv');
    let exerciseSelections:  Array<any> = new Array<any>();
    for(let i = 0; i < this.exerciseSelectionDivs.length; i++){
      exerciseSelections.push({exerciseId: $(`#exerciseSelectionSelect${i} option:selected`).val(), sets: $(`#exerciseSelectionInputSets${i}`).val(), repetitions: $(`#exerciseSelectionInputReps${i}`).val()});
    }
    console.log(JSON.stringify(exerciseSelections));
    return exerciseSelections;
  }

  isInvalidForm(): boolean{
    return this.name.errors?.required ||
      this.caloriesBurned.errors?.required || 
      this.timeToFinish.errors?.required ||
      this.difficulty.errors?.required ||
      this.photo.errors?.required ||
      this.description.errors?.required ||
      this.getAllSelectedExercises().length === 0
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

  get caloriesBurned() {
    return this.editForm.get('caloriesBurned');
  }

}

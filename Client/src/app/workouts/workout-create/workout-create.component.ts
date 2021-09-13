import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Workout } from 'src/app/models/Workout';
import { WorkoutService } from 'src/app/services/workout.service';
import { ToastrService } from 'ngx-toastr';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ExerciseService } from 'src/app/services/exercise.service';
import { ExerciseListing } from 'src/app/models/ExerciseListing';
import * as $ from 'jquery';
@Component({
  selector: 'app-workout-create',
  templateUrl: './workout-create.component.html',
  styleUrls: ['./workout-create.component.css']
})
export class WorkoutCreateComponent implements OnInit {

  createForm: FormGroup;
  exercises: FormArray;
  allExercises: Array<ExerciseListing>;
  exerciseSelectionDivs: HTMLCollectionOf<Element>;

  constructor(
    private fb: FormBuilder,
    private workoutService: WorkoutService,
    private exerciseService: ExerciseService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService) {

      this.createForm = this.fb.group({
        'name': ['', Validators.required],
        'timeToFinish': ['', Validators.required],
        'difficulty': ['', Validators.required],
        'photo': ['', Validators.required],
        'description': ['', Validators.required],
        'caloriesBurned': ['', Validators.required],
        'exercises': this.fb.array([this.createAnotherExerciseField()])
      })

      this.exerciseService.getExercisesByNames().subscribe(exercises => {
        console.log(exercises);
        this.allExercises = exercises;
      });
  }

  ngOnInit() {
    
  }

  createWorkout(){
    let formValue = this.createForm.value;
    formValue['exercises'] = this.getAllSelectedExercises();
    formValue['difficulty'] = +formValue['difficulty'];
    console.log(this.createForm.value);
    this.workoutService.createWorkout(formValue).subscribe(cb => {
      this.toastr.success("Successfully created workout");
    })
  }

  createAnotherExerciseField(): FormGroup{
    return this.fb.group({
      'exerciseId': '',
      'sets': '',
      'repetitions': ''
    })
  }

  addItem(): void {
    this.exercises = this.createForm.get('exercises') as FormArray;
    this.exercises.push(this.createAnotherExerciseField());
    this.getAllSelectedExercises();
  }

  removeItem(i): void{
    this.exercises = this.createForm.get('exercises') as FormArray;
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

  get caloriesBurned() {
    return this.createForm.get('caloriesBurned');
  }

}


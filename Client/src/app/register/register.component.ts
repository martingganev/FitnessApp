import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

declare function nextPrev(n): void;
declare function showTab(n): void;
declare function initializeRegisterScript(): void;
declare function hideSubmitBtn(): void;
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.registerForm = this.fb.group({
      'username': ['', Validators.required],
      'email': ['', Validators.required, Validators.email],
      'password': ['', Validators.required],
      'height': ['', Validators.required],
      'weight': ['', Validators.required],
      'gender': ['', Validators.required],
      'dailyCalorieGoal': ['', Validators.required],
      'trainingGoal': ['', Validators.required],
      'workoutSchedule': ['', Validators.required],
    })
   }

  ngOnInit() {
    document.body.style.backgroundImage = "url('../../assets/background-image-girl.png')";
    initializeRegisterScript();
    hideSubmitBtn();
    showTab(0);
  }

  ngOnDestroy(): void {
    document.body.style.backgroundImage = "";
  }

  register() {
    this.registerForm.get("gender").setValue(Number(this.gender.value));
    this.authService.register(this.registerForm.value).subscribe(data => {
      this.router.navigate(["/login"]);
    })
  }

  nextPrevMethod(n)
  {
    nextPrev(n);
  }

  get username() {
    return this.registerForm.get('username')
  }

  get password() {
    return this.registerForm.get('password')
  }

  get height() {
    return this.registerForm.get('height')
  }

  get weight() {
    return this.registerForm.get('weight')
  }

  get gender() {
    return this.registerForm.get('gender')
  }
  
  get dailyCalorieGoal() {
    return this.registerForm.get('dailyCalorieGoal')
  }

  get trainingGoal() {
    return this.registerForm.get('trainingGoal')
  }

  get workoutSchedule() {
    return this.registerForm.get('workoutSchedule')
  }
}

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserPersonalInfo } from 'src/app/models/UserPersonalInfo';
import { UserService } from 'src/app/services/user.service';
import * as $ from 'jquery';
import { AuthService } from 'src/app/services/auth.service';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-personal-info',
  templateUrl: './personal-info.component.html',
  styleUrls: ['./personal-info.component.css']
})
export class PersonalInfoComponent implements OnInit {

  personalInfo: UserPersonalInfo;
  updateUserInfoForm: FormGroup;

  constructor(private userService: UserService, private fb: FormBuilder, private authService: AuthService, private toastr: ToastrService) { 
    
    this.updateUserInfoForm = this.fb.group({
      'username': ['', Validators.required],
      'email': ['', Validators.required, Validators.email],
      'height': ['', Validators.required],
      'weight': ['', Validators.required],
      'gender': ['', Validators.required],
      'dailyCalorieGoal': ['', Validators.required],
      'trainingGoal': ['', Validators.required],
      'workoutSchedule': ['', Validators.required]
    });

    this.userService.getPesonalInfo().subscribe(data => {
      this.personalInfo = data;
      console.log(data);
    });
   }

  ngOnInit(): void {
  }

  updateUserInfo(){
    if(this.workoutSchedule.value === ""){
      this.parseWorkoutSchedule();
    }
    this.updateUserInfoForm.get("gender").setValue(Number(this.gender.value));
    console.log(this.updateUserInfoForm.value);
    this.userService.updatePesonalInfo(this.updateUserInfoForm.value).subscribe(res => {
      console.log(res);
        if(res.isSuccess === true){
          this.authService.saveToken(res.token)
          this.toastr.success("Successfully updated your info!");
        } else{
          this.toastr.error(`Update not successful: ${res.errors}`);
        }
    });
  }

  parseWorkoutSchedule(){
    var data=[];
    var $el=$("#workoutScheduleSelect");
    $el.find('option:selected').each(function(){
        data.push($(this).val().toString().split(': ')[1].split(`'`)[1]);
    });
    console.log(data)
    this.updateUserInfoForm.get('workoutSchedule').setValue(data);
  }

  get username() {
    return this.updateUserInfoForm.get('username')
  }

  get email() {
    return this.updateUserInfoForm.get('email')
  }

  get height() {
    return this.updateUserInfoForm.get('height')
  }

  get weight() {
    return this.updateUserInfoForm.get('weight')
  }

  get gender() {
    return this.updateUserInfoForm.get('gender')
  }
  
  get dailyCalorieGoal() {
    return this.updateUserInfoForm.get('dailyCalorieGoal')
  }

  get trainingGoal() {
    return this.updateUserInfoForm.get('trainingGoal')
  }

  get workoutSchedule() {
    return this.updateUserInfoForm.get('workoutSchedule')
  }

}

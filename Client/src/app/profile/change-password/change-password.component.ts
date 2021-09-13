import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  changePasswordForm: FormGroup;

  constructor(private userService: UserService, private fb: FormBuilder, private authService: AuthService, private toastr: ToastrService, private router: Router) { 
    this.changePasswordForm = this.fb.group({
      'oldPassword': ['', Validators.required],
      'newPassword': ['', Validators.required],
      'newPasswordConfirm': ['', Validators.required],
    });
  }

  matchValues(matchTo: string): (AbstractControl) => ValidationErrors | null {
    return (control: AbstractControl): ValidationErrors | null => {
      return !!control.parent &&
        !!control.parent.value &&
        control.value === control.parent.controls[matchTo].value
        ? null
        : { isMatching: false };
    };
  }

  ngOnInit(): void {
  }

  changePassword(){
    console.log(this.newPasswordConfirm.errors);
    this.userService.changePassword(this.changePasswordForm.value).subscribe(res => {
      console.log(res);
        if(res.isSuccess === true){
          this.authService.saveToken(res.token)
          this.toastr.success("Successfully changed your password!");
          this.router.navigate([""]);
        } else{
          this.toastr.error(`Password change unsuccessful: ${res.errors}`);
        }
    });
  }

  get oldPassword() {
    return this.changePasswordForm.get('oldPassword')
  }

  get newPassword() {
    return this.changePasswordForm.get('newPassword')
  }

  get newPasswordConfirm() {
    return this.changePasswordForm.get('newPasswordConfirm')
  }

  get NewPasswordsMatch(){
    return this.newPassword.value === this.newPasswordConfirm.value;
  }

  get IsValidForm() {
    return this.oldPassword.touched && this.oldPassword.errors === null && this.newPassword.touched && this.newPassword.errors === null && this.newPasswordConfirm.touched && this.newPasswordConfirm.errors === null && this.NewPasswordsMatch;
  }
}

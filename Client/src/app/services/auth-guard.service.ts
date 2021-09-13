import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) { }

  canActivate(): boolean {
    if (this.authService.isAuthenticated()) {
      return true
    } else {
      this.toastr.warning("You should be logged in to access this page!")
      this.router.navigate(["/login"]);
      return false;
    }
  }
}

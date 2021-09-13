import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { DashboardUserInfo } from '../models/DashboardUserInfo';
import { UserPersonalInfo } from '../models/UserPersonalInfo';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private dashboardInfoPath = environment.apiUrl + "/Identity/GetDashboardInformationForUser";
  private getUserPersonalInfoPath = environment.apiUrl + "/Identity/GetUserPersonalInfo";
  private updateUserPersonalInfoPath = environment.apiUrl + "/Identity/UpdateUserPersonalInfo";
  private changePasswordPath = environment.apiUrl + "/Identity/ChangePassword";

  constructor(private http: HttpClient, private authService: AuthService) { }

  getDashboardInfo(): Observable<DashboardUserInfo> {
    return this.http.get<DashboardUserInfo>(this.dashboardInfoPath)
  }

  getPesonalInfo(): Observable<UserPersonalInfo> {
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken()};
    return this.http.get<UserPersonalInfo>(this.getUserPersonalInfoPath, { headers });
  }

  updatePesonalInfo(data): Observable<any> {
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken()};
    return this.http.post<any>(this.updateUserPersonalInfoPath, data, { headers });
  }

  changePassword(data): Observable<any> {
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken()};
    return this.http.post<any>(this.changePasswordPath, data, { headers });
  }
}

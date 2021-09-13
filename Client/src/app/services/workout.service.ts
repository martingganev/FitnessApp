import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Workout } from '../models/Workout';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { WorkoutByName } from '../models/WorkoutByName';


@Injectable()
export class WorkoutService {
  private allWorkoutsPath = environment.apiUrl + "/Workouts/AllWorkouts";
  private getWorkoutPath = environment.apiUrl + "/Workouts/";
  private addWorkoutToUserPath = environment.apiUrl + "/Workouts/AddWorkoutToUser";
  private createWorkoutPath = environment.apiUrl + "/Workouts/Create";
  private editWorkoutPath = environment.apiUrl + "/Workouts/";
  private allWorkoutsByNamesPath = environment.apiUrl + "/Workouts/AllWorkoutsByNames";
  private deleteWorkoutLogPath = environment.apiUrl + "/Workouts/DeleteWorkoutLog";

  constructor(private http: HttpClient, private authService: AuthService) { }

  getWorkouts(): Observable<Array<Workout>> {
    return this.http.get<Array<Workout>>(this.allWorkoutsPath);
  }

  getWorkout(id: number): Observable<Workout> {
    return this.http.get<Workout>(this.getWorkoutPath + id);
  }

  getWorkoutsByNames(): Observable<Array<WorkoutByName>> {
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken()};
    console.log("bearer: " + headers.Authorization)
    return this.http.get<Array<WorkoutByName>>(this.allWorkoutsByNamesPath, {headers});
  }

  addWorkoutToUser(id: number) {
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken()};
    console.log("bearer: " + headers.Authorization)
    return this.http.post<any>(this.addWorkoutToUserPath, {"id": id}, { headers });
  }

  createWorkout(data) {
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken()};
    console.log("bearer: " + headers.Authorization)
    return this.http.post<any>(this.createWorkoutPath, data, {headers});
  }

  editWorkout(id, data) {
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken()};
    console.log("bearer: " + headers.Authorization)
    return this.http.put<any>(this.editWorkoutPath + id, data, {headers});
  }

  deleteWorkout(id) {
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken()};
    console.log("bearer: " + headers.Authorization)
    return this.http.delete<any>(this.editWorkoutPath + id, {headers});
  }

  deleteWorkoutLog(data) {
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken()};
    console.log("bearer: " + headers.Authorization)
    return this.http.put<any>(this.deleteWorkoutLogPath, data, { headers });
  }
}

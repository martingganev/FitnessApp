import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ExerciseListing } from '../models/ExerciseListing';

@Injectable()
export class ExerciseService {

  private allExercisesByNamesPath = environment.apiUrl + "/Exercises/AllExercisesByNames";

  constructor(private http: HttpClient) { }

  getExercisesByNames(): Observable<Array<ExerciseListing>> {
    return this.http.get<Array<ExerciseListing>>(this.allExercisesByNamesPath);
  }
}

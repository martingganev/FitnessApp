import { Exercise } from './Exercise';

export class Workout {
    public id: number;
    public name: string;
    public timeToFinish: number;
    public difficulty: string;
    public photo: string;
    public description: string;
    public caloriesBurned: number;
    public isMine: boolean;
    public isAdmin: boolean;
    public exercises: Exercise[];
  
    constructor(name: string, desc: string, photo: string, exercises: Exercise[]) {
      this.name = name;
      this.description = desc;
      this.photo = photo;
      this.exercises = exercises;
    }
  }
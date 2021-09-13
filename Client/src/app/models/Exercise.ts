export class Exercise{

    public exerciseName: string;
    public sets: number;
    public repetitions: number;

    constructor(public name: string, public numberSets: number, public numberRepetitions: number) {
        this.exerciseName = name;
        this.sets = numberSets;
        this.repetitions = numberRepetitions;
    }
}
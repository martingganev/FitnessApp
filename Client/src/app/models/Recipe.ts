import { Product } from './Product';

export class Recipe {
    public id: number;
    public name: string;
    public timeToFinish: number;
    public difficulty: string;
    public totalCalories: DoubleRange;
    public photo: string;
    public description: string;
    public notesAndTips: string;
    public isMine: boolean;
    public isAdmin: boolean;
    public products: Product[];
  
    constructor(name: string, desc: string, photo: string, IsMine: boolean, products: Product[]) {
      this.name = name;
      this.description = desc;
      this.photo = photo;
      this.products = products;
      this.isMine = IsMine;
    }
  }
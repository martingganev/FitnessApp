export class Product{

    public productName: string;
    public quantity: number;

    constructor(public name: string, public amount: number) {
        this.productName = name;
        this.quantity = amount;
    }
}


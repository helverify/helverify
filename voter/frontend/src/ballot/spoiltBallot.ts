export class SpoiltBallot {
    options: PlainTextOption[];

    constructor(options: PlainTextOption[]) {
        this.options = options;
    }
}

export class PlainTextOption {
    name: string;
    shortCode: string;
    position: number;
    randomness: string[];

    constructor(name: string, shortCode: string, position: number, randomness: string[]) {
        this.name = name;
        this.shortCode = shortCode;
        this.position = position;
        this.randomness = randomness;
    }
}
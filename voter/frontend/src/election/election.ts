import bigInt from "big-integer";

export type ElectionParameters = {
    p: bigInt.BigInteger;
    g: bigInt.BigInteger;
    publicKey: bigInt.BigInteger;
    consensusNodePublicKeys: bigInt.BigInteger[];
}

export class ElectionResults {
    results: OptionTally[];

    constructor(results: OptionTally[]) {
        this.results = results;
    }
}

export class OptionTally {
    name: string;
    count: number;

    constructor(name: string, count: number) {
        this.name = name;
        this.count = count;
    }
}
import bigInt from "big-integer";

/**
 * Represents an election's encryption parameters
 */
export type ElectionParameters = {

    /**
     * ElGamal parameter p
     */
    p: bigInt.BigInteger;

    /**
     * ElGamal parameter g
     */
    g: bigInt.BigInteger;

    /**
     * Election public key
     */
    publicKey: bigInt.BigInteger;

    /**
     * Public keys of all consensus nodes
     */
    consensusNodePublicKeys: bigInt.BigInteger[];
}

/**
 * Represents the final results of an election
 */
export class ElectionResults {

    /**
     * Name-value pairs representing the counts per option/candidate
     */
    results: OptionTally[];

    /**
     * Constructor
     * @param results Name-value pairs representing the counts per option/candidate
     */
    constructor(results: OptionTally[]) {
        this.results = results;
    }
}

/**
 * Name-value pairs representing the counts per option/candidate
 */
export class OptionTally {

    /**
     * Name of the option / candidate
     */
    name: string;

    /**
     * Tally of this candidate
     */
    count: number;

    /**
     * Constructor
     * @param name Name of the option / candidate
     * @param count Tally of this candidate
     */
    constructor(name: string, count: number) {
        this.name = name;
        this.count = count;
    }
}
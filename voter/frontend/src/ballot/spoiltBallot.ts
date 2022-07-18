import bigInt from "big-integer";
import {Cipher} from "./encryptedBallot";
import {ElGamal} from "../cryptography/elGamal";
import {BigNumberHelper} from "../helper/bigNumberHelper";

/**
 * Represents a spoilt ballot, i.e., the decryption of the uncasted ballot.
 */
export class SpoiltBallot {

    /**
     * Ballot code of the virtual ballot
     */
    ballotCode: string;

    /**
     * Options / candidates in plaintext
     */
    options: PlainTextOption[];

    /**
     * Constructor
     * @param ballotCode Ballot code of the virtual ballot
     * @param options Options / candidates in plaintext
     */
    constructor(ballotCode: string, options: PlainTextOption[]) {
        this.ballotCode = ballotCode;
        this.options = options;
    }
}

/**
 * Represents one option / candidate in plaintext
 */
export class PlainTextOption {

    /**
     * Name of the option / candidate
     */
    name: string;

    /**
     * Short code of this option
     */
    shortCode: string;

    /**
     * Position for displaying the options in the correct order (as on the paper ballot)
     */
    position: number;

    /**
     * Random values used to encrypt this option
     */
    randomness: string[];

    /**
     * Vector of zeroes and 1 one representing this option
     */
    values: number[];

    /**
     * Constructor
     * @param name Name of the option / candidate
     * @param shortCode Short code of this option
     * @param position Position for displaying the options in the correct order (as on the paper ballot)
     * @param randomness Random values used to encrypt this option
     * @param values Vector of zeroes and 1 one representing this option
     */
    constructor(name: string, shortCode: string, position: number, randomness: string[], values: number[]) {
        this.name = name;
        this.shortCode = shortCode;
        this.position = position;
        this.randomness = randomness;
        this.values = values;
    }

    /**
     * Verifies that the encrypted values are reproducible by re-encrypting the plaintext values with the corresponding randomness.
     * @param p ElGamal parameter p
     * @param g ElGamal parameter g
     * @param pk Election public key
     * @param encryptionReference Reference encryption to be compared against
     */
    verifyEncryption(p: bigInt.BigInteger, g: bigInt.BigInteger, pk: bigInt.BigInteger, encryptionReference: Cipher[]): boolean {
        const elGamal: ElGamal = new ElGamal();

        return this.values.every((v, index): boolean => {
            const cipher: Cipher = elGamal.encrypt(bigInt(v), pk, p, g, BigNumberHelper.fromHexString(this.randomness[index]));

            const reference: Cipher = encryptionReference[index];

            const areEqual: boolean = cipher.c.toString(16) === reference.c.toString(16) && cipher.d.toString(16) === reference.d.toString(16);

            return areEqual;
        });
    }
}
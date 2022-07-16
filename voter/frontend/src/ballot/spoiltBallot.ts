import bigInt from "big-integer";
import {Cipher} from "./encryptedBallot";
import {ElGamal} from "../cryptography/elGamal";
import {BigNumberHelper} from "../helper/bigNumberHelper";

export class SpoiltBallot {
    options: PlainTextOption[];
    ballotId: string;

    constructor(ballotId: string, options: PlainTextOption[]) {
        this.ballotId = ballotId;
        this.options = options;
    }
}

export class PlainTextOption {
    name: string;
    shortCode: string;
    position: number;
    randomness: string[];
    values: number[];

    constructor(name: string, shortCode: string, position: number, randomness: string[], values: number[]) {
        this.name = name;
        this.shortCode = shortCode;
        this.position = position;
        this.randomness = randomness;
        this.values = values;
    }

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
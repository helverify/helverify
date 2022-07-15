import {ProofOfContainingOne} from "./proofOfContainingOne";
import {BigNumberHelper} from "../helper/bigNumberHelper";
import {ProofOfZeroOrOne} from "./proofOfZeroOrOne";
import bigInt from "big-integer";
import {HashHelper} from "../helper/hashHelper";

export class EncryptedBallot {
    rowProofs: SumProof[];
    columnProofs: SumProof[];
    encryptedOptions: EncryptedOption[];
    ballotId: string;

    constructor(rowProofs: SumProof[], columnProofs: SumProof[], encryptedOptions: EncryptedOption[], ballotId: string) {
        this.rowProofs = rowProofs;
        this.columnProofs = columnProofs;
        this.encryptedOptions = encryptedOptions;
        this.ballotId = ballotId;
    }

    verifyShortCodes(){
        return this.encryptedOptions.every(eo => eo.verifyShortCode());
    }

    verifyBallotId(){
        const ciphers: Cipher[] = this.encryptedOptions.flatMap(eo => eo.values.map(v => v.cipher));

        const hash: string = HashHelper.getCipherHash(ciphers);

        console.log(hash, this.ballotId);
        return hash === this.ballotId;
    }
}

export class EncryptedOption {
    shortCode: string;
    values: EncryptedOptionValue[];

    constructor(shortCode: string, values: EncryptedOptionValue[]) {
        this.shortCode = shortCode;
        this.values = values;
    }

    verifyShortCode(): boolean{
        let hash = HashHelper.getCipherHash(this.values.map(v => v.cipher));

        return hash.substring(0, 2) === this.shortCode;
    }
}

export class EncryptedOptionValue {
    cipher: Cipher;
    proof: ProofOfZeroOrOne;

    constructor(cipher: Cipher, proof: ProofOfZeroOrOne) {
        this.cipher = cipher;
        this.proof = proof;
    }

    verifyProof(publicKey: bigInt.BigInteger, p: bigInt.BigInteger, g: bigInt.BigInteger): boolean {
        return this.proof.verify(this.cipher.c, this.cipher.d, publicKey, p, g);
    }
}

export class Cipher {
    c: bigInt.BigInteger;
    d: bigInt.BigInteger;

    constructor(cHex: string, dHex: string) {
        this.c = BigNumberHelper.fromHexString(cHex);
        this.d = BigNumberHelper.fromHexString(dHex);
    }
}

export class SumProof {
    cipher: Cipher;
    proof: ProofOfContainingOne;

    constructor(cipher: Cipher, proof: ProofOfContainingOne) {
        this.cipher = cipher;
        this.proof = proof;
    }

    verify(publicKey: bigInt.BigInteger, p: bigInt.BigInteger, g: bigInt.BigInteger): boolean {
        return this.proof.verify(this.cipher.c, this.cipher.d, publicKey, p, g);
    }
}
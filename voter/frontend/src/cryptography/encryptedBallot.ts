import {ProofOfContainingOne} from "./proofOfContainingOne";
import {BigNumberHelper} from "../helper/bigNumberHelper";
import {ProofOfZeroOrOne} from "./proofOfZeroOrOne";
import bigInt from "big-integer";

export class EncryptedBallot {
    rowProofs: SumProof[];
    columnProofs: SumProof[];
    encryptedOptions: EncryptedOption[];

    constructor(rowProofs: SumProof[], columnProofs: SumProof[], encryptedOptions: EncryptedOption[]) {
        this.rowProofs = rowProofs;
        this.columnProofs = columnProofs;
        this.encryptedOptions = encryptedOptions;
    }

    verifyProofs(publicKey: bigInt.BigInteger, p: bigInt.BigInteger, g: bigInt.BigInteger): boolean {
        this.rowProofs.forEach(proof => {
            let isValid: boolean = proof.verify(publicKey, p, g);
            if (!isValid) {
                return false;
            }
        });

        this.columnProofs.forEach(proof => {
            let isValid: boolean = proof.verify(publicKey, p, g);
            if (!isValid) {
                return false;
            }
        });

        this.encryptedOptions.forEach(option => {
            option.values.forEach(v =>{
                let isValid: boolean = v.verifyProof(publicKey, p, g);
                if(!isValid){
                    return false;
                }
            })
        });

        return true;
    }
}

export class EncryptedOption {
    shortCode: string;
    values: EncryptedOptionValue[];

    constructor(shortCode: string, values: EncryptedOptionValue[]) {
        this.shortCode = shortCode;
        this.values = values;
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
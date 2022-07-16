import {Cipher} from "../ballot/encryptedBallot";
import {ProofOfDecryption} from "../cryptography/proofOfDecryption";
import bigInt from "big-integer";

export class ResultEvidence {
    decryptedResults: DecryptedResult[];

    constructor(decryptedResults: DecryptedResult[]){
        this.decryptedResults = decryptedResults;
    }

    verifyDecryptionProofs(p: bigInt.BigInteger, g: bigInt.BigInteger):boolean {
        return this.decryptedResults.every(decRes => decRes.verifyShareProofs(p, g));
    }
}

export class DecryptedResult {
    cipherText: Cipher;
    plainText: number;
    decryptedShares: DecryptedShare[];

    constructor(cipherText: Cipher, plainText: number, decryptedShares: DecryptedShare[]) {
        this.cipherText = cipherText;
        this.plainText = plainText;
        this.decryptedShares = decryptedShares;
    }

    verifyShareProofs(p: bigInt.BigInteger, g: bigInt.BigInteger):boolean{
        return this.decryptedShares.every(share => share.verifyProof(this.cipherText, p, g));
    }
}

export class DecryptedShare {
    share: bigInt.BigInteger;
    proofOfDecryption: ProofOfDecryption;
    publicKeyShare: bigInt.BigInteger;

    constructor(share: bigInt.BigInteger, proofOfDecryption: ProofOfDecryption, publicKeyShare: bigInt.BigInteger) {
        this.share = share;
        this.proofOfDecryption = proofOfDecryption;
        this.publicKeyShare = publicKeyShare;
    }

    verifyProof(cipher: Cipher, p: bigInt.BigInteger, g: bigInt.BigInteger): boolean{
        return this.proofOfDecryption.verify(cipher.c, cipher.d, this.publicKeyShare, p, g);
    }
}
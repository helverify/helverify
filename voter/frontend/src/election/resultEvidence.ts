import {Cipher} from "../ballot/encryptedBallot";
import {ProofOfDecryption} from "../cryptography/proofOfDecryption";
import bigInt from "big-integer";

/**
 * Represents the evidence for the results of an election
 */
export class ResultEvidence {

    /**
     * Decrypted shares used to derive the final result
     */
    decryptedResults: DecryptedResult[];

    /**
     * Constructor
     * @param decryptedResults Decrypted shares used to derive the final result
     */
    constructor(decryptedResults: DecryptedResult[]) {
        this.decryptedResults = decryptedResults;
    }

    /**
     * Verifies that the decryption of the results has been performed correctly.
     * @param p ElGamal parameter p
     * @param g ElGamal parameter g
     */
    verifyDecryptionProofs(p: bigInt.BigInteger, g: bigInt.BigInteger): Promise<boolean> {
        return new Promise((resolve, reject) => {
            const isValid: boolean = this.decryptedResults.every(decRes => decRes.verifyShareProofs(p, g));
            resolve(isValid);
        });
    }
}

/**
 * Represents the decrypted result of one candidate / option
 */
export class DecryptedResult {

    /**
     * ElGamal ciphertext of the candidate / option result
     */
    cipherText: Cipher;

    /**
     * Result (tally) in plaintext
     */
    plainText: number;

    /**
     * Decrypted shares used to calculate the final tally
     */
    decryptedShares: DecryptedShare[];

    /**
     * Constructor
     * @param cipherText ElGamal ciphertext of the candidate / option result
     * @param plainText Result (tally) in plaintext
     * @param decryptedShares Decrypted shares used to calculate the final tally
     */
    constructor(cipherText: Cipher, plainText: number, decryptedShares: DecryptedShare[]) {
        this.cipherText = cipherText;
        this.plainText = plainText;
        this.decryptedShares = decryptedShares;
    }

    /**
     * Verifies that every decrypted share has been decrypted correctly.
     * @param p ElGamal parameter p
     * @param g ElGamal parameter g
     */
    verifyShareProofs(p: bigInt.BigInteger, g: bigInt.BigInteger): boolean {
        return this.decryptedShares.every(share => share.verifyProof(this.cipherText, p, g));
    }
}

/**
 * Represents one decrypted share
 */
export class DecryptedShare {

    /**
     * Decrypted share
     */
    share: bigInt.BigInteger;

    /**
     * Proof of correct decryption
     */
    proofOfDecryption: ProofOfDecryption;

    /**
     * Public key share of the consensus node performing the decryption
     */
    publicKeyShare: bigInt.BigInteger;

    /**
     * Constructor
     * @param share Decrypted share
     * @param proofOfDecryption Proof of correct decryption
     * @param publicKeyShare Public key share of the consensus node performing the decryption
     */
    constructor(share: bigInt.BigInteger, proofOfDecryption: ProofOfDecryption, publicKeyShare: bigInt.BigInteger) {
        this.share = share;
        this.proofOfDecryption = proofOfDecryption;
        this.publicKeyShare = publicKeyShare;
    }

    /**
     * Verifies that the specified ciphertext has been correctly decrypted
     * @param cipher ElGamal ciphertext
     * @param p ElGamal parameter p
     * @param g ElGamal parameter g
     */
    verifyProof(cipher: Cipher, p: bigInt.BigInteger, g: bigInt.BigInteger): boolean {
        return this.proofOfDecryption.verify(cipher.c, cipher.d, this.publicKeyShare, p, g);
    }
}
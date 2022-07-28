import {ProofOfContainingOne} from "../cryptography/proofOfContainingOne";
import {BigNumberHelper} from "../helper/bigNumberHelper";
import {ProofOfZeroOrOne} from "../cryptography/proofOfZeroOrOne";
import bigInt from "big-integer";
import {HashHelper} from "../helper/hashHelper";
import {ElectionParameters} from "../election/election";

/**
 * Represents an encrypted ballot
 */
export class EncryptedBallot {

    /**
     * Contains the proofs that each row sums up to one.
     */
    rowProofs: SumProof[];

    /**
     * Contains the proofs that each column sums up to one
     */
    columnProofs: SumProof[];

    /**
     * Encrypted Options of this ballot
     */
    encryptedOptions: EncryptedOption[];

    /**
     * Ballot code of this virtual ballot.
     */
    ballotCode: string;

    /**
     * Constructor
     * @param rowProofs Proofs that each row sums up to one.
     * @param columnProofs Proofs that each column sums up to one
     * @param encryptedOptions Encrypted Options of this ballot
     * @param ballotCode Ballot code of this virtual ballot
     */
    constructor(rowProofs: SumProof[], columnProofs: SumProof[], encryptedOptions: EncryptedOption[], ballotCode: string) {
        this.rowProofs = rowProofs;
        this.columnProofs = columnProofs;
        this.encryptedOptions = encryptedOptions;
        this.ballotCode = ballotCode;
    }

    /**
     * Verifies that all the short codes are matching the encryptions they have been derived from.
     */
    verifyShortCodes() {
        return this.encryptedOptions.every(eo => eo.verifyShortCode());
    }

    /**
     * Verifies that the ballot code has been correctly derived from the encryptions.
     */
    verifyBallotId() {
        const ciphers: Cipher[] = this.encryptedOptions.flatMap(eo => eo.values.map(v => v.cipher));

        const hash: string = HashHelper.getCipherHash(ciphers);

        return hash === this.ballotCode;
    }

    verifyRowProofs(electionParameters: ElectionParameters): boolean {
        return this.rowProofs.every(rp => rp.verify(electionParameters.publicKey, electionParameters.p, electionParameters.g));
    }

    verifyColumnProofs(electionParameters: ElectionParameters): boolean {
        return this.columnProofs.every(cp => cp.verify(electionParameters.publicKey, electionParameters.p, electionParameters.g));
    }

    verifyContainsOnlyZeroOrOne(electionParameters: ElectionParameters): boolean {
        return this.encryptedOptions.every(eo => eo.values.every(v => v.verifyProof(electionParameters.publicKey, electionParameters.p, electionParameters.g)));
    }
}

/**
 * Represents an encrypted option / candidate
 */
export class EncryptedOption {

    /**
     * First two characters of the SHA-256 hash of this option's ciphertext.
     */
    shortCode: string;

    /**
     * Vector of encryptions (encrypted zeros and exactly 1 one)
     */
    values: EncryptedOptionValue[];

    /**
     * Constructor
     * @param shortCode First two characters of the SHA-256 hash of this option's ciphertext.
     * @param values ector of encryptions (encrypted zeros and exactly 1 one)
     */
    constructor(shortCode: string, values: EncryptedOptionValue[]) {
        this.shortCode = shortCode;
        this.values = values;
    }

    /**
     * Verifies that the short code provided actually corresponds to the first two characters of the encryption's SHA-256 hash.
     */
    verifyShortCode(): boolean {
        let hash = HashHelper.getCipherHash(this.values.map(v => v.cipher));

        return hash.substring(0, 2) === this.shortCode;
    }
}

/**
 * Represents an encryption of zero or one with the corresponding proof.
 */
export class EncryptedOptionValue {

    /**
     * Ciphertext
     */
    cipher: Cipher;

    /**
     * Proof that the ciphertext contains an encryption of zero or one and nothing else.
     */
    proof: ProofOfZeroOrOne;

    /**
     * Constructor
     * @param cipher Ciphertext
     * @param proof Proof that the ciphertext contains an encryption of zero or one and nothing else.
     */
    constructor(cipher: Cipher, proof: ProofOfZeroOrOne) {
        this.cipher = cipher;
        this.proof = proof;
    }

    /**
     * Verifies that the proof of containing zero or one is correct.
     * @param publicKey Election public key.
     * @param p ElGamal parameter p
     * @param g ElGamal parameter g
     */
    verifyProof(publicKey: bigInt.BigInteger, p: bigInt.BigInteger, g: bigInt.BigInteger): boolean {
        return this.proof.verify(this.cipher.c, this.cipher.d, publicKey, p, g);
    }
}

/**
 * Represents an ElGamal ciphertext
 */
export class Cipher {

    /**
     * First component of the ElGamal ciphertext
     */
    c: bigInt.BigInteger;

    /**
     * Second component of the ElGamal ciphertext
     */
    d: bigInt.BigInteger;

    /**
     * Constructor
     * @param cHex First component of the ElGamal ciphertext as a hexadecimal string
     * @param dHex Second component of the ElGamal ciphertext as a hexadecimal string
     */
    constructor(cHex: string, dHex: string) {
        this.c = BigNumberHelper.fromHexString(cHex);
        this.d = BigNumberHelper.fromHexString(dHex);
    }
}

/**
 * Proofs that a ciphertext (sum of multiple ciphertexts) contains the value 1.
 */
export class SumProof {

    /**
     * ElGamal ciphertext
     */
    cipher: Cipher;

    /**
     * Proof that the ciphertext contains the value 1
     */
    proof: ProofOfContainingOne;

    /**
     * Constructor
     * @param cipher ElGamal ciphertext
     * @param proof Proof that the ciphertext contains the value 1
     */
    constructor(cipher: Cipher, proof: ProofOfContainingOne) {
        this.cipher = cipher;
        this.proof = proof;
    }

    /**
     * Verifies the proof that the ciphertext indeed contains the value 1
     * @param publicKey Election public key
     * @param p ElGamal parameter p
     * @param g ElGamal parameter g
     */
    verify(publicKey: bigInt.BigInteger, p: bigInt.BigInteger, g: bigInt.BigInteger): boolean {
        return this.proof.verify(this.cipher.c, this.cipher.d, publicKey, p, g);
    }
}
import bigInt from "big-integer";
import sha256 from "crypto-js/sha256";
import {Cipher} from "../ballot/encryptedBallot";

/**
 * Provides various types of SHA-256 hashes
 */
export class HashHelper {

    /**
     * Creates a SHA-256 hash of all the BigIntegers with modulus q
     * @param q Modulus to be applied
     * @param params List of BigIntegers to be hashed
     */
    static getHash(q: bigInt.BigInteger, params: bigInt.BigInteger[]): bigInt.BigInteger {
        let combined = "";

        for(let i: number = 0; i < params.length; i++){
            let hash = sha256(params[i].toString(16));
            combined = combined.concat(hash.toString());
        }

        return bigInt(sha256(combined).toString(), 16).mod(q);
    }

    /**
     * Creates a SHA-256 hash of the specified list of strings
     * @param strings List of strings to be hashed
     */
    static getHashOfStrings(strings: string[]): string{
        let combined = "";

        for(let i: number = 0; i < strings.length; i++){
            let hash = sha256(strings[i]);
            combined = combined.concat(hash.toString());
        }

        return sha256(combined).toString();
    }

    /**
     * Creates a SHA-256 hash of the specified ElGamal ciphertexts
     * @param ciphers List of ElGamal ciphertexts to be hashed
     */
    static getCipherHash(ciphers: Cipher[]){
        let combined = "";

        const sortedCiphers = ciphers.sort((a,b) => {
            return a.c.toString(16).localeCompare(b.c.toString(16))
        })

        for(let i: number = 0; i < sortedCiphers.length; i++){
            let hashC: string = sha256(sortedCiphers[i].c.toString(16)).toString()
            let hashD: string = sha256(sortedCiphers[i].d.toString(16)).toString();

            combined = combined.concat(hashC.toString(), hashD.toString());
        }

        return sha256(combined).toString();
    }
}
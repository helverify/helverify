import bigInt from "big-integer";
import {Cipher} from "../ballot/encryptedBallot";

/**
 * Exponential ElGamal encryption algorithm
 */
export class ElGamal {

    /**
     * Encrypts a message using the exponential ElGamal algorithm
     * @param message Plaintext message
     * @param publicKey Election public key
     * @param p ElGamal parameter p
     * @param g ElGamal parameter g
     * @param randomness Random value used for the encryption
     */
    encrypt(message: bigInt.BigInteger, publicKey: bigInt.BigInteger, p: bigInt.BigInteger, g: bigInt.BigInteger, randomness: bigInt.BigInteger): Cipher{
        const m: bigInt.BigInteger = g.modPow(message, p).mod(p); // exponential ElGamal message encoding

        return {
            c: g.modPow(randomness, p).mod(p),
            d: m.multiply(publicKey.modPow(randomness, p).mod(p)).mod(p)
        };
    }
}
import bigInt from "big-integer";
import {Cipher} from "../ballot/encryptedBallot";

export class ElGamal {

    encrypt(message: bigInt.BigInteger, publicKey: bigInt.BigInteger, p: bigInt.BigInteger, g: bigInt.BigInteger, randomness: bigInt.BigInteger): Cipher{
        const m: bigInt.BigInteger = g.modPow(message, p).mod(p); // exponential ElGamal message encoding

        return {
            c: g.modPow(randomness, p).mod(p),
            d: m.multiply(publicKey.modPow(randomness, p).mod(p)).mod(p)
        };
    }
}
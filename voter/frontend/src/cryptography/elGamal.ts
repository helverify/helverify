import bigInt from "big-integer";
import {Cipher} from "../ballot/encryptedBallot";

export class ElGamal {

    encrypt(message: bigInt.BigInteger, publicKey: bigInt.BigInteger, p: bigInt.BigInteger, g: bigInt.BigInteger, randomness: bigInt.BigInteger | null = null): Cipher{
        if(randomness === null){
            randomness = bigInt.randBetween(bigInt.one, p);
        }

        const m: bigInt.BigInteger = g.modPow(message, p).mod(p); // exponential ElGamal message encoding

        const c: bigInt.BigInteger = g.modPow(randomness, p).mod(p);

        const d: bigInt.BigInteger = m.multiply(publicKey.modPow(randomness, p).mod(p)).mod(p);

        return {
            c: c,
            d: d
        };
    }
}
import bigInt from "big-integer";
import {HashHelper} from "../helper/hashHelper";
import {BigNumberHelper} from "../helper/bigNumberHelper";

/**
 * Proves that a ciphertext (or a sum of ciphertexts) contains the value 1
 */
export class ProofOfContainingOne{

    /**
     * U component
     */
    u: bigInt.BigInteger;

    /**
     * V component
     */
    v: bigInt.BigInteger;

    /**
     * S component
     */
    s: bigInt.BigInteger;

    /**
     * Constructor
     * @param uHex U component as a hexadecimal string
     * @param vHex V component as a hexadecimal string
     * @param sHex S component as a hexadecimal string
     */
    constructor(uHex: string, vHex: string, sHex: string) {
        this.u = BigNumberHelper.fromHexString(uHex);
        this.v = BigNumberHelper.fromHexString(vHex);
        this.s = BigNumberHelper.fromHexString(sHex);
    }

    /**
     * Verifies that the proof is valid, i.e., that the ciphertext indeed contains the value 1
     * @param a C component of the ElGamal ciphertext
     * @param b D component of the ElGamal ciphertext
     * @param h Election public key
     * @param p ElGamal parameter p
     * @param g ElGamal parameter g
     */
    verify(a: bigInt.BigInteger, b: bigInt.BigInteger, h: bigInt.BigInteger, p: bigInt.BigInteger, g: bigInt.BigInteger): boolean{
        const q: bigInt.BigInteger = p.subtract(bigInt.one).multiply(bigInt(2).modInv(p)).mod(p);

        const c: bigInt.BigInteger = HashHelper.getHash(q, [h, a, b, this.u, this.v]);

        const gInverse: bigInt.BigInteger = g.modInv(p).mod(p);

        const condition1 : boolean = g.modPow(this.s, p).mod(p).equals(this.u.multiply(a.modPow(c, p)).mod(p));

        const condition2 : boolean = h.modPow(this.s, p).equals(this.v.multiply(b.multiply(gInverse).mod(p).modPow(c, p).mod(p)).mod(p));

        return condition1 && condition2;
    }
}
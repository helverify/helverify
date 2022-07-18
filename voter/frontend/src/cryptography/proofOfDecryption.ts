import bigInt from "big-integer";
import {BigNumberHelper} from "../helper/bigNumberHelper";
import {HashHelper} from "../helper/hashHelper";

/**
 * Proves that the decryption of a ciphertext has been performed correctly
 */
export class ProofOfDecryption {

    /**
     * D component
     */
    d: bigInt.BigInteger;

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
     * @param dHex D component as hexadecimal string
     * @param uHex U component as hexadecimal string
     * @param vHex V component as hexadecimal string
     * @param sHex S component as hexadecimal string
     */
    constructor(dHex: string, uHex: string, vHex: string, sHex: string) {
        this.d = BigNumberHelper.fromHexString(dHex);
        this.u = BigNumberHelper.fromHexString(uHex);
        this.v = BigNumberHelper.fromHexString(vHex);
        this.s = BigNumberHelper.fromHexString(sHex);
    }

    /**
     * Verifies that this proof is valid, i.e., that the specified ciphertext has indeed been decrypted correctly
     * @param a C component of the ElGamal ciphertext
     * @param b D component of the ElGamal ciphertext
     * @param pk Public key share of the decrypting consensus node
     * @param p ElGamal parameter p
     * @param g ElGamal parameter g
     */
    verify(a: bigInt.BigInteger, b: bigInt.BigInteger, pk: bigInt.BigInteger, p: bigInt.BigInteger, g: bigInt.BigInteger): boolean {
        const q: bigInt.BigInteger = p.subtract(bigInt.one).multiply(bigInt(2).modInv(p)).mod(p);

        const c: bigInt.BigInteger = HashHelper.getHash(q, [pk, a, b, this.u, this.v]);

        const condition1: boolean = a.mod(p).modPow(this.s, p).mod(p).equals(this.u.multiply(this.d.modPow(c, p).mod(p)).mod(p));

        const condition2: boolean = g.mod(p).modPow(this.s, p).mod(p).equals(this.v.multiply(pk.modPow(c, p).mod(p)).mod(p));

        return condition1 && condition2;
    }
}
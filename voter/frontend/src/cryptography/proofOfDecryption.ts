import bigInt from "big-integer";
import {BigNumberHelper} from "../helper/bigNumberHelper";
import {HashHelper} from "../helper/hashHelper";

/**
 *
 */
export class ProofOfDecryption {
    d: bigInt.BigInteger;
    u: bigInt.BigInteger;
    v: bigInt.BigInteger;
    s: bigInt.BigInteger;

    /**
     *
     * @param dHex
     * @param uHex
     * @param vHex
     * @param sHex
     */
    constructor(dHex: string, uHex: string, vHex: string, sHex: string) {
        this.d = BigNumberHelper.fromHexString(dHex);
        this.u = BigNumberHelper.fromHexString(uHex);
        this.v = BigNumberHelper.fromHexString(vHex);
        this.s = BigNumberHelper.fromHexString(sHex);
    }

    /**
     *
     * @param a
     * @param b
     * @param pk
     * @param p
     * @param g
     */
    verify(a: bigInt.BigInteger, b: bigInt.BigInteger, pk: bigInt.BigInteger, p: bigInt.BigInteger, g: bigInt.BigInteger): boolean {
        const q: bigInt.BigInteger = p.subtract(bigInt.one).multiply(bigInt(2).modInv(p)).mod(p);

        const c: bigInt.BigInteger = HashHelper.getHash(q, [pk, a, b, this.u, this.v]);

        const condition1: boolean = a.mod(p).modPow(this.s, p).mod(p).equals(this.u.multiply(this.d.modPow(c, p).mod(p)).mod(p));

        const condition2: boolean = g.mod(p).modPow(this.s, p).mod(p).equals(this.v.multiply(pk.modPow(c, p).mod(p)).mod(p));

        return condition1 && condition2;
    }
}
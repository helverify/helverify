import bigInt from "big-integer";
import {HashHelper} from "../helper/hashHelper";
import {BigNumberHelper} from "../helper/bigNumberHelper";

/**
 *
 */
export class ProofOfContainingOne{
    u: bigInt.BigInteger;
    v: bigInt.BigInteger;
    s: bigInt.BigInteger;

    constructor(uHex: string, vHex: string, sHex: string) {
        this.u = BigNumberHelper.fromHexString(uHex);
        this.v = BigNumberHelper.fromHexString(vHex);
        this.s = BigNumberHelper.fromHexString(sHex);
    }

    /**
     *
     * @param a
     * @param b
     * @param h
     * @param p
     * @param g
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
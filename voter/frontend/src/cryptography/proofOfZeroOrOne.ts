import bigInt from "big-integer";
import {HashHelper} from "../helper/hashHelper";
import {BigNumberHelper} from "../helper/bigNumberHelper";

/**
 *
 */
export class ProofOfZeroOrOne {
    u0: bigInt.BigInteger;
    u1: bigInt.BigInteger;
    v0: bigInt.BigInteger;
    v1: bigInt.BigInteger;
    c0: bigInt.BigInteger;
    c1: bigInt.BigInteger;
    r0: bigInt.BigInteger;
    r1: bigInt.BigInteger;

    constructor(u0Hex: string, u1Hex: string, v0Hex: string, v1Hex: string, c0Hex: string, c1Hex: string, r0Hex: string, r1Hex: string) {
        this.u0 = BigNumberHelper.fromHexString(u0Hex);
        this.u1 = BigNumberHelper.fromHexString(u1Hex);
        this.v0 = BigNumberHelper.fromHexString(v0Hex);
        this.v1 = BigNumberHelper.fromHexString(v1Hex);
        this.c0 = BigNumberHelper.fromHexString(c0Hex);
        this.c1 = BigNumberHelper.fromHexString(c1Hex);
        this.r0 = BigNumberHelper.fromHexString(r0Hex);
        this.r1 = BigNumberHelper.fromHexString(r1Hex);
    }

    /**
     *
     * @param a
     * @param b
     * @param h
     * @param p
     * @param g
     */
    verify(a: bigInt.BigInteger, b: bigInt.BigInteger, h: bigInt.BigInteger, p: bigInt.BigInteger, g: bigInt.BigInteger) {
        const q: bigInt.BigInteger = p.subtract(bigInt.one).multiply(bigInt(2).modInv(p)).mod(p);

        const c = HashHelper.getHash(q, [h, a, b, this.u0, this.v0, this.u1, this.v1]);

        const gInverse: bigInt.BigInteger = g.modInv(p).mod(p);

        const condition1: boolean = g.modPow(this.r0, p).equals(this.u0.multiply(a.modPow(this.c0, p).mod(p)).mod(p));
        const condition2: boolean = g.modPow(this.r1, p).equals(this.u1.multiply(a.modPow(this.c1, p).mod(p)).mod(p));
        const condition3: boolean = h.modPow(this.r0, p).equals(this.v0.multiply(b.modPow(this.c0, p).mod(p)).mod(p));
        const condition4: boolean = h.modPow(this.r1, p).equals(this.v1.multiply(b.multiply(gInverse).mod(p).modPow(this.c1, p)).mod(p));
        const condition5: boolean = this.c0.add(this.c1).mod(q).equals(c);

        return condition1 && condition2 && condition3 && condition4 && condition5;
    }
}
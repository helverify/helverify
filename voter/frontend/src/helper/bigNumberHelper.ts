import bigInt from "big-integer";

/**
 * Helper for BigInteger generation
 */
export class BigNumberHelper{

    /**
     * Converts a hex string to a BigInteger
     * @param hex Hexadecimal string representation
     */
    static fromHexString(hex: string){
        return bigInt(hex, 16);
    }
}
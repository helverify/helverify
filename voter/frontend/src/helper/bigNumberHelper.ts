import bigInt from "big-integer";

export class BigNumberHelper{
    static fromHexString(hex: string){
        return bigInt(hex, 16);
    }
}
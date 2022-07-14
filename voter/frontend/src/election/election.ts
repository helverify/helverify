import bigInt from "big-integer";

export type ElectionParameters = {
    p: bigInt.BigInteger;
    g: bigInt.BigInteger;
    publicKey: bigInt.BigInteger;
    consensusNodePublicKeys: bigInt.BigInteger[];
}
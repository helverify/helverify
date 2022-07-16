import bigInt from "big-integer";
import sha256 from "crypto-js/sha256";
import {Cipher} from "../ballot/encryptedBallot";

export class HashHelper {
    static getHash(q: bigInt.BigInteger, params: bigInt.BigInteger[]){
        let combined = "";

        for(let i: number = 0; i < params.length; i++){
            let hash = sha256(params[i].toString(16));
            combined = combined.concat(hash.toString());
        }

        return bigInt(sha256(combined).toString(), 16).mod(q);
    }

    static getHashOfStrings(strings: string[]){
        let combined = "";

        for(let i: number = 0; i < strings.length; i++){
            let hash = sha256(strings[i]);
            combined = combined.concat(hash.toString());
        }

        return sha256(combined).toString();
    }

    static getCipherHash(ciphers: Cipher[]){
        let combined = "";

        const sortedCiphers = ciphers.sort((a,b) => {
            return a.c.toString(16).localeCompare(b.c.toString(16))
        })

        for(let i: number = 0; i < sortedCiphers.length; i++){
            let hashC: string = sha256(sortedCiphers[i].c.toString(16)).toString()
            let hashD: string = sha256(sortedCiphers[i].d.toString(16)).toString();

            combined = combined.concat(hashC.toString(), hashD.toString());
        }

        return sha256(combined).toString();
    }
}
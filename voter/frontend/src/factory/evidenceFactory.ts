import {Cipher} from "../ballot/encryptedBallot";
import {EvidenceDto} from "../election/resultEvidenceDto";
import {DecryptedResult, DecryptedShare, ResultEvidence} from "../election/resultEvidence";
import {BigNumberHelper} from "../helper/bigNumberHelper";
import {ProofOfDecryption} from "../cryptography/proofOfDecryption";
import bigInt from "big-integer";

/**
 * Creates ResultEvidence
 */
export class EvidenceFactory {

    /**
     * Generates ResultEvidence from IPFS evidence
     * @param evidenceDto EvidenceDto from IPFS
     */
    static createEvidence(evidenceDto: EvidenceDto): ResultEvidence {
        const decryptedResults: DecryptedResult[] = evidenceDto.decryptedResults.map(dr => {
            const cipherText: Cipher = new Cipher(dr.cipherText.c, dr.cipherText.d);
            const plainText: number = dr.plainText;
            const shares: DecryptedShare[] = dr.shares.map(s => {
                const share: bigInt.BigInteger = BigNumberHelper.fromHexString(s.share);
                const proof: ProofOfDecryption = new ProofOfDecryption(s.proofOfDecryption.d, s.proofOfDecryption.u, s.proofOfDecryption.v, s.proofOfDecryption.s);
                const publicKeyShare: bigInt.BigInteger = BigNumberHelper.fromHexString(s.publicKeyShare);
                return new DecryptedShare(share, proof, publicKeyShare);
            })

            return new DecryptedResult(cipherText, plainText, shares);
        })

        return new ResultEvidence(decryptedResults);
    }
}
import {
    CipherDto,
    ProofOfContainingOneDto,
    ProofOfZeroOrOneDto,
    VirtualBallotDto
} from "../cryptography/ballot";
import {
    Cipher,
    EncryptedBallot,
    EncryptedOption,
    EncryptedOptionValue,
    SumProof
} from "../cryptography/encryptedBallot";
import {ProofOfContainingOne} from "../cryptography/proofOfContainingOne";
import {ProofOfZeroOrOne} from "../cryptography/proofOfZeroOrOne";

export class BallotFactory {

    static CreateEncryptedBallot(ballot: VirtualBallotDto): EncryptedBallot {
        let rowProofs: SumProof[] = [];
        let columnProofs: SumProof[] = [];
        let encryptedOptions: EncryptedOption[] = [];

        ballot.rowProofs.forEach(rp => {
            const cipher: Cipher = this.getCipher(rp.cipher);
            const proofOfContainingOne: ProofOfContainingOne = this.getProofOfContainingOne(rp.proofOfContainingOne);

            rowProofs.push(new SumProof(cipher, proofOfContainingOne));
        });

        ballot.columnProofs.forEach(cp => {
            const cipher: Cipher = this.getCipher(cp.cipher);
            const proofOfContainingOne: ProofOfContainingOne = this.getProofOfContainingOne(cp.proofOfContainingOne);

            columnProofs.push(new SumProof(cipher, proofOfContainingOne));
        });

        ballot.encryptedOptions.forEach(eo => {
            let values: EncryptedOptionValue[] = [];
            eo.values.forEach(v => {
                values.push(new EncryptedOptionValue(this.getCipher(v.cipher), this.getProofOfZeroOrOne(v.proofOfZeroOrOne)));
            });
            encryptedOptions.push(new EncryptedOption(eo.shortCode, values));
        })

        return new EncryptedBallot(rowProofs, columnProofs, encryptedOptions);
    }

    private static getProofOfContainingOne(proof: ProofOfContainingOneDto): ProofOfContainingOne {
        let u: string;
        let v: string;
        let s: string;

        u = proof.u;
        v = proof.v;
        s = proof.s;

        return new ProofOfContainingOne(u, v, s);
    }

    private static getProofOfZeroOrOne(proof: ProofOfZeroOrOneDto): ProofOfZeroOrOne {
        return new ProofOfZeroOrOne(proof.u0,
            proof.u1,
            proof.v0,
            proof.v1,
            proof.c0,
            proof.c1,
            proof.r0,
            proof.r1);
    }

    private static getCipher(cipher: CipherDto): Cipher {
        return new Cipher(cipher.c, cipher.d);
    }
}
import {
    CipherDto,
    ProofOfContainingOneDto,
    ProofOfZeroOrOneDto, SpoiltBallotDto,
    VirtualBallotDto
} from "../ballot/ballot";
import {
    Cipher,
    EncryptedBallot,
    EncryptedOption,
    EncryptedOptionValue,
    SumProof
} from "../ballot/encryptedBallot";
import {ProofOfContainingOne} from "../cryptography/proofOfContainingOne";
import {ProofOfZeroOrOne} from "../cryptography/proofOfZeroOrOne";
import {PlainTextOption, SpoiltBallot} from "../ballot/spoiltBallot";
import {CastBallot} from "../ballot/castBallot";

export class BallotFactory {

    static createEncryptedBallot(ballot: VirtualBallotDto): EncryptedBallot {
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

        return new EncryptedBallot(rowProofs, columnProofs, encryptedOptions, ballot.code);
    }

    static createSpoiltBallot(ballot: SpoiltBallotDto): SpoiltBallot {
        let plainTextOptions: PlainTextOption[] = ballot.options.map(o => new PlainTextOption(o.name, o.shortCode, o.values.indexOf(1), o.randomness));

        let spoiltBallot: SpoiltBallot = new SpoiltBallot(plainTextOptions);

        return spoiltBallot;
    }

    static createCastBallot(ballot: any[]): CastBallot {
        const selections : string[] = ballot[3];

        return new CastBallot(selections);
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
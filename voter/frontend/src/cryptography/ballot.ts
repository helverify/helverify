// initially generated using http://json2ts.com/

export interface RowProofDto {
    cipher: CipherDto;
    proofOfContainingOne: ProofOfContainingOneDto;
}

export interface ProofOfContainingOneDto {
    u: string;
    v: string;
    s: string;
}

export interface ColumnProofDto {
    cipher: CipherDto;
    proofOfContainingOne: ProofOfContainingOneDto;
}

export interface CipherDto {
    c: string;
    d: string;
}

export interface ProofOfZeroOrOneDto {
    u0: string;
    u1: string;
    v0: string;
    v1: string;
    c0: string;
    c1: string;
    r0: string;
    r1: string;
}

export interface ValueDto {
    cipher: CipherDto;
    proofOfZeroOrOne: ProofOfZeroOrOneDto;
}

export interface EncryptedOptionDto {
    shortCode: string;
    values: ValueDto[];
}

export interface VirtualBallotDto {
    code: string;
    rowProofs: RowProofDto[];
    columnProofs: ColumnProofDto[];
    encryptedOptions: EncryptedOptionDto[];
}
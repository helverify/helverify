// initially generated using http://json2ts.com/

export interface CipherTextDto {
    c: string;
    d: string;
}

export interface ProofOfDecryptionDto {
    d: string;
    u: string;
    v: string;
    s: string;
}

export interface ShareDto {
    share: string;
    proofOfDecryption: ProofOfDecryptionDto;
    publicKeyShare: string;
}

export interface DecryptedResultDto {
    cipherText: CipherTextDto;
    plainText: number;
    shares: ShareDto[];
}

export interface EvidenceDto {
    decryptedResults: DecryptedResultDto[];
}

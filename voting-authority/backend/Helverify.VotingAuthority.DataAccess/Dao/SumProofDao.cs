using Helverify.VotingAuthority.DataAccess.Dto;

namespace Helverify.VotingAuthority.DataAccess.Dao;

public struct SumProofDao
{
    public CipherTextDto Cipher { get; set; }
    public ProofOfContainingOneDao ProofOfContainingOne { get; set; }
}
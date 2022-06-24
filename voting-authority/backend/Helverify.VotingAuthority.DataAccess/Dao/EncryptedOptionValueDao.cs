using Helverify.VotingAuthority.DataAccess.Dto;

namespace Helverify.VotingAuthority.DataAccess.Dao;

public struct EncryptedOptionValueDao
{
    public CipherTextDto Cipher { get; set; }

    public ProofOfZeroOrOneDao ProofOfZeroOrOne { get; set; }
}
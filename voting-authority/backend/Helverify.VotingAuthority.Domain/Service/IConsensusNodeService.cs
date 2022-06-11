using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Domain.Service;

public interface IConsensusNodeService
{
    Task<PublicKeyDto> GenerateKeyPairAsync(Uri endpoint, Election election);
    Task<DecryptionShareDto> DecryptShareAsync(Uri endpoint, string c, string d);
}
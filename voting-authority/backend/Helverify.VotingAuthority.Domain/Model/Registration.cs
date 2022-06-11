using Helverify.Cryptography.ZeroKnowledge;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Extensions;
using Org.BouncyCastle.Math;


namespace Helverify.VotingAuthority.Domain.Model
{
    public class Registration
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public Uri Endpoint { get; set; }
        public string? ElectionId { get; set; }
        public BigInteger? PublicKey { get; set; }

        public void SetPublicKey(PublicKeyDto publicKeyDto, Election election)
        {
            ProofOfPrivateKeyOwnership proof = new ProofOfPrivateKeyOwnership(
                publicKeyDto.ProofOfPrivateKey.C.ExportToBigInteger(), publicKeyDto.ProofOfPrivateKey.D.ExportToBigInteger());

            bool isValid = proof.Verify(publicKeyDto.PublicKey.ExportToBigInteger(), election.P, election.G);

            if (!isValid)
            {
                throw new Exception("Consensus node has no private key matching the specified public key");
            }

            PublicKey = publicKeyDto.PublicKey.ExportToBigInteger();
        }
    }
}

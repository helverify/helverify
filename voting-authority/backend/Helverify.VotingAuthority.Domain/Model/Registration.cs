using Helverify.Cryptography.ZeroKnowledge;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Extensions;
using Org.BouncyCastle.Math;


namespace Helverify.VotingAuthority.Domain.Model
{
    /// <summary>
    /// Represents a registered consensus node.
    /// </summary>
    public class Registration
    {
        /// <summary>
        /// Unique identifier for the consensus node.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Name of the consensus node.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// API Endpoint for calling the consensus node.
        /// </summary>
        public Uri Endpoint { get; set; }

        /// <summary>
        /// Identifier of the corresponding election for which the registration is valid.
        /// </summary>
        public string? ElectionId { get; set; }

        /// <summary>
        /// Public key of the consensus node.
        /// </summary>
        public BigInteger? PublicKey { get; set; }

        /// <summary>
        /// Updates the registration with the public key of the consensus node.
        /// Also verifies that the consensus node is in possession of the corresponding private key.
        /// </summary>
        /// <param name="publicKeyDto"></param>
        /// <param name="election"></param>
        /// <exception cref="Exception"></exception>
        public void SetPublicKey(PublicKeyDto? publicKeyDto, Election election)
        {
            ProofOfPrivateKeyOwnership proof = new ProofOfPrivateKeyOwnership(
                publicKeyDto.ProofOfPrivateKey.C.ConvertToBigInteger(), publicKeyDto.ProofOfPrivateKey.D.ConvertToBigInteger());

            bool isValid = proof.Verify(publicKeyDto.PublicKey.ConvertToBigInteger(), election.P, election.G);

            if (!isValid)
            {
                throw new Exception("Consensus node has no private key matching the specified public key");
            }

            PublicKey = publicKeyDto.PublicKey.ConvertToBigInteger();
        }
    }
}

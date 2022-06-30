using Helverify.Cryptography.ZeroKnowledge;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Org.BouncyCastle.Math;


namespace Helverify.VotingAuthority.Domain.Model
{
    /// <summary>
    /// Represents a registered consensus node.
    /// </summary>
    public sealed class Registration
    {
        /// <summary>
        /// Name of the consensus node.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// API Endpoint for calling the consensus node.
        /// </summary>
        public Uri Endpoint { get; set; }
        
        /// <summary>
        /// Blockchain account of consensus node
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// Enode Identifier of consensus node
        /// </summary>
        public string Enode { get; set; }
        
        /// <summary>
        /// Public key for each election.
        /// </summary>

        public Dictionary<string, BigInteger> PublicKeys = new();

        /// <summary>
        /// Updates the registration with the public key of the consensus node.
        /// Also verifies that the consensus node is in possession of the corresponding private key.
        /// </summary>
        /// <param name="publicKeyDto"></param>
        /// <param name="election"></param>
        /// <exception cref="Exception"></exception>
        public void SetPublicKey(PublicKeyDto publicKeyDto, Election election)
        {
            if (election.Id == null)
            {
                throw new ArgumentNullException(nameof(election));
            }

            ProofOfPrivateKeyOwnership proof = new ProofOfPrivateKeyOwnership(
                publicKeyDto.ProofOfPrivateKey.C.ConvertToBigInteger(), publicKeyDto.ProofOfPrivateKey.D.ConvertToBigInteger());

            BigInteger publicKey = publicKeyDto.PublicKey.ConvertToBigInteger();

            bool isValid = proof.Verify(publicKey, election.P, election.G);

            if (!isValid)
            {
                throw new Exception("Consensus node has no private key matching the specified public key");
            }

            PublicKeys[election.Id] = publicKey;
        }
    }
}

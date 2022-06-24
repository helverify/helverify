using Helverify.Cryptography.Encryption;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Model
{
    /// <summary>
    /// Represents an election.
    /// </summary>
    public sealed class Election
    {
        private readonly IElGamal _elGamal;

        /// <summary>
        /// Constructor
        /// </summary>
        public Election()
        {
            _elGamal = new ExponentialElGamal();
        }

        /// <summary>
        /// Unique election identifier.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Name of the election.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Voting/election question
        /// </summary>
        public string Question { get; set; } = string.Empty;

        /// <summary>
        /// Represents the voting options for this election.
        /// </summary>
        public IList<ElectionOption> Options { get; set; } = new List<ElectionOption>();

        /// <summary>
        /// Prime p of the ElGamal cryptosystem.
        /// </summary>
        public BigInteger P { get; set; } 

        /// <summary>
        /// Generator g of the ElGamal cryptosystem.
        /// </summary>
        public BigInteger G { get; set; }

        /// <summary>
        /// Election public key
        /// </summary>
        public BigInteger? PublicKey { get; set; }

        /// <summary>
        /// Registered consensus nodes
        /// </summary>
        public IList<Registration> Registrations { get; set; } = new List<Registration>();

        /// <summary>
        /// Combines the public keys of the registered consensus nodes into a composite election public key.
        /// </summary>
        public void CombinePublicKeys()
        {
            IList<BigInteger> publicKeys = Registrations.Select(r => r.PublicKey!).ToList();

            List<DHPublicKeyParameters> dhPublicKeys = publicKeys.Select(pk => new DHPublicKeyParameters(pk, DhParameters)).ToList();

            DHPublicKeyParameters electionPublicKey = _elGamal.CombinePublicKeys(dhPublicKeys, DhParameters);

            PublicKey = electionPublicKey.Y;
        }

        /// <summary>
        /// Encrypts a message to an ElGamal ciphertext
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Ciphertext</returns>
        public ElGamalCipher Encrypt(int message)
        {
            return _elGamal.Encrypt(message, new DHPublicKeyParameters(PublicKey, DhParameters));
        }

        /// <summary>
        /// Combines the decryption shares of the consensus nodes to restore the plaintext message.
        /// </summary>
        /// <param name="decryptedShares">Decrypted shares</param>
        /// <param name="cipherD">Second component (d) of an ElGamal ciphertext</param>
        /// <returns></returns>
        public int CombineShares(IList<string> decryptedShares, string cipherD)
        {
            IList<BigInteger> shares = decryptedShares.Select(s => s.ConvertToBigInteger()).ToList();
            BigInteger d = cipherD.ConvertToBigInteger();

            int message = _elGamal.CombineShares(shares, d, P, G);

            return message;
        }

        /// <summary>
        /// Builds Diffie-Hellman parameters from the ElGamal cryptosystem parameters.
        /// </summary>
        public DHParameters DhParameters => new (P, G);

        /// <summary>
        /// Address of the smart contract for the election on the Ethereum blockchain.
        /// </summary>
        public string ContractAddress { get; set; }

        /// <summary>
        /// Creates the ballot template for this election.
        /// </summary>
        /// <returns></returns>
        public BallotTemplate GenerateBallotTemplate()
        {
            return new BallotTemplate(this);
        }
    }
}

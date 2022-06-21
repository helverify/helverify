using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Helverify.VotingAuthority.Domain.Helper;
using Org.BouncyCastle.Crypto.Parameters;

namespace Helverify.VotingAuthority.Domain.Model.Virtual
{
    /// <summary>
    /// Represents a virtual ballot (as opposed to the physical paper ballot)
    /// </summary>
    public class VirtualBallot
    {
        /// <summary>
        /// Long ballot code, consisting of the hash of all encryptions.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Options / candidates represented as ciphertext
        /// </summary>
        public IList<EncryptedOption> EncryptedOptions { get; internal set; }

        /// <summary>
        /// Options / canidates represented as plaintext
        /// </summary>
        public IList<PlainTextOption> PlainTextOptions { get; }

        /// <summary>
        /// Proofs which show that each sums up to one.
        /// </summary>
        public IList<SumProof> RowProofs { get; }

        /// <summary>
        /// Proofs which show that each column sums up to one.
        /// </summary>
        public IList<SumProof> ColumnProofs { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="plainTextOptions">Options / candidates in plain text</param>
        /// <param name="publicKey">Public key of the election which is used to encrypt the ballots</param>
        public VirtualBallot(IList<PlainTextOption> plainTextOptions, DHPublicKeyParameters publicKey)
        {
            EncryptedOptions = new List<EncryptedOption>();
            PlainTextOptions = new List<PlainTextOption>();
            RowProofs = new List<SumProof>();
            ColumnProofs = new List<SumProof>();

            foreach (PlainTextOption plainTextOption in plainTextOptions)
            {
                EncryptedOption encryptedOption = new EncryptedOption(publicKey, plainTextOption.Values);

                PlainTextOption ptOption = plainTextOption.Clone();

                ptOption.ShortCode = encryptedOption.ShortCode;

                PlainTextOptions.Add(ptOption);

                EncryptedOptions.Add(encryptedOption);
            }

            GenerateRowProofs(publicKey);

            GenerateColumnProofs(publicKey);

            Code = HashHelper.Hash(AllCiphers);

            // sort options by short code to hide positional information
            EncryptedOptions = EncryptedOptions.OrderBy(e => e.ShortCode).ToList();
        }

        /// <summary>
        /// Evaluates if all short codes are unique.
        /// </summary>
        /// <returns>True if short codes are unique, false otherwise.</returns>
        public bool AreShortCodesUnique()
        {
            IEnumerable<string> shortCodes = EncryptedOptions.Select(e => e.ShortCode.ToLower());

            return shortCodes.Distinct().Count() == EncryptedOptions.Count;
        }

        /// <summary>
        /// Generates a proof for each column of encryptions, proving that each column sums up to one.
        /// </summary>
        /// <param name="publicKey">Public key of the election, used to encrypt the options.</param>
        private void GenerateColumnProofs(DHPublicKeyParameters publicKey)
        {
            foreach (EncryptedOption encOption in EncryptedOptions)
            {
                IList<ElGamalCipher> columnCiphers = new List<ElGamalCipher>();

                for (int j = 0; j < EncryptedOptions.Count; j++)
                {
                    columnCiphers.Add(encOption.Values[j].Cipher);
                }

                ElGamalCipher sum = SumUpCiphers(publicKey, columnCiphers);

                ProofOfContainingOne columnProof = ProofOfContainingOne.Create(sum.C, sum.D, publicKey.Y, sum.R,
                    publicKey.Parameters.P, publicKey.Parameters.G);

                ColumnProofs.Add(new SumProof(columnProof, sum));
            }
        }

        /// <summary>
        /// Generates a proof for each row of encryptions, proving that each row sums up to one.
        /// </summary>
        /// <param name="publicKey">Public key of the election, used to encrypt the options.</param>
        private void GenerateRowProofs(DHPublicKeyParameters publicKey)
        {
            foreach (EncryptedOption encOption in EncryptedOptions)
            {
                IList<ElGamalCipher> rowCiphers = encOption.Values.Select(v => v.Cipher).ToList();

                ElGamalCipher sum = SumUpCiphers(publicKey, rowCiphers);

                ProofOfContainingOne rowProof = ProofOfContainingOne.Create(sum.C, sum.D, publicKey.Y, sum.R,
                    publicKey.Parameters.P, publicKey.Parameters.G);

                RowProofs.Add(new SumProof(rowProof, sum));
            }
        }

        /// <summary>
        /// Performs homomorphic addition of ciphertexts.
        /// </summary>
        /// <param name="publicKey">Public key of the election</param>
        /// <param name="ciphers">Ciphertexts</param>
        /// <returns></returns>
        private static ElGamalCipher SumUpCiphers(DHPublicKeyParameters publicKey, IList<ElGamalCipher> ciphers)
        {
            ElGamalCipher sum = ciphers[0];

            for (int i = 1; i < ciphers.Count; i++)
            {
                sum = sum.Add(ciphers[i], publicKey.Parameters.P);
            }

            return sum;
        }

        /// <summary>
        /// Extracts all ciphertexts from the encrypted options
        /// </summary>
        private ElGamalCipher[] AllCiphers => EncryptedOptions.SelectMany(e => e.Values.Select(v => v.Cipher)).ToArray();
    }
}

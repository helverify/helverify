using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Helverify.VotingAuthority.Domain.Helper;
using Org.BouncyCastle.Crypto.Parameters;

namespace Helverify.VotingAuthority.Domain.Model.Virtual
{
    public class Ballot
    {
        public string Code { get; }

        public IList<EncryptedOption> EncryptedOptions { get; internal set; }

        public IList<PlainTextOption> PlainTextOptions { get; internal set; }

        public IList<SumProof> RowProofs { get; }

        public IList<SumProof> ColumnProofs { get; }

        public Ballot(IList<PlainTextOption> plainTextOptions, DHPublicKeyParameters publicKey)
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

        public bool AreShortCodesUnique()
        {
            IEnumerable<string> shortCodes = EncryptedOptions.Select(e => e.ShortCode.ToLower());

            return shortCodes.Distinct().Count() == EncryptedOptions.Count;
        }

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

        private static ElGamalCipher SumUpCiphers(DHPublicKeyParameters publicKey, IList<ElGamalCipher> rowCiphers)
        {
            ElGamalCipher sum = rowCiphers[0];

            for (int i = 1; i < rowCiphers.Count; i++)
            {
                sum = sum.Add(rowCiphers[i], publicKey.Parameters.P);
            }

            return sum;
        }

        private ElGamalCipher[] AllCiphers => EncryptedOptions.SelectMany(e => e.Values.Select(v => v.Cipher)).ToArray();
    }

    public class PlainTextOption
    {
        public string ShortCode { get; set; }
        public string Name { get; set; }
        public IList<int> Values { get; }

        public PlainTextOption(string name, IList<int> values)
        {
            Name = name;
            Values = values;
            ShortCode = string.Empty;
        }

        public PlainTextOption Clone()
        {
            return new PlainTextOption(Name, Values);
        }
    }
}

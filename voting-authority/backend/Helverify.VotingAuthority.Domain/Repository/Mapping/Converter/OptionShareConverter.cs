using AutoMapper;
using Helverify.Cryptography.ZeroKnowledge;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model.Decryption;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Repository.Mapping.Converter
{
    public class OptionShareConverter : ITypeConverter<DecryptedBallotShareDto, IList<OptionShare>>
    {

        private IList<DecryptedShare> ConvertDecryptedShares(IList<DecryptionShareDto> decryptedBallotDecryptedShare,
            BigInteger publicKey)
        {
            IList<DecryptedShare> shares = new List<DecryptedShare>();

            foreach (DecryptionShareDto decryptedShare in decryptedBallotDecryptedShare)
            {
                DecryptedShare share = new DecryptedShare
                {
                    Share = decryptedShare.DecryptedShare.ConvertToBigInteger(),
                    ProofOfDecryption = new ProofOfDecryption(
                        decryptedShare.ProofOfDecryption.D.ConvertToBigInteger(),
                        decryptedShare.ProofOfDecryption.U.ConvertToBigInteger(),
                        decryptedShare.ProofOfDecryption.V.ConvertToBigInteger(),
                        decryptedShare.ProofOfDecryption.S.ConvertToBigInteger()
                    ),
                    PublicKeyShare = publicKey
                };

                shares.Add(share);
            }

            return shares;
        }

        public IList<OptionShare> Convert(DecryptedBallotShareDto decryptedBallot, IList<OptionShare> destination, ResolutionContext context)
        {
            IList<OptionShare> optionSharesPart = new List<OptionShare>();

            foreach (string key in decryptedBallot.DecryptedShares.Keys)
            {
                IList<DecryptionShareDto> decryptedShareDtos = decryptedBallot.DecryptedShares[key];

                IList<DecryptedShare> shares = ConvertDecryptedShares(decryptedShareDtos, decryptedBallot.PublicKey);

                OptionShare optionShare = new OptionShare
                {
                    ShortCode = key,
                    Shares = shares,
                };

                optionSharesPart.Add(optionShare);
            }

            return optionSharesPart;
        }
    }
}

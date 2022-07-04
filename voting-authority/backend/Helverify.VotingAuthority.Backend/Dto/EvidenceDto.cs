using System.ComponentModel.DataAnnotations;

namespace Helverify.VotingAuthority.Backend.Dto
{
    /// <summary>
    /// Parameters for publishing voting evidence.
    /// </summary>
    public struct EvidenceDto
    {
        public EvidenceDto()
        {
            SpoiltBallotIndex = 0;
            SelectedOptions = new List<string>();
        }

        /// <summary>
        /// Contains the selected options of a ballot.
        /// </summary>
        public IList<string> SelectedOptions { get; set; }

        /// <summary>
        /// Column to be spoiled (0 or 1)
        /// </summary>
        [Range(0, 1)]
        public int SpoiltBallotIndex { get; set; }
    }
}

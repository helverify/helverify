using System.ComponentModel.DataAnnotations;

namespace Helverify.VotingAuthority.Backend.Dto
{
    public class EvidenceDto
    {
        public IList<string> SelectedOptions { get; set; } = new List<string>();

        /// <summary>
        /// Column to be spoiled (0 or 1)
        /// </summary>
        [Range(0, 1)]
        public int SpoiltBallotIndex { get; set; }
    }
}

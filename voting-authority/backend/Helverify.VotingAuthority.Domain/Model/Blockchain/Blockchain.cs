namespace Helverify.VotingAuthority.Domain.Model.Blockchain
{
    public class Blockchain
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public IList<Registration> Registrations { get; set; } = new List<Registration>();
    }
}

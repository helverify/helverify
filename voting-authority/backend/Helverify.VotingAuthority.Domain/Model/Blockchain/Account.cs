namespace Helverify.VotingAuthority.Domain.Model.Blockchain
{
    public class Account
    {
        public string Address { get; }
        public string Funds { get; set; }

        public Account(string address, string funds = "0")
        {
            Address = address;
            Funds = funds;
        }
    }
}

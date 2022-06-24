namespace Helverify.VotingAuthority.Domain.Model.Blockchain
{
    /// <summary>
    /// Represents a BC account.
    /// </summary>
    public sealed class Account
    {
        /// <summary>
        /// Account address
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Initial funds
        /// </summary>
        public string Funds { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="funds">Initial funds</param>
        public Account(string address, string funds = "0")
        {
            Address = address;
            Funds = funds;
        }
    }
}

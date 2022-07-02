namespace Helverify.VotingAuthority.DataAccess.Configuration
{
    /// <summary>
    /// Contains all used environment variables names
    /// </summary>
    internal class EnvironmentVariables
    {
        /// <summary>
        /// Connection string for accessing MongoDB
        /// </summary>
        internal const string MongoDbConnectionString = "MongoDbConnectionString";

        /// <summary>
        /// Connection string for accessing IPFS
        /// </summary>
        internal const string IpfsHost = "IpfsHost";

        /// <summary>
        /// Blockchain Account Password
        /// </summary>
        internal const string BcAccountPassword = "BC_ACCOUNT_PWD";
    }
}

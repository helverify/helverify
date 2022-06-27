namespace Helverify.VotingAuthority.DataAccess.Dao
{
    public class RegistrationDao
    {
        public string Name { get; set; } = string.Empty;
        
        public Uri? Endpoint { get; set; }
        
        public string? ElectionId { get; set; }
        
        public string AccountAddress { get; set; }
        
        public string Enode { get; set; }

        public Dictionary<string, string> PublicKeys = new ();
    }
}

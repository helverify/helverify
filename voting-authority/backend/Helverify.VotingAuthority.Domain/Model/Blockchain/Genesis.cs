using Helverify.VotingAuthority.DataAccess.Dao;

namespace Helverify.VotingAuthority.Domain.Model.Blockchain
{
    public class Genesis
    {
        private const string ExtradataPrefix = "0x0000000000000000000000000000000000000000000000000000000000000000";
        private const string ExtradataSuffix = "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";

        public Genesis()
        {
            
        }

        public Genesis(IList<RegistrationDao> registrations, int chainId, string initialBalance, int cliqueEpoch, int cliquePeriod)
        {
            IList<string> addresses = registrations.Select(r => r.AccountAddress).ToList();
            
            Extradata = ExtradataPrefix + string.Join(string.Empty, addresses) + ExtradataSuffix;

            foreach (RegistrationDao registration in registrations)
            {
                Alloc.Add(registration.AccountAddress, new AccountBalance { Balance = initialBalance });
            }

            Config = new Config
            {
                ChainId = chainId,
                Clique = new Clique
                {
                    Epoch = cliqueEpoch,
                    Period = cliquePeriod,
                },
                ByzantiumBlock = 0,
                ConstantinopleBlock = 0,
                Eip150Block = 0,
                Eip155Block = 0,
                Eip158Block = 0,
                HomesteadBlock = 0,
                PetersburgBlock = 0
            };
            GasLimit = "8000000";
            Difficulty = "1";
        }

        public Config Config { get; set; } = new();
        public string Difficulty { get; set; }
        public string GasLimit { get; set; }
        public string Extradata { get; set; }
        public IDictionary<string, AccountBalance> Alloc { get; set; } = new Dictionary<string, AccountBalance>();
    }

    public class Config
    {
        public int ChainId { get; set; }
        public int HomesteadBlock { get; set; }
        public int Eip150Block { get; set; }
        public int Eip155Block { get; set; }
        public int Eip158Block { get; set; }
        public int ByzantiumBlock { get; set; }
        public int ConstantinopleBlock { get; set; }
        public int PetersburgBlock { get; set; }
        public Clique Clique { get; set; } = new ();
    }

    public class Clique
    {
        public int Period { get; set; }
        public int Epoch { get; set; }
    }
    
    public class AccountBalance
    {
        public string Balance { get; set; } = null!;
    }
}

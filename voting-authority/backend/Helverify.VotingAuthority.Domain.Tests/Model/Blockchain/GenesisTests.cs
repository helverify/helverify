using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Helverify.VotingAuthority.Domain.Tests.Model.Blockchain
{
    internal class GenesisTests
    {
        [Test]
        public void TestGenesis()
        {
            // arrange
            //GenesisDto genesis = new GenesisDto
            //{
            //    Config = new Config
            //    {
            //        ChainId = 13337,
            //        CliqueDto = new CliqueDto
            //        {
            //            Epoch = 30000,
            //            Period = 5
            //        },
            //        ByzantiumBlock = 0,
            //        ConstantinopleBlock = 0,
            //        Eip150Block = 0,
            //        Eip155Block = 0,
            //        Eip158Block = 0,
            //        HomesteadBlock = 0,
            //        PetersburgBlock = 0
            //    },
            //    Difficulty = "1",
            //    Extradata = "0x000000000000000000000000000000000000000000000000000000000000000017C65ABfbCC0c235E113768DF66432B24806cA150000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000",
            //    GasLimit = "8000000",
            //    Alloc = new Dictionary<string, AccountBalanceDto>
            //    {
            //        {"17C65ABfbCC0c235E113768DF66432B24806cA15", new AccountBalanceDto{Balance = "3000000000"}},
            //        {"b587afE7AaAFD1457a2F87C9B38740454605354b", new AccountBalanceDto{Balance = "3000000000"}}
            //    }
            //};

            Account auth1 = new Account("b488f7E0Ddde1A2EFd1c70ba88C6c0C053aDb177", "42");
            Account auth2 = new Account("8100cBC8594F99130bFAa706E1007cC8b742edb8", "73");
            Account other = new Account("FF00c130bFAa70660bFAa706E1007cC8b742edb8", "92");


            IList<Account> authorities = new List<Account>
            {
                auth1, auth2
            };

            IList<Account> prefundedAccounts = new List<Account>
            {
                auth1, auth2, other
            };
            
            Genesis genesis = new Genesis(13337, authorities, prefundedAccounts);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            // act
            string genesisJson = JsonConvert.SerializeObject(genesis, settings);

            // assert
            Assert.That(genesisJson, Is.Not.Null);
        }
    }
}

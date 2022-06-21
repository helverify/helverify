using System.Text;
using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Helverify.VotingAuthority.Domain.Repository
{
    internal class PublicBulletinBoardRepository
    {
        private readonly IMapper _mapper;

        public PublicBulletinBoardRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void StoreBallot(PaperBallot paperBallot)
        {
            PaperBallotDao ballotDao = _mapper.Map<PaperBallotDao>(paperBallot);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string ballotJson = JsonConvert.SerializeObject(ballotDao, settings);
            
            string ballot1 = JsonConvert.SerializeObject(_mapper.Map<VirtualBallotDao>(paperBallot.Ballots[0]));
            string ballot2 = JsonConvert.SerializeObject(_mapper.Map<VirtualBallotDao>(paperBallot.Ballots[1]));
        }
    }
}

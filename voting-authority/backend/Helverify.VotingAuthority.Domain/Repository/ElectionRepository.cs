﻿using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Domain.Repository
{
    internal class ElectionRepository : IElectionRepository
    {
        private readonly IMongoService<ElectionDao> _mongoService;
        private readonly IMapper _mapper;

        public ElectionRepository(IMongoService<ElectionDao> mongoService, IMapper mapper)
        {
            _mongoService = mongoService;
            _mapper = mapper;
        }

        public async Task<Election> CreateAsync(Election election)
        {
            ElectionDao electionDao = _mapper.Map<ElectionDao>(election);

            await _mongoService.CreateAsync(electionDao);

            return _mapper.Map<Election>(electionDao);
        }

        public async Task<Election> GetAsync(string id)
        {
            ElectionDao electionDao = await _mongoService.GetAsync(id);

            Election election = _mapper.Map<Election>(electionDao);

            return election;
        }

        public async Task<IList<Election>> GetAsync()
        {
            IList<ElectionDao> electionDaos = (await _mongoService.GetAsync()).ToList();

            IList<Election> elections = _mapper.Map<IList<Election>>(electionDaos);

            return elections;
        }

        public async Task<Election> UpdateAsync(string id, Election election)
        {
            ElectionDao electionDao = _mapper.Map<ElectionDao>(election);

            await _mongoService.UpdateAsync(id, electionDao);

            return _mapper.Map<Election>(electionDao);
        }

        public async Task DeleteAsync(string id)
        {
            await _mongoService.RemoveAsync(id);
        }
    }
}

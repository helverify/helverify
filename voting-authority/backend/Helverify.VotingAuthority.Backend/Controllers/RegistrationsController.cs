﻿using AutoMapper;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace Helverify.VotingAuthority.Backend.Controllers
{
    [Route("api/elections/{electionId}/registrations")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private const string ContentType = "application/json";

        private readonly IRepository<Registration> _registrationRepository;
        private readonly IRepository<Election> _electionRepository;
        private readonly IConsensusNodeService _consensusNodeService;
        private readonly IMapper _mapper;

        public RegistrationsController(IRepository<Registration> registrationRepository, IRepository<Election> electionRepository, 
            IConsensusNodeService consensusNodeService, IMapper mapper)
        {
            _registrationRepository = registrationRepository;
            _electionRepository = electionRepository;
            _consensusNodeService = consensusNodeService;
            _mapper = mapper;
        }

        [HttpPost]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult<RegistrationDto>> Post([FromRoute] string electionId, [FromBody] RegistrationDto registrationDto)
        {
            Registration registration = _mapper.Map<Registration>(registrationDto);

            registration.ElectionId = electionId;
            
            Election election = await _electionRepository.GetAsync(electionId);

            PublicKeyDto publicKey = await _consensusNodeService.GenerateKeyPairAsync(registrationDto.Endpoint, election);

            registration.SetPublicKey(publicKey, election);

            registration = await _registrationRepository.CreateAsync(registration);

            RegistrationDto result = _mapper.Map<RegistrationDto>(registration);

            return Ok(result);
        }

        [HttpGet]
        [Produces(ContentType)]
        public async Task<ActionResult<List<RegistrationDto>>> Get()
        {
            IList<Registration> registrations = await _registrationRepository.GetAsync();

            IList<RegistrationDto> results = _mapper.Map<IList<RegistrationDto>>(registrations);
            
            return Ok(results);
        }

        [HttpGet]
        [Route("{id}")]
        [Produces(ContentType)]
        public async Task<ActionResult<RegistrationDto>> Get(string id)
        {
            Registration registration = await _registrationRepository.GetAsync(id);

            RegistrationDto result = _mapper.Map<RegistrationDto>(registration);
            
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult<RegistrationDto>> Put(string id, [FromBody] RegistrationDto registrationDto)
        {
            Registration registration = _mapper.Map<Registration>(registrationDto);

            registration = await _registrationRepository.UpdateAsync(id, registration);

            RegistrationDto result = _mapper.Map<RegistrationDto>(registration);
            
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task Delete(string id)
        {
            await _registrationRepository.DeleteAsync(id);
        }
    }
}

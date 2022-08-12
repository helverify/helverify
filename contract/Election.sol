// SPDX-License-Identifier: MIT
pragma solidity >=0.8.0 <0.9.0;

contract Election {

    struct PaperBallot{
        string ballotId;
        string ballot1Code;
        string ballot1Ipfs;
        string ballot2Code;
        string ballot2Ipfs;
    }

    struct SpoiltBallot{
        string code;
        string ballotIpfs;
    }

    struct CastBallot{
        string ballotId;
        string ballotCode;
        string ballotIpfs;
        string[] selection;
    }

    struct Result{
        string option;
        uint tally;
    }

    struct ElectionParameters {
        string electionId;
        ElGamalParameters elGamalParameters;
        string publicKey;
        string[] consensusNodePublicKeys;
    }

    struct ElGamalParameters {
        string p;
        string g;
    }

    mapping(string => PaperBallot) paperBallots;
   
    ElectionParameters public electionParameters;

    address public votingAuthority;

    string[] public ballotIds;
    
    mapping(string => SpoiltBallot) public spoiltBallots;

    mapping(string => CastBallot) public castBallots;

    mapping(uint => string) public options;

    Result[] public results;

    string public resultEvidenceIpfs;

    uint public numberOfCastBallots;
    
    constructor (){
        votingAuthority = msg.sender;
    }

    function setUp(string[] memory candidates, string memory id, ElGamalParameters memory parameters, string memory electionPublicKey, string[] memory consensusPublicKeys) public{
        if(msg.sender != votingAuthority){
            revert("Only voting authority is allowed to set up an election.");
        }

        for(uint i = 0; i < candidates.length; i++){
            options[i] = candidates[i];
        }

        electionParameters = ElectionParameters(id, parameters, electionPublicKey, consensusPublicKeys);
    }

    function storeBallot(PaperBallot[] memory ballots) public {
        if(msg.sender != votingAuthority){
            revert("Only voting authority is allowed to store ballots.");
        }

        for(uint i = 0; i < ballots.length; i++){
            string memory ballotId = ballots[i].ballotId;

            paperBallots[ballotId] = ballots[i];
            
            ballotIds.push(ballotId);
        }
    }

    function spoilBallot(string memory ballotId, string memory virtualBallotId, string memory spoiltBallotIpfs) public{
        if(msg.sender != votingAuthority){
            revert("Only voting authority is allowed to spoil ballots.");
        }

        spoiltBallots[ballotId] = SpoiltBallot(virtualBallotId, spoiltBallotIpfs);
    }

    function publishBallotSelection(string memory ballotId, string memory ballotCode, string[] memory shortCodes) public{
        if(msg.sender != votingAuthority){
            revert("Only voting authority is allowed publish the selected short codes");
        }
        
        PaperBallot memory ballot = paperBallots[ballotId];

        string memory ballotIpfs = "";
        
        if(equals(ballot.ballot1Code, ballotCode)){
            ballotIpfs = ballot.ballot1Ipfs;
        }

        if(equals(ballot.ballot2Code, ballotCode)){
            ballotIpfs = ballot.ballot2Ipfs;
        }

        if(equals(ballotIpfs, "")){
            revert("Ballot code provided does not match with stored ballot information.");
        }

        if(bytes(castBallots[ballotId].ballotId).length == 0){
            numberOfCastBallots++; // only increment number of cast ballots the first time the choice is registered
        }

        castBallots[ballotId] = CastBallot(ballotId, ballotCode, ballotIpfs, shortCodes);
    }

    function retrieveBallot(string memory ballotId) public view returns (PaperBallot memory){
        return paperBallots[ballotId];
    }
    
    function retrieveCastBallot(string memory ballotId) public view returns (CastBallot memory){
        return castBallots[ballotId];
    }

    function retrieveSpoiltBallot(string memory ballotId) public view returns (SpoiltBallot memory){
        return spoiltBallots[ballotId];
    }

    function publishResult(Result[] memory tallyResults, string memory tallyProofsIpfs) public {
        if(msg.sender != votingAuthority){
            revert("Only voting authority is allowed to publish results.");
        }
        delete results;

        for(uint i = 0; i < tallyResults.length; i++){
            results.push(tallyResults[i]);
        }

        resultEvidenceIpfs = tallyProofsIpfs;
    }

    // Pagination pattern according to: https://programtheblockchain.com/posts/2018/04/20/storage-patterns-pagination/
    function getAllBallotIds(uint startIndex, uint partitionSize) public view returns (string[] memory, uint){
        uint numberOfBallots = ballotIds.length;
        
        if(numberOfBallots < startIndex){
            revert("startIndex out of array bounds");
        }

        uint endIndex = numberOfBallots > startIndex + partitionSize ? startIndex + partitionSize : numberOfBallots;

        string[] memory ids = new string[](partitionSize);

        uint i = 0;
        
        for(uint index = startIndex; index < endIndex; index++){
            ids[i++] = ballotIds[index];
        }

        return (ids, endIndex);
    }

    function getNumberOfBallots() public view returns (uint) {
        return ballotIds.length;
    }

    function getNumberOfCastBallots() public view returns (uint) {
        return numberOfCastBallots;
    }

    function getResults() public view returns (Result[] memory){
        return results;
    }

    function getElectionParameters() public view returns(ElectionParameters memory){
        return electionParameters;
    }

    // according to: https://stackoverflow.com/questions/57727780/how-to-compare-string-in-solidity
    function equals(string memory a, string memory b) private pure returns (bool){
        return keccak256(abi.encodePacked(a)) == keccak256(abi.encodePacked(b));
    }
}
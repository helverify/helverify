// SPDX-License-Identifier: MIT
pragma solidity >=0.8.0 <0.9.0;

contract Election {

	//
	// STRUCTS
	//

	//
	// Represents a paper ballot, including its IPFS CIDs
	//
    struct PaperBallot{
        string ballotId;
        string ballot1Code;
        string ballot1Ipfs;
        string ballot2Code;
        string ballot2Ipfs;
    }

	//
	// Represents a spoilt ballot, including its IFPS CID
	//
    struct SpoiltBallot{
        string code;
        string ballotIpfs;
    }

	//
	// Represents a cast ballot, including its IFPS CID and its selections.
	//
    struct CastBallot{
        string ballotId;
        string ballotCode;
        string ballotIpfs;
        string[] selection;
    }

	//
	// Represents the final tally for one option.
	//
    struct Result{
        string option;
        uint tally;
    }

	//
	// Contains the election parameters, such as ElGamal parameters, keys and id.
	//
    struct ElectionParameters {
        string electionId;
        ElGamalParameters elGamalParameters;
        string publicKey;
        string[] consensusNodePublicKeys;
    }

	//
	// Represents a set of ElGamal cryptosystem parameters.
	//
    struct ElGamalParameters {
        string p;
        string g;
    }
	
	//
	// MEMORY
	//

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
    
	
	//
	// METHODS
	//
	
	//
	// Constructor, defining the Voting Authority
	//
    constructor (){
        votingAuthority = msg.sender;
    }
	
	//
	// Election set up, defining candidates, keys, and ElGamal parameters.
	//
    function setUp(string[] memory candidates, string memory id, ElGamalParameters memory parameters, string memory electionPublicKey, string[] memory consensusPublicKeys) public{
        if(msg.sender != votingAuthority){
            revert("Only voting authority is allowed to set up an election.");
        }

        for(uint i = 0; i < candidates.length; i++){
            options[i] = candidates[i];
        }

        electionParameters = ElectionParameters(id, parameters, electionPublicKey, consensusPublicKeys);
    }

	//
	// Stores a list of ballots onto the PBB.
	//
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

	//
	// Stores a the IPFS CID of a spoilt ballot on the PBB.
	//
    function spoilBallot(string memory ballotId, string memory virtualBallotId, string memory spoiltBallotIpfs) public{
        if(msg.sender != votingAuthority){
            revert("Only voting authority is allowed to spoil ballots.");
        }

        spoiltBallots[ballotId] = SpoiltBallot(virtualBallotId, spoiltBallotIpfs);
    }

	//
	// Publishes the selected short codes of a particular ballot onto the PBB.
	//
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

	//
	// Returns the PaperBallot references to IPFS.
	//
    function retrieveBallot(string memory ballotId) public view returns (PaperBallot memory){
        return paperBallots[ballotId];
    }
    
	//
	// Returns the IPFS CID of a CastBallot, including the selections.
	//
    function retrieveCastBallot(string memory ballotId) public view returns (CastBallot memory){
        return castBallots[ballotId];
    }

	//
	// Returns the IPFS CID of a SpoiltBallot.
	//
    function retrieveSpoiltBallot(string memory ballotId) public view returns (SpoiltBallot memory){
        return spoiltBallots[ballotId];
    }

	//
	// Publishes the final results of an election, including the IPFS CID of the corresponding evidence.
	//
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

	//
	// Returns all ballot IDs of this election.
    // Pagination pattern according to: https://programtheblockchain.com/posts/2018/04/20/storage-patterns-pagination/
	//
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

	//
	// Helper to retrieve the number of ballots.
	//
    function getNumberOfBallots() public view returns (uint) {
        return ballotIds.length;
    }

	//
	// Helper to retrieve the number of cast ballots.
	//
    function getNumberOfCastBallots() public view returns (uint) {
        return numberOfCastBallots;
    }

	//
	// Helper to retrieve the final results.
	//
    function getResults() public view returns (Result[] memory){
        return results;
    }

	//
	// Helper to retrieve election parameters.
	//
    function getElectionParameters() public view returns(ElectionParameters memory){
        return electionParameters;
    }

	//
    // String comparison according to: https://stackoverflow.com/questions/57727780/how-to-compare-string-in-solidity
	//
    function equals(string memory a, string memory b) private pure returns (bool){
        return keccak256(abi.encodePacked(a)) == keccak256(abi.encodePacked(b));
    }
}
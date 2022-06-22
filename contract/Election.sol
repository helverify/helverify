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

    struct Result{
        string option;
        uint tally;
        string proofIpfs;
    }

    mapping(string => PaperBallot) paperBallots;

    string public electionId;

    address public votingAuthority;

    string[] public ballotIds;
    
    mapping(string => SpoiltBallot) public spoiltBallots;

    mapping(string => string[]) public selectedShortCodes;

    mapping(uint => Result) public results;

    mapping(uint => string) public options;
    
    constructor (){
        votingAuthority = msg.sender;
    }

    function setUp(string[] memory candidates, string memory id) public{
        if(msg.sender != votingAuthority){
            revert("Only voting authority is allowed to set up an election.");
        }

        for(uint i = 0; i < candidates.length; i++){
            options[i] = candidates[i];
        }

        electionId = id;
    }

    function storeBallot(string memory ballotId, string memory ballotCode1, string memory ballot1Ipfs, string memory ballotCode2, string memory ballot2Ipfs) public {
        if(msg.sender != votingAuthority){
            revert("Only voting authority is allowed to store ballots.");
        }

        paperBallots[ballotId] = PaperBallot(ballotId, ballotCode1, ballot1Ipfs, ballotCode2, ballot2Ipfs);
        ballotIds.push(ballotId);
    }

    function spoilBallot(string memory ballotId, string memory virtualBallotId, string memory spoiltBallotIpfs) public{
        if(msg.sender != votingAuthority){
            revert("Only voting authority is allowed to spoil ballots.");
        }

        spoiltBallots[ballotId] = SpoiltBallot(virtualBallotId, spoiltBallotIpfs);
    }

    function publishShortCodes(string memory ballotId, string[] memory shortCodes) public{
        if(msg.sender != votingAuthority){
            revert("Only voting authority is allowed publish the selected short codes");
        }

        selectedShortCodes[ballotId] = shortCodes;
    }

    function retrieveBallot(string memory ballotId) public view returns (PaperBallot memory){
        return paperBallots[ballotId];
    }

    function publishResult(uint option, uint tally, string memory tallyProofsIpfs) public {
        if(msg.sender != votingAuthority){
            revert("Only voting authority is allowed to publish results.");
        }

        results[option] = Result(options[option], tally, tallyProofsIpfs);
    }

    function getNumberOfBallots() public view returns (uint) {
        return ballotIds.length;
    }
}
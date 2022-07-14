import {AbiItem} from "web3-utils";

export const ElectionABI: AbiItem[] = [{"inputs":[],"stateMutability":"nonpayable","type":"constructor"},{"inputs":[{"internalType":"uint256","name":"","type":"uint256"}],"name":"ballotIds","outputs":[{"internalType":"string","name":"","type":"string"}],"stateMutability":"view","type":"function"},{"inputs":[{"internalType":"string","name":"","type":"string"}],"name":"castBallots","outputs":[{"internalType":"string","name":"ballotId","type":"string"},{"internalType":"string","name":"ballotCode","type":"string"},{"internalType":"string","name":"ballotIpfs","type":"string"}],"stateMutability":"view","type":"function"},{"inputs":[],"name":"electionParameters","outputs":[{"internalType":"string","name":"electionId","type":"string"},{"components":[{"internalType":"string","name":"p","type":"string"},{"internalType":"string","name":"g","type":"string"}],"internalType":"struct Election.ElGamalParameters","name":"elGamalParameters","type":"tuple"},{"internalType":"string","name":"publicKey","type":"string"}],"stateMutability":"view","type":"function"},{"inputs":[{"internalType":"uint256","name":"startIndex","type":"uint256"},{"internalType":"uint256","name":"partitionSize","type":"uint256"}],"name":"getAllBallotIds","outputs":[{"internalType":"string[]","name":"","type":"string[]"},{"internalType":"uint256","name":"","type":"uint256"}],"stateMutability":"view","type":"function"},{"inputs":[],"name":"getElectionParameters","outputs":[{"components":[{"internalType":"string","name":"electionId","type":"string"},{"components":[{"internalType":"string","name":"p","type":"string"},{"internalType":"string","name":"g","type":"string"}],"internalType":"struct Election.ElGamalParameters","name":"elGamalParameters","type":"tuple"},{"internalType":"string","name":"publicKey","type":"string"},{"internalType":"string[]","name":"consensusNodePublicKeys","type":"string[]"}],"internalType":"struct Election.ElectionParameters","name":"","type":"tuple"}],"stateMutability":"view","type":"function"},{"inputs":[],"name":"getNumberOfBallots","outputs":[{"internalType":"uint256","name":"","type":"uint256"}],"stateMutability":"view","type":"function"},{"inputs":[],"name":"getResults","outputs":[{"components":[{"internalType":"string","name":"option","type":"string"},{"internalType":"uint256","name":"tally","type":"uint256"}],"internalType":"struct Election.Result[]","name":"","type":"tuple[]"}],"stateMutability":"view","type":"function"},{"inputs":[{"internalType":"uint256","name":"","type":"uint256"}],"name":"options","outputs":[{"internalType":"string","name":"","type":"string"}],"stateMutability":"view","type":"function"},{"inputs":[{"internalType":"string","name":"ballotId","type":"string"},{"internalType":"string","name":"ballotCode","type":"string"},{"internalType":"string[]","name":"shortCodes","type":"string[]"}],"name":"publishBallotSelection","outputs":[],"stateMutability":"nonpayable","type":"function"},{"inputs":[{"components":[{"internalType":"string","name":"option","type":"string"},{"internalType":"uint256","name":"tally","type":"uint256"}],"internalType":"struct Election.Result[]","name":"tallyResults","type":"tuple[]"},{"internalType":"string","name":"tallyProofsIpfs","type":"string"}],"name":"publishResult","outputs":[],"stateMutability":"nonpayable","type":"function"},{"inputs":[],"name":"resultEvidenceIpfs","outputs":[{"internalType":"string","name":"","type":"string"}],"stateMutability":"view","type":"function"},{"inputs":[{"internalType":"uint256","name":"","type":"uint256"}],"name":"results","outputs":[{"internalType":"string","name":"option","type":"string"},{"internalType":"uint256","name":"tally","type":"uint256"}],"stateMutability":"view","type":"function"},{"inputs":[{"internalType":"string","name":"ballotId","type":"string"}],"name":"retrieveBallot","outputs":[{"components":[{"internalType":"string","name":"ballotId","type":"string"},{"internalType":"string","name":"ballot1Code","type":"string"},{"internalType":"string","name":"ballot1Ipfs","type":"string"},{"internalType":"string","name":"ballot2Code","type":"string"},{"internalType":"string","name":"ballot2Ipfs","type":"string"}],"internalType":"struct Election.PaperBallot","name":"","type":"tuple"}],"stateMutability":"view","type":"function"},{"inputs":[{"internalType":"string","name":"ballotId","type":"string"}],"name":"retrieveCastBallot","outputs":[{"components":[{"internalType":"string","name":"ballotId","type":"string"},{"internalType":"string","name":"ballotCode","type":"string"},{"internalType":"string","name":"ballotIpfs","type":"string"},{"internalType":"string[]","name":"selection","type":"string[]"}],"internalType":"struct Election.CastBallot","name":"","type":"tuple"}],"stateMutability":"view","type":"function"},{"inputs":[{"internalType":"string[]","name":"candidates","type":"string[]"},{"internalType":"string","name":"id","type":"string"},{"components":[{"internalType":"string","name":"p","type":"string"},{"internalType":"string","name":"g","type":"string"}],"internalType":"struct Election.ElGamalParameters","name":"parameters","type":"tuple"},{"internalType":"string","name":"electionPublicKey","type":"string"},{"internalType":"string[]","name":"consensusPublicKeys","type":"string[]"}],"name":"setUp","outputs":[],"stateMutability":"nonpayable","type":"function"},{"inputs":[{"internalType":"string","name":"ballotId","type":"string"},{"internalType":"string","name":"virtualBallotId","type":"string"},{"internalType":"string","name":"spoiltBallotIpfs","type":"string"}],"name":"spoilBallot","outputs":[],"stateMutability":"nonpayable","type":"function"},{"inputs":[{"internalType":"string","name":"","type":"string"}],"name":"spoiltBallots","outputs":[{"internalType":"string","name":"code","type":"string"},{"internalType":"string","name":"ballotIpfs","type":"string"}],"stateMutability":"view","type":"function"},{"inputs":[{"components":[{"internalType":"string","name":"ballotId","type":"string"},{"internalType":"string","name":"ballot1Code","type":"string"},{"internalType":"string","name":"ballot1Ipfs","type":"string"},{"internalType":"string","name":"ballot2Code","type":"string"},{"internalType":"string","name":"ballot2Ipfs","type":"string"}],"internalType":"struct Election.PaperBallot[]","name":"ballots","type":"tuple[]"}],"name":"storeBallot","outputs":[],"stateMutability":"nonpayable","type":"function"},{"inputs":[],"name":"votingAuthority","outputs":[{"internalType":"address","name":"","type":"address"}],"stateMutability":"view","type":"function"}];
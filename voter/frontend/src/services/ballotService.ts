import {VirtualBallotDto} from "../cryptography/ballot";
import Web3 from "web3";
import {Contract} from "web3-eth-contract";
import {ElectionABI} from "../contract/electionContract";
import {EncryptedBallot} from "../cryptography/encryptedBallot";
import {BallotFactory} from "../factory/BallotFactory";


export class BallotService {
    electionContract: Contract;

    constructor(contractAddress: string) {
        const web3 = new Web3("ws://localhost:8546");

        this.electionContract = new web3.eth.Contract(ElectionABI, contractAddress);
    }

    async getEncryptedBallots(ballotId: string): Promise<EncryptedBallot[]> {
        let encryptedBallots: EncryptedBallot[] = [];

        let result: string[] = await this.electionContract.methods.retrieveBallot(ballotId).call();
        const cidFirstBallot: string = result[2];
        const cidSecondBallot: string = result[4];

        encryptedBallots.push(await this.getBallotFromIpfs(cidFirstBallot));
        encryptedBallots.push(await this.getBallotFromIpfs(cidSecondBallot));

        return encryptedBallots;
    }

    private static async getBallotFromIpfs(cid: string): Promise<EncryptedBallot> {
        let encryptedBallot: EncryptedBallot;

        let result: Response = await fetch(`http://localhost:8080/ipfs/${cid}`);

        let blob = await result.blob();

        let text = await blob.text();

        let ballot: VirtualBallotDto = JSON.parse(text);

        encryptedBallot = BallotFactory.CreateEncryptedBallot(ballot);

        console.log(encryptedBallot);

        return encryptedBallot;
    }
}
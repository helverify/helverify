import {SpoiltBallotDto, VirtualBallotDto} from "../cryptography/ballot";
import Web3 from "web3";
import {Contract} from "web3-eth-contract";
import {ElectionABI} from "../contract/electionContract";
import {EncryptedBallot} from "../cryptography/encryptedBallot";
import {BallotFactory} from "../factory/BallotFactory";
import {SpoiltBallot} from "../ballot/spoiltBallot";


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

        encryptedBallots.push(await BallotService.getEncryptedBallotFromIpfs(cidFirstBallot));
        encryptedBallots.push(await BallotService.getEncryptedBallotFromIpfs(cidSecondBallot));

        return encryptedBallots;
    }

    async getSpoiltBallot(ballotId: string) : Promise<SpoiltBallot> {
        let result: string[] = await this.electionContract.methods.retrieveSpoiltBallot(ballotId).call();

        return await BallotService.getSpoiltBallotFromIpfs(result[1]);
    }

    private static async getEncryptedBallotFromIpfs(cid: string): Promise<EncryptedBallot> {
        let encryptedBallot: EncryptedBallot;

        let text: string = await BallotService.getJsonFromIpfs(cid);

        let ballot: VirtualBallotDto = JSON.parse(text);

        encryptedBallot = BallotFactory.CreateEncryptedBallot(ballot);

        return encryptedBallot;
    }

    private static async getSpoiltBallotFromIpfs(cid: string): Promise<SpoiltBallot> {
        let spoiltBallot: SpoiltBallot;

        let text: string = await BallotService.getJsonFromIpfs(cid);

        let ballot: SpoiltBallotDto = JSON.parse(text);

        spoiltBallot = BallotFactory.CreateSpoiltBallot(ballot);

        return spoiltBallot;
    }

    private static async getJsonFromIpfs(cid: string): Promise<string>{
        let result: Response = await fetch(`http://localhost:8080/ipfs/${cid}`);

        let blob = await result.blob();

        return await blob.text();
    }
}
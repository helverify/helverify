import {SpoiltBallotDto, VirtualBallotDto} from "../ballot/ballotDto";
import Web3 from "web3";
import {Contract} from "web3-eth-contract";
import {ElectionABI} from "../contract/electionContract";
import {EncryptedBallot, EncryptedOption} from "../ballot/encryptedBallot";
import {BallotFactory} from "../factory/ballotFactory";
import {SpoiltBallot} from "../ballot/spoiltBallot";
import {CastBallot} from "../ballot/castBallot";
import {ElectionParameters} from "../election/election";


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

        return await BallotService.getSpoiltBallotFromIpfs(result[0], result[1]);
    }

    async getCastBallot(ballotId: string): Promise<CastBallot> {
        let result: string[] = await this.electionContract.methods.retrieveCastBallot(ballotId).call();

        return BallotFactory.createCastBallot(result);
    }

    static verifyEncryptions(spoiltBallot: SpoiltBallot, encryptedBallot: EncryptedBallot, electionParameters: ElectionParameters): boolean {
        return spoiltBallot.options.every((o): boolean => {
            const shortCode: string = o.shortCode;

            const encryptedOption: EncryptedOption = encryptedBallot.encryptedOptions.filter(enc => enc.shortCode === shortCode)[0];

            return o.verifyEncryption(electionParameters.p, electionParameters.g, electionParameters.publicKey, encryptedOption.values.map(v => v.cipher));
        })
    }

    private static async getEncryptedBallotFromIpfs(cid: string): Promise<EncryptedBallot> {
        let encryptedBallot: EncryptedBallot;

        let text: string = await BallotService.getJsonFromIpfs(cid);

        let ballot: VirtualBallotDto = JSON.parse(text);

        encryptedBallot = BallotFactory.createEncryptedBallot(ballot);

        return encryptedBallot;
    }

    private static async getSpoiltBallotFromIpfs(ballotId: string, cid: string): Promise<SpoiltBallot> {
        let spoiltBallot: SpoiltBallot;

        let text: string = await BallotService.getJsonFromIpfs(cid);

        let ballot: SpoiltBallotDto = JSON.parse(text);

        spoiltBallot = BallotFactory.createSpoiltBallot(ballotId, ballot);

        return spoiltBallot;
    }

    private static async getJsonFromIpfs(cid: string): Promise<string>{
        let result: Response = await fetch(`http://localhost:8080/ipfs/${cid}`);

        let blob = await result.blob();

        return await blob.text();
    }
}
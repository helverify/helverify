import {SpoiltBallotDto, VirtualBallotDto} from "../ballot/ballotDto";
import Web3 from "web3";
import {Contract} from "web3-eth-contract";
import {ElectionABI} from "../contract/electionContract";
import {EncryptedBallot, EncryptedOption} from "../ballot/encryptedBallot";
import {BallotFactory} from "../factory/ballotFactory";
import {SpoiltBallot} from "../ballot/spoiltBallot";
import {CastBallot} from "../ballot/castBallot";
import {ElectionParameters} from "../election/election";

/**
 * Provides access to all types of ballots for verification.
 */
export class BallotService {

    /**
     * Election Smart Contract
     */
    electionContract: Contract;

    /**
     * Constructor
     * @param contractAddress Address of the Election Smart Contract
     */
    constructor(contractAddress: string) {
        const web3 = new Web3(process.env.REACT_APP_GETH_WS ?? "");

        this.electionContract = new web3.eth.Contract(ElectionABI, contractAddress);
    }

    /**
     * Retrieves the pair of encrypted ballots with the specified ballotId from contract / IPFS
     * @param ballotId Ballot identifier
     */
    async getEncryptedBallots(ballotId: string): Promise<EncryptedBallot[]> {
        let encryptedBallots: EncryptedBallot[] = [];

        let result: string[] = await this.electionContract.methods.retrieveBallot(ballotId).call();
        const cidFirstBallot: string = result[2];
        const cidSecondBallot: string = result[4];

        encryptedBallots.push(await BallotService.getEncryptedBallotFromIpfs(cidFirstBallot));
        encryptedBallots.push(await BallotService.getEncryptedBallotFromIpfs(cidSecondBallot));

        return encryptedBallots;
    }

    /**
     * Retrieves the spoilt ballot for the specified ballotId from contract / IPFS
     * @param ballotId Ballot identifier
     */
    async getSpoiltBallot(ballotId: string) : Promise<SpoiltBallot> {
        let result: string[] = await this.electionContract.methods.retrieveSpoiltBallot(ballotId).call();

        return await BallotService.getSpoiltBallotFromIpfs(result[0], result[1]);
    }

    /**
     * Retrieves the cast ballot (i.e., the short codes of the selected options) from contract
     * @param ballotId Ballot identifier
     */
    async getCastBallot(ballotId: string): Promise<CastBallot> {
        let result: string[] = await this.electionContract.methods.retrieveCastBallot(ballotId).call();

        return BallotFactory.createCastBallot(result);
    }

    /**
     * Verifies that re-encrypting the plaintext values yields the same ciphertexts as in the encrypted ballot.
     * @param spoiltBallot Spoilt ballot from IPFS
     * @param encryptedBallot Encrypted ballot from IPFS
     * @param electionParameters ElGamal parameters
     */
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

    private static async getSpoiltBallotFromIpfs(ballotCode: string, cid: string): Promise<SpoiltBallot> {
        let spoiltBallot: SpoiltBallot;

        let text: string = await BallotService.getJsonFromIpfs(cid);

        let ballot: SpoiltBallotDto = JSON.parse(text);

        spoiltBallot = BallotFactory.createSpoiltBallot(ballotCode, ballot);

        return spoiltBallot;
    }

    private static async getJsonFromIpfs(cid: string): Promise<string>{
        let result: Response = await fetch(`${process.env.REACT_APP_IPFS_URL}/${cid}`);

        let blob = await result.blob();

        return await blob.text();
    }
}
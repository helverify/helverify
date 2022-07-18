import {ElectionResults, OptionTally} from "../election/election";
import {Contract} from "web3-eth-contract";
import Web3 from "web3";
import {ElectionABI} from "../contract/electionContract";
import {ResultEvidence} from "../election/resultEvidence";
import {EvidenceDto} from "../election/resultEvidenceDto";
import {EvidenceFactory} from "../factory/evidenceFactory";

/**
 * Provides access to election data from contract / IPFS
 */
export class ElectionService {

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
     * Retrieves the final results from contract
     */
    async getFinalResults(): Promise<ElectionResults> {
        const results: any[] = await this.electionContract.methods.getResults().call();

        const optionTallies : OptionTally[] = results.map((r) => {
           const name: string = r[0];
           const count: number = r[1];

           return new OptionTally(name, count);
        });

        return new ElectionResults(optionTallies);
    }

    /**
     * Retrieves the evidence of the final results from contract / IFPS
     */
    async getFinalResultEvidence(): Promise<ResultEvidence> {
        const evidenceCid: string = await this.electionContract.methods.resultEvidenceIpfs().call();

        return ElectionService.getEvidenceFromIpfs(evidenceCid);
    }

    private static async getEvidenceFromIpfs(cid: string): Promise<ResultEvidence> {
        const text: string = await ElectionService.getJsonFromIpfs(cid);

        const evidenceDto: EvidenceDto = JSON.parse(text);

        const evidence: ResultEvidence = EvidenceFactory.createEvidence(evidenceDto);

        return evidence;
    }

    private static async getJsonFromIpfs(cid: string): Promise<string>{
        let result: Response = await fetch(`${process.env.REACT_APP_IPFS_URL}/${cid}`);

        let blob = await result.blob();

        return await blob.text();
    }
}
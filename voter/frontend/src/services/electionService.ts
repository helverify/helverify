import {ElectionResults, OptionTally} from "../election/election";
import {Contract} from "web3-eth-contract";
import Web3 from "web3";
import {ElectionABI} from "../contract/electionContract";
import {ResultEvidence} from "../election/resultEvidence";
import {EvidenceDto} from "../election/resultEvidenceDto";
import {EvidenceFactory} from "../factory/evidenceFactory";

export class ElectionService {
    electionContract: Contract;

    constructor(contractAddress: string) {
        const web3 = new Web3("ws://localhost:8546");

        this.electionContract = new web3.eth.Contract(ElectionABI, contractAddress);
    }

    async getFinalResults(): Promise<ElectionResults> {
        const results: any[] = await this.electionContract.methods.getResults().call();

        const optionTallies : OptionTally[] = results.map((r) => {
           const name: string = r[0];
           const count: number = r[1];

           return new OptionTally(name, count);
        });

        return new ElectionResults(optionTallies);
    }

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
        let result: Response = await fetch(`http://localhost:8080/ipfs/${cid}`);

        let blob = await result.blob();

        return await blob.text();
    }
}
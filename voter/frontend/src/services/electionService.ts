import {ElectionResults, OptionTally} from "../election/election";
import {Contract} from "web3-eth-contract";
import Web3 from "web3";
import {ElectionABI} from "../contract/electionContract";

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
}
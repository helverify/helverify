import {EncryptedBallot} from "../../ballot/encryptedBallot";
import {ElectionParameters} from "../../election/election";

export type ValidationStep = {
    caption: string;
    component: any;
};

export type ValidationStepProps = {
    previous: () => void;
    next: () => void;
    ballots: EncryptedBallot[] | undefined;
    electionParameters: ElectionParameters | undefined;
    ballotId: string | undefined;
}
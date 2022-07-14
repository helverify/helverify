import {EncryptedBallot} from "../cryptography/encryptedBallot";
import {ElectionParameters} from "../election/election";

export type EncryptedBallotVerificationProps = {
    caption: string;
    ballot: EncryptedBallot;
    electionParameters: ElectionParameters;
}

export const EncryptedBallotVerification = (props: EncryptedBallotVerificationProps) => {
    return (
        <>
            <span>{props.caption}</span>
            <span>Proofs: </span>
            <span>{props.ballot.verifyProofs(props.electionParameters.publicKey, props.electionParameters.p, props.electionParameters.g) ? "true" : "false"}</span>
        </>
    );
}
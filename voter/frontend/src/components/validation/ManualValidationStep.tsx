import {ValidationStepProps} from "./validationStep";
import {Button, Typography} from "@mui/material";
import {CastBallotView} from "../CastBallotView";
import {CastBallot} from "../../ballot/castBallot";
import {SpoiltBallot} from "../../ballot/spoiltBallot";
import {SpoiltBallotView} from "../SpoiltBallotView";


export type ManualValidationStepProps = ValidationStepProps & {
    castBallot: CastBallot | undefined;
    spoiltBallot: SpoiltBallot | undefined;
}

export const ManualValidationStep = (props: ManualValidationStepProps) => {
    return(
        <>
            <Typography>Manual Validation</Typography>
                {props.castBallot !== undefined && (
                    <CastBallotView ballot={props.castBallot}/>
                )}
                {props.spoiltBallot !== undefined && props.electionParameters !== undefined && props.ballots !== undefined && (
                    <SpoiltBallotView ballot={props.spoiltBallot} electionParameters={props.electionParameters} encryptions={props.ballots}/>
                )}
            <Button onClick={props.previous}>Previous</Button>
            <Button onClick={props.next}>Next</Button>
        </>
    );
}
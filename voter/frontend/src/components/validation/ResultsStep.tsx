import {ValidationStepProps} from "./validationStep";
import {Button, Typography} from "@mui/material";
import {ResultsView} from "../ResultsView";
import {ElectionResults} from "../../election/election";
import {EvidenceView} from "../EvidenceView";
import {ResultEvidence} from "../../election/resultEvidence";

export type ResultsStepProps = ValidationStepProps & {
    electionResults: ElectionResults | undefined;
    evidence: ResultEvidence | undefined;
}

export const ResultsStep = (props: ResultsStepProps) => {
    return (
        <>
            <Typography>Results</Typography>
            {props.electionResults !== undefined && (
                <ResultsView electionResults={props.electionResults}/>
            )}
            {props.evidence !== undefined && props.electionParameters !== undefined && (
                <EvidenceView electionEvidence={props.evidence} electionParameters={props.electionParameters}/>
            )}
            <Button onClick={props.previous}>Previous</Button>
            <Button onClick={props.next}>Next</Button>
        </>
    );
}
import {ValidationStepProps} from "./validationStep";
import {Box, Stack, Typography} from "@mui/material";
import {ResultsView} from "../ResultsView";
import {ElectionResults} from "../../election/election";
import {EvidenceView} from "../EvidenceView";
import {ResultEvidence} from "../../election/resultEvidence";
import React from "react";

export type ResultsStepProps = ValidationStepProps & {
    electionResults: ElectionResults | undefined;
    evidence: ResultEvidence | undefined;
    setTaR: (value: boolean) => void;
}

export const ResultsStep = (props: ResultsStepProps) => {
    return (
        <Box>
            <Stack direction="column" spacing={1}>
                <Typography variant={"h5"}>Election Results</Typography>
                <Typography>This step shows the results of the elections. Additionally, you can make sure that the
                    results have been decrypted correctly.</Typography>
                {props.electionResults !== undefined && (
                    <Box>
                        <ResultsView
                            electionResults={props.electionResults}
                        />
                    </Box>
                )}
                {props.evidence !== undefined && props.electionParameters !== undefined && (
                    <Box>
                        <EvidenceView
                            electionEvidence={props.evidence}
                            electionParameters={props.electionParameters}
                            setTaR={props.setTaR}
                        />
                    </Box>
                )}
            </Stack>
        </Box>
    );
}
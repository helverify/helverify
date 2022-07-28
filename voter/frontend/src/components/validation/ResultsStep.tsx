import {ValidationStepProps} from "./validationStep";
import {Box, Button, Stack, Typography} from "@mui/material";
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
        <Box>
            <Stack direction="column" spacing={1}>
                <Typography variant={"h5"}>Election Results</Typography>
                <Typography>This step shows the results of the elections. Additionally, you can make sure that the
                    results have been decrypted correctly.</Typography>
                {props.electionResults !== undefined && (
                    <Box>
                        <ResultsView electionResults={props.electionResults}/>
                    </Box>
                )}
                {props.evidence !== undefined && props.electionParameters !== undefined && (
                    <Box>
                        <EvidenceView electionEvidence={props.evidence}
                                      electionParameters={props.electionParameters}/>

                    </Box>
                )}
                <Stack direction="row" spacing={1} style={{marginTop: "10px"}}>
                    <Box display="flex" justifyContent="left" alignItems="left" flexGrow={1}>
                        <Button onClick={props.previous} variant="outlined">Previous</Button>
                    </Box>
                    <Box display="flex" justifyContent="right" alignItems="right">
                        <Button onClick={props.next} variant="contained">Next</Button>
                    </Box>
                </Stack>
            </Stack>
        </Box>
    )
        ;
}
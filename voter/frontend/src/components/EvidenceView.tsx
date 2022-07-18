import {ResultEvidence} from "../election/resultEvidence";
import {Box, Card, Stack, Typography} from "@mui/material";
import {ValidityIcon} from "./ValidityIcon";
import {ElectionParameters} from "../election/election";
import bigInt from "big-integer";

export type EvidenceViewProps = {
    electionParameters: ElectionParameters;
    electionEvidence: ResultEvidence;
}

export const EvidenceView = (props: EvidenceViewProps) => {
    const p: bigInt.BigInteger = props.electionParameters.p;
    const g: bigInt.BigInteger = props.electionParameters.g;

    return (
        <Card>
            <Box sx={{m: 1}}>
                <Stack spacing={1}>
                    <Typography variant="h4">Result</Typography>
                    <Typography variant="h5">Correctness</Typography>
                    <Typography color="text.secondary">Have the final results been decrypted correctly?</Typography>
                    <ValidityIcon isValid={props.electionEvidence.verifyDecryptionProofs(p, g)}/>
                </Stack>
            </Box>
        </Card>
    );
}
import {ValidationStepProps} from "./validationStep";
import {Box, Button, Stack, Typography} from "@mui/material";
import {BallotEncryptionCheck} from "../BallotEncryptionCheck";
import {useState} from "react";

export const AutomaticValidationStep = (props: ValidationStepProps) => {
    const [isLoading, setLoading] = useState<boolean>(true);

    return(
        <Box>
            <Stack direction="column" spacing={1}>
                <Typography variant={"h5"}>Automatic Validation</Typography>
                <Typography>This verification step checks that your ballot has been generated and encrypted properly.</Typography>
                <BallotEncryptionCheck isLoading={isLoading} setLoading={setLoading} ballots={props.ballots} electionParameters={props.electionParameters} ballotId={props.ballotId ?? ""}/>
                <Box display="flex" justifyContent="right" alignItems="right">
                    <Button variant="contained" onClick={props.next}>Next</Button>
                </Box>
            </Stack>
        </Box>
    );
}
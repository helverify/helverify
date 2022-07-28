import {ValidationStepProps} from "./validationStep";
import {Box, Stack, Typography} from "@mui/material";
import {BallotEncryptionCheck} from "../BallotEncryptionCheck";
import React from "react";

export const AutomaticValidationStep = (props: ValidationStepProps) => {
    return(
        <Box>
            <Stack direction="column" spacing={1}>
                <Typography variant={"h5"}>Ballot Authenticity</Typography>
                <Typography>This verification step checks that your ballot has been generated and encrypted properly. It is recommended to perform this step as soon as you receive your ballot by mail.</Typography>
                <BallotEncryptionCheck ballots={props.ballots} electionParameters={props.electionParameters} ballotId={props.ballotId ?? ""}/>
            </Stack>
        </Box>
    );
}
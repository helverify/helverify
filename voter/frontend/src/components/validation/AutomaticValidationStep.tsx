import {ValidationStepProps} from "./validationStep";
import {Box, Checkbox, Stack, Typography} from "@mui/material";
import {BallotEncryptionCheck} from "../BallotEncryptionCheck";
import React from "react";

export type AutomaticValidationStepProps = ValidationStepProps & {
  setCaI: (value: boolean) => void;
  isCaI: boolean;
};

export const AutomaticValidationStep = (props: AutomaticValidationStepProps) => {

    const handleCheckBoxChange = (event: any) => {
        props.setCaI(event.target.checked);
    }

    return(
        <Box>
            <Stack direction="column" spacing={1}>
                <Typography variant={"h5"}>Ballot Authenticity</Typography>
                <Typography>This verification step checks that your ballot has been generated and encrypted properly. It is recommended to perform this step as soon as you receive your ballot by mail.</Typography>
                <BallotEncryptionCheck ballots={props.ballots} electionParameters={props.electionParameters} ballotId={props.ballotId ?? ""}/>
                <Box display={"flex"} alignItems={"center"}>
                    <Checkbox onChange={handleCheckBoxChange} checked={props.isCaI}/>
                    <Typography>I confirm that I have checked my selections before casting the ballot.</Typography>
                </Box>
            </Stack>
        </Box>
    );
}
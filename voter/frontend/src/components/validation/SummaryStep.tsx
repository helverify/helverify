import {ValidationStepProps} from "./validationStep";
import {Box, Stack, Typography} from "@mui/material";

export const SummaryStep = (props: ValidationStepProps) => {
    return(
        <>
            <Box>
                <Stack direction="column" spacing={1}>
                    <Typography variant={"h5"}>Verification Complete</Typography>
                    <Typography>That's it, you have finished the verification for this election!</Typography>
                </Stack>
            </Box>
        </>
    );
}
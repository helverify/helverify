import {ValidationStepProps} from "./validationStep";
import {Box, Button, Grid, Stack, Typography} from "@mui/material";
import {CastBallotView} from "../CastBallotView";
import {CastBallot} from "../../ballot/castBallot";
import {SpoiltBallot} from "../../ballot/spoiltBallot";
import {SpoiltBallotView} from "../SpoiltBallotView";

export type ManualValidationStepProps = ValidationStepProps & {
    castBallot: CastBallot | undefined;
    spoiltBallot: SpoiltBallot | undefined;
}

export const ManualValidationStep = (props: ManualValidationStepProps) => {
    return (
        <>
            <Typography variant={"h5"}>Manual Validation</Typography>
            <Typography>This verification step allows you to (a) verify that your selections have been registered
                correctly and (b) verify that the unselected column of your ballot matches with the short codes shown
                here.</Typography>
            <Grid container>
                {props.castBallot !== undefined && (
                    <Grid item xs={12} sm={6}>
                        <CastBallotView ballot={props.castBallot}/>
                    </Grid>
                )}
                {props.spoiltBallot !== undefined && props.electionParameters !== undefined && props.ballots !== undefined && (
                    <Grid item xs={12} sm={6}>
                        <SpoiltBallotView ballot={props.spoiltBallot} electionParameters={props.electionParameters}
                                          encryptions={props.ballots}/>
                    </Grid>
                )}
            </Grid>
            <Stack direction="row" spacing={1} style={{marginTop: "10px"}}>
                <Box display="flex" justifyContent="left" alignItems="left" flexGrow={1}>
                    <Button onClick={props.previous} variant="outlined">Previous</Button>
                </Box>
                <Box display="flex" justifyContent="right" alignItems="right">
                    <Button onClick={props.next} variant="contained">Next</Button>
                </Box>
            </Stack>
        </>
    );
}
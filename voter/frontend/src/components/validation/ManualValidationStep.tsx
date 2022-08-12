import {ValidationStepProps} from "./validationStep";
import {Box, Checkbox, Grid, Stack, Typography} from "@mui/material";
import {CastBallotView} from "../CastBallotView";
import {CastBallot} from "../../ballot/castBallot";
import {SpoiltBallot} from "../../ballot/spoiltBallot";
import {SpoiltBallotView} from "../SpoiltBallotView";

export type ManualValidationStepProps = ValidationStepProps & {
    castBallot: CastBallot | undefined;
    spoiltBallot: SpoiltBallot | undefined;
    isRaC: boolean;
    setRaC: (value: boolean) => void;
}

export const ManualValidationStep = (props: ManualValidationStepProps) => {

    const handleCheckBoxChange = (event: any) => {
        props.setRaC(event.target.checked);
    }

    return (
        <Box>
            <Stack direction="column" spacing={1}>
                <Typography variant={"h5"}>Manual Validation</Typography>
                <Typography>This verification step allows you to (a) verify that your selections have been registered
                    correctly and (b) verify that the unselected column of your ballot matches with the short codes
                    shown
                    here.</Typography>
                <Grid container>
                    {props.castBallot !== undefined && (
                        <Grid item xs={12} sm={6}>
                            <Box sx={{m: 1, marginLeft: 0}} marginRight={{xs: 0, sm: 1}}>
                                <CastBallotView ballot={props.castBallot}/>
                            </Box>
                        </Grid>
                    )}
                    {props.spoiltBallot !== undefined && props.electionParameters !== undefined && props.ballots !== undefined && (
                        <Grid item xs={12} sm={6}>
                            <Box sx={{m: 1, marginRight: 0}} marginLeft={{xs: 0, sm: 1}}>
                                <SpoiltBallotView ballot={props.spoiltBallot}
                                                  electionParameters={props.electionParameters}
                                                  encryptions={props.ballots}/>
                            </Box>
                        </Grid>
                    )}
                </Grid>
                <Box display={"flex"} alignItems={"center"}>
                    <Checkbox onChange={handleCheckBoxChange} checked={props.isRaC}/>
                    <Typography>I confirm that the selected short codes are identical to my selections on the ballot.</Typography>
                </Box>
            </Stack>
        </Box>
    );
}
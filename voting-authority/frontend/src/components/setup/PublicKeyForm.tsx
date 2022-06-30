import {SetupStepProps} from "./electionSetupStep";
import {Button, Card, FormControl, Grid, Stack} from "@mui/material";
import {apiClient} from "../../api/apiClient";

export const PublicKeyForm = (props: SetupStepProps) => {

    const generatePublicKey = () => {
        if(props.election.id === undefined || props.election.id === null){
            return;
        }

        const electionId: string = props.election.id;

        apiClient().api.electionsPublicKeyUpdate(electionId).then((result) => props.next(result.data, props.blockchain));
    };

    return(
        <Grid container spacing={1}>
            <Grid item xs={6}>
                <Card>
                    <Stack spacing={1} sx={{m: 2}}>
                        <FormControl>
                            <Button variant="contained" onClick={generatePublicKey}>Generate Public Key</Button>
                        </FormControl>
                    </Stack>
                </Card>
            </Grid>
        </Grid>
    );
}
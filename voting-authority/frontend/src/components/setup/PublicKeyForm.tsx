import {SetupStepProps} from "./electionSetupStep";
import {Button, Card, FormControl, Grid, Stack} from "@mui/material";
import {Api} from "../../Api";

export const PublicKeyForm = (props: SetupStepProps) => {

    const generatePublicKey = () => {
        if(props.election.id === undefined || props.election.id === null){
            return;
        }

        const client = new Api({
            baseUrl: "http://localhost:5000"
        });

        const electionId: string = props.election.id;

        client.api.electionsPublicKeyUpdate(electionId).then(() => props.next(props.election));
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
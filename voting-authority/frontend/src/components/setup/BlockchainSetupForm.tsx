import {SetupStepProps} from "./electionSetupStep";
import {Backdrop, Button, Card, CircularProgress, FormControl, Grid, Stack} from "@mui/material";
import {Api} from "../../Api";
import {useState} from "react";

export const BlockchainSetupForm = (props: SetupStepProps) => {

    const [isLoading, setLoading] = useState<boolean>(false);

    const setUpBlockchain = () => {
        if (props.election.id === undefined || props.election.id === null) {
            return;
        }

        const client = new Api({
            baseUrl: "http://localhost:5000"
        });

        const electionId: string = props.election.id;
        setLoading(true);
        client.api.electionsRegistrationsBlockchainSetupCreate(electionId).then(() => {
            setLoading(false);
            props.next(props.election)
        });
    }


    return (
        <>
        <Backdrop open={isLoading}>
            <CircularProgress />
        </Backdrop>
            <Grid container spacing={1}>
                <Grid item xs={6}>
                    <Card>
                        <Stack spacing={1} sx={{m: 2}}>
                            <FormControl>
                                <Button variant="contained" onClick={setUpBlockchain}>
                                    Set up Blockchain
                                </Button>
                            </FormControl>
                        </Stack>
                    </Card>
                </Grid>
            </Grid>
        </>

    );
}
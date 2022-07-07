import {SetupStepProps} from "./electionSetupStep";
import {Backdrop, Button, Card, CircularProgress, FormControl, Grid, Stack} from "@mui/material";
import {apiClient} from "../../api/apiClient";
import {useState} from "react";

export const ContractDeploymentForm = (props: SetupStepProps) => {

    const [isLoading, setLoading] = useState<boolean>(false);

    const deployContract = () => {
        if(props.election.id === undefined || props.election.id === null){
            return;
        }
        setLoading(true);

        apiClient().api.electionsContractCreate(props.election.id).then(() =>  {
            setLoading(false);
            props.next(props.election, props.blockchain);
        });
    }

    return(
        <>
            <Backdrop open={isLoading}>
                <CircularProgress />
            </Backdrop>
            <Grid container spacing={1}>
                <Grid item xs={6}>
                    <Card>
                        <Stack spacing={1} sx={{m: 2}}>
                            <FormControl>
                                <Button variant="contained" onClick={deployContract}>Deploy Election Smart Contract</Button>
                            </FormControl>
                        </Stack>
                    </Card>
                </Grid>
            </Grid>
        </>
    );
}
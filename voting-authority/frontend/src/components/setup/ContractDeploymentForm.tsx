import {SetupStepProps} from "./electionSetupStep";
import {Button, Card, FormControl, Grid, Stack} from "@mui/material";
import {apiClient} from "../../api/apiClient";

export const ContractDeploymentForm = (props: SetupStepProps) => {

    const deployContract = () => {
        if(props.election.id === undefined || props.election.id === null){
            return;
        }
        apiClient().api.electionsContractCreate(props.election.id).then(() =>  {
           props.next(props.election, props.blockchain);
        });
    }

    return(
        <>
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
import {SetupStepProps} from "./electionSetupStep";
import {Backdrop, Box, Button, CircularProgress, Stack, Typography} from "@mui/material";
import {apiClient} from "../../api/apiClient";
import {useState} from "react";

export const ContractDeploymentForm = (props: SetupStepProps) => {
    const typographyStyle = {marginTop: "25px", marginBottom: "15px"};
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

            <Stack>
                <Box>
                    <Typography variant={"h5"} style={typographyStyle}>Deploy Election Smart Contract</Typography>
                </Box>
                <Box display="flex" alignItems="right" justifyContent="right">
                    <Button variant="contained" onClick={deployContract}>Next</Button>
                </Box>
            </Stack>

        </>
    );
}
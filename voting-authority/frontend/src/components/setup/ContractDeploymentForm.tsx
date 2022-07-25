import {SetupStepProps} from "./electionSetupStep";
import {Box, Button, Stack, Typography} from "@mui/material";
import {apiClient} from "../../api/apiClient";
import {useState} from "react";
import {ProgressWithLabel} from "../progress/ProgressWithLabel";

export const ContractDeploymentForm = (props: SetupStepProps) => {
    const typographyStyle = {marginTop: "25px", marginBottom: "15px"};
    const [isLoading, setLoading] = useState<boolean>(false);

    const deployContract = () => {
        if (props.election.id === undefined || props.election.id === null) {
            return;
        }
        setLoading(true);

        apiClient().api.electionsContractCreate(props.election.id).then(() => {
            setLoading(false);
            props.next(props.election, props.blockchain);
        });
    }

    return (
        <>
            <Stack>
                <Box>
                    <Typography variant={"h5"} style={typographyStyle}>Deploy Election Smart Contract</Typography>
                    <Typography>Almost done, this is the final step. The last thing we need to do is to deploy the election smart contract to the Blockchain in order to be able to use it as a public bulletin board for the election.</Typography>
                </Box>
                <Box display="flex" alignItems="right" justifyContent="right">
                    <Button variant="contained" onClick={deployContract}>Next</Button>
                </Box>
            </Stack>
            <ProgressWithLabel isLoading={isLoading} label="Deploying Smart Contract"/>
        </>
    );
}
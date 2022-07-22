import {SetupStepProps} from "./electionSetupStep";
import {Box, Button, Stack, Typography} from "@mui/material";
import {apiClient} from "../../api/apiClient";

export const PublicKeyForm = (props: SetupStepProps) => {
    const typographyStyle = {marginTop: "25px", marginBottom: "15px"};

    const generatePublicKey = () => {
        if(props.election.id === undefined || props.election.id === null){
            return;
        }

        const electionId: string = props.election.id;

        apiClient().api.electionsPublicKeyUpdate(electionId).then((result) => props.next(result.data, props.blockchain));
    };

    return(
        <Stack>
            <Box>
                <Typography variant={"h5"} style={typographyStyle}>Generate Public Key</Typography>
            </Box>
            <Box display="flex" alignItems="right" justifyContent="right">
                <Button variant="contained" onClick={generatePublicKey}>Next</Button>
            </Box>
        </Stack>
    );
}
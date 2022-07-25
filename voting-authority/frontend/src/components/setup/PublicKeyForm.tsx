import {SetupStepProps} from "./electionSetupStep";
import {Box, Button, Stack, Typography} from "@mui/material";
import {apiClient} from "../../api/apiClient";
import {useState} from "react";
import {ProgressWithLabel} from "../progress/ProgressWithLabel";

export const PublicKeyForm = (props: SetupStepProps) => {
    const typographyStyle = {marginTop: "25px", marginBottom: "15px"};

    const [isLoading, setLoading] = useState<boolean>(false);

    const generatePublicKey = () => {
        if(props.election.id === undefined || props.election.id === null){
            return;
        }
        setLoading(true);

        const electionId: string = props.election.id;

        apiClient().api.electionsPublicKeyUpdate(electionId).then((result) => {
            setLoading(false);
            props.next(result.data, props.blockchain);
        });
    };

    return(
        <>
            <Stack>
                <Box>
                    <Typography variant={"h5"} style={typographyStyle}>Generate Public Key</Typography>
                </Box>
                <Box display="flex" alignItems="right" justifyContent="right">
                    <Button variant="contained" onClick={generatePublicKey}>Next</Button>
                </Box>
            </Stack>
            <ProgressWithLabel isLoading={isLoading} label="Generating Public Key"/>
        </>
    );
}
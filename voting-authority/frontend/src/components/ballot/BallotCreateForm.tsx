import {apiClient} from "../../api/apiClient";
import React, {useState} from "react";
import {BallotGenerationDto} from "../../api/Api";
import {BallotForm} from "./BallotForm";
import {Box, Container, Paper, Typography} from "@mui/material";

export type BallotCreateFormProps = {
    electionId: string;
    close: () => void;
};

export const BallotCreateForm = (props: BallotCreateFormProps) => {
    const [isLoading, setLoading] = useState<boolean>(false);

    const createBallots = (numberOfBallots: number) => {
        setLoading(true);

        if (props.electionId === undefined || props.electionId === null) {
            props.close();
            return;
        }

        const id: string = props.electionId ?? "";

        const ballotGenerationData: BallotGenerationDto = {
            numberOfBallots: numberOfBallots
        };

        apiClient().api.electionsBallotsCreate(id, ballotGenerationData).then(() => {
            setLoading(false);
            props.close();
        });
    };

    return (
        <Container maxWidth={"xs"}>
            <Box style={{ position: "fixed", top: "45%"}}>
                <Paper variant="outlined" style={{minWidth: "450px"}}>
                    <Typography variant="h4" align="center" sx={{m: 2}}>Create Ballots</Typography>
                    <BallotForm buttonCaption="Create Ballots" buttonAction={createBallots} isLoading={isLoading}
                                loadingLabel="Creating ballots"/>
                </Paper>
            </Box>
        </Container>
    );
}
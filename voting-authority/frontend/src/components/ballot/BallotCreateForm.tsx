import {apiClient} from "../../api/apiClient";
import React, {useState} from "react";
import {BallotGenerationDto} from "../../api/Api";
import {useNavigate, useParams} from "react-router-dom";
import {BallotForm} from "./BallotForm";
import {Container, Paper, Typography} from "@mui/material";

export const BallotCreateForm = () => {
    const [isLoading, setLoading] = useState(false);

    const navigate = useNavigate();

    const {electionId} = useParams();

    const createBallots = (numberOfBallots: number) => {
        setLoading(true);

        if (electionId === undefined || electionId === null) {
            navigate("/elections");
        }

        const id: string = electionId ?? "";

        const ballotGenerationData: BallotGenerationDto = {
            numberOfBallots: numberOfBallots
        };

        apiClient().api.electionsBallotsCreate(id, ballotGenerationData).then(() => {
            setLoading(false);
            navigate("/elections");
        });
    };


    return (
        <Container maxWidth={"sm"}>
            <Paper variant="outlined" style={{minWidth: "450px"}}>
                <Typography variant="h4" align="center" sx={{m: 2}}>Create Ballots</Typography>
                <BallotForm buttonCaption="Create Ballots" buttonAction={createBallots} isLoading={isLoading}/>
            </Paper>
        </Container>

    );
}
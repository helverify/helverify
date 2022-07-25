import React, {useState} from "react";
import {useNavigate, useParams} from "react-router-dom";
import {BallotForm} from "./BallotForm";
import {Container, Paper, Typography} from "@mui/material";

export const BallotPrintForm = () => {
    const [isLoading, setLoading] = useState(false);

    const navigate = useNavigate();

    const {electionId} = useParams();

    const printBallots = (numberOfBallots: number) => {
        if (electionId === undefined || electionId === null || numberOfBallots === 0) {
            navigate("/elections");
            return;
        }

        setLoading(true);

        const url: string = `${process.env.REACT_APP_VA_BACKEND}/api/elections/${electionId}/ballots/pdf?numberOfBallots=${numberOfBallots}`;

        // inspired by https://stackoverflow.com/questions/66811401/material-ui-how-to-download-a-file-when-clicking-a-button
        const downloadLink = document.createElement("a");

        downloadLink.href = url;
        downloadLink.target = "about:tab";

        try {
            downloadLink.click();
        } finally {
            setLoading(false);
            navigate("/elections");
        }
    };


    return (
        <Container maxWidth={"sm"}>
            <Paper variant="outlined" style={{minWidth: "450px"}}>
                <Typography variant="h4" align="center" sx={{m: 2}}>Create Ballots</Typography>
                <BallotForm buttonCaption="Print Ballots" buttonAction={printBallots} isLoading={isLoading} loadingLabel="Printing ballots"/>
            </Paper>
        </Container>
    );
}
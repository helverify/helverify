import React, {useState} from "react";
import {BallotForm} from "./BallotForm";
import {Box, Container, Paper, Typography} from "@mui/material";

export type BallotPrintFormProps = {
    electionId: string;
    close: () => void;
}

export const BallotPrintForm = (props: BallotPrintFormProps) => {
    const [isLoading, setLoading] = useState(false);

    const printBallots = (numberOfBallots: number) => {
        if (props.electionId === undefined || props.electionId === null || numberOfBallots === 0) {
            props.close();
            return;
        }

        setLoading(true);

        const url: string = `${process.env.REACT_APP_VA_BACKEND}/api/elections/${props.electionId}/ballots/pdf?numberOfBallots=${numberOfBallots}`;

        // inspired by https://stackoverflow.com/questions/66811401/material-ui-how-to-download-a-file-when-clicking-a-button
        const downloadLink = document.createElement("a");

        downloadLink.href = url;
        downloadLink.target = "about:tab";

        try {
            downloadLink.click();
        } finally {
            setLoading(false);
            props.close();
        }
    };


    return (
        <Container maxWidth={"xs"}>
            <Box style={{position: "fixed", top: "45%"}}>
                <Paper variant="outlined" style={{minWidth: "450px"}}>
                    <Typography variant="h4" align="center" sx={{m: 2}}>Create Ballots</Typography>
                    <BallotForm buttonCaption="Print Ballots" buttonAction={printBallots} isLoading={isLoading}
                                loadingLabel="Printing ballots"/>
                </Paper>
            </Box>
        </Container>
    );
}
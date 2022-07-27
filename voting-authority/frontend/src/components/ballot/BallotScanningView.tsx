import {QrReader} from "react-qr-reader";
import React, {useState} from "react";
import {
    Alert, Box,
    Container, Paper, Snackbar,
    Stack, Typography
} from "@mui/material";
import {apiClient} from "../../api/apiClient";
import {ElectionDto, PrintBallotDto} from "../../api/Api";
import {BallotChoiceForm} from "./BallotChoiceForm";
import {ElectionGrid} from "../election/ElectionGrid";

type QrData = {
    electionId: string
    ballotId: string
}

export const BallotScanningView = () => {
    const [election, setElection] = useState<ElectionDto>({});
    const [ballot, setBallot] = useState<PrintBallotDto>({});
    const [success, setSuccess] = useState<string>("");

    const isCameraEnabled: boolean = ballot.ballotId === undefined;

    const loadElection = (data: QrData) => {
        apiClient().api.electionsDetail(data.electionId).then((result) => setElection(result.data));
    };

    const loadBallot = (data: QrData) => {
        apiClient().api.electionsBallotsDetail(data.electionId, {id: data.ballotId}).then((result) => setBallot(result.data));
    };

    const reset = () => {
        setSuccess(`Successfully stored the selections for ballot ${ballot.ballotId}`);
        setElection({});
        setBallot({});
    };

    return (
        <>
            <Container maxWidth={"md"}>
                <Stack direction={"column"} spacing={1}>
                    <Paper variant="outlined" style={{minWidth: "450px"}}>
                        <Typography variant="h4" align="center" sx={{m: 2}}>Register Ballots</Typography>
                        <Stack direction={"column"} spacing={1}>
                            <Stack direction={"column"} spacing={1} style={{width: "100%"}}>
                                {isCameraEnabled && (<QrReader
                                    onResult={(res, err) => {
                                        let text = res?.getText();
                                        if (!!text) {
                                            let data = JSON.parse(text);

                                            const qrData: QrData = {
                                                electionId: data.ElectionId,
                                                ballotId: data.BallotId
                                            };

                                            loadElection(qrData);
                                            loadBallot(qrData);
                                        }

                                        if (err && err.message) {
                                            console.log(err.message)
                                        }
                                    }}
                                    constraints={{facingMode: "environment"}}
                                    containerStyle={{width: "100%"}}
                                />)}
                            </Stack>
                            {!isCameraEnabled && (
                                <BallotChoiceForm ballot={ballot} electionId={election.id ?? ""}
                                                  onSubmit={reset}/>
                            )}
                        </Stack>
                    </Paper>
                </Stack>
                <Box style={{ marginTop: "10px"}}>
                    {election.id !== undefined && (
                        <ElectionGrid election={election} />
                    )}
                </Box>
            </Container>
            <Snackbar
                open={success !== ""}
                onClose={() => setSuccess("")}
                autoHideDuration={3000}
            >
                <Alert severity="success">{success}</Alert>
            </Snackbar>
        </>
    );
};

//https://stackoverflow.com/questions/51940157/how-to-align-horizontal-icon-and-text-in-mui
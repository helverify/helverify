import {QrReader} from "react-qr-reader";
import React, {useState} from "react";
import {Alert, Box, Container, Paper, Snackbar, Stack, Typography} from "@mui/material";
import {apiClient} from "../../api/apiClient";
import {ElectionDto, ElectionStatisticsDto, PrintBallotDto} from "../../api/Api";
import {BallotChoiceForm} from "./BallotChoiceForm";
import {ElectionInfoTally} from "../election/ElectionInfoTally";

type QrData = {
    electionId: string
    ballotId: string
}

export const BallotScanningView = () => {
    const [election, setElection] = useState<ElectionDto>({});
    const [stats, setStats] = useState<ElectionStatisticsDto>({});
    const [ballot, setBallot] = useState<PrintBallotDto>({});
    const [success, setSuccess] = useState<string>("");

    const isCameraEnabled: boolean = ballot.ballotId === undefined;

    const loadElection = (data: QrData) => {
        apiClient().api.electionsDetail(data.electionId).then((result) => setElection(result.data));
        apiClient().api.electionsStatisticsDetail(data.electionId).then((result) => setStats(result.data));
    };

    const loadBallot = (data: QrData) => {
        apiClient().api.electionsBallotsDetail(data.electionId, {id: data.ballotId}).then((result) => setBallot(result.data));
    };

    const reset = () => {
        setSuccess(`Successfully stored the selections for ballot ${ballot.ballotId}`);
        apiClient().api.electionsStatisticsDetail(election.id ?? "").then((result) => setStats(result.data));
        setBallot({});
    };

    return (
        <>
            <Container maxWidth={"md"}>
                <Box style={{marginTop: "10px"}}>
                    {election.id !== undefined && (
                        <>
                            <ElectionInfoTally election={election} statistics={stats}/>
                        </>
                    )}
                </Box>
                <Stack direction={"column"} spacing={1} style={{marginTop: "10px"}}>
                    <Paper variant="outlined" style={{minWidth: "450px"}}>
                        <Typography variant="h4" align="center" sx={{m: 2}}>Tallying</Typography>
                        <Stack direction={"column"} spacing={1}>
                            <Stack direction={"column"} spacing={1} style={{width: "100%"}}>
                                {isCameraEnabled && (
                                    <>
                                        <Typography variant="h5" align="center" sx={{m: 2}}>Please scan the ballot
                                            QR-Code</Typography>
                                        <QrReader
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
                                        />
                                    </>
                                )}
                            </Stack>
                            {!isCameraEnabled && (
                                <BallotChoiceForm ballot={ballot} electionId={election.id ?? ""}
                                                  onSubmit={reset}/>
                            )}
                        </Stack>
                    </Paper>
                </Stack>
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
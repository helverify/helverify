import {CameraAlt, QrCodeScanner} from "@mui/icons-material";
import {QrReader} from "react-qr-reader";
import React, {useState} from "react";
import {
    Box,
    Button,
    Card, Container, Paper,
    Stack, Typography
} from "@mui/material";
import {apiClient} from "../../api/apiClient";
import {ElectionDto, PrintBallotDto} from "../../api/Api";
import {ElectionInfo} from "../election/ElectionInfo";
import {BallotChoiceForm} from "./BallotChoiceForm";

type QrData = {
    electionId: string
    ballotId: string
}

export const BallotRegistrationView = () => {
    const [election, setElection] = useState<ElectionDto>({});
    const [ballot, setBallot] = useState<PrintBallotDto>({});
    const [isCameraEnabled, setCameraEnabled] = useState<boolean>(true);

    const toggleCamera = () => {
        setCameraEnabled(!isCameraEnabled);
    }

    const loadElection = (data: QrData) => {
        apiClient().api.electionsDetail(data.electionId).then((result) => setElection(result.data));
    }

    const loadBallot = (data: QrData) => {
        apiClient().api.electionsBallotsDetail(data.electionId, {id: data.ballotId}).then((result) => setBallot(result.data));
    }

    return (
        <>
            <Container maxWidth={"md"}>
                <Paper variant="outlined" style={{minWidth: "450px"}}>
                    <Typography variant="h4" align="center" sx={{m: 2}}>Register Ballots</Typography>
                    <Stack direction={"column"} spacing={1}>
                        <ElectionInfo election={election}/>
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
                            {!isCameraEnabled && (
                                <Box
                                    style={{display: "flex", alignItems: "center", justifyContent: "center"}}>
                                    <QrCodeScanner/>
                                </Box>
                            )}
                            <Button onClick={toggleCamera}><CameraAlt/></Button>
                        </Stack>
                        <BallotChoiceForm ballot={ballot} electionId={election.id ?? ""}
                                          onSubmit={() => setBallot({})}/>
                    </Stack>
                </Paper>
            </Container>
        </>
    )
        ;
};

//https://stackoverflow.com/questions/51940157/how-to-align-horizontal-icon-and-text-in-mui
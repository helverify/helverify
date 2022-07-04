import {Camera, CameraAlt, QrCodeScanner} from "@mui/icons-material";
import {QrReader} from "react-qr-reader";
import {useState} from "react";
import {
    Box,
    Button,
    Card,
    CardContent,
    Checkbox,
    FormControl, FormControlLabel, FormLabel, Radio,
    RadioGroup,
    Skeleton,
    Stack,
    Typography
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
            <Stack direction={"column"} spacing={1}>
                <ElectionInfo election={election}/>
                <Stack direction={"row"} spacing={1} >
                    <Card style={{width: "100%", maxWidth: "800px"}}>
                        <Stack direction={"column"} spacing={1} style={{width: "100%"}}>

                            {isCameraEnabled && (<QrReader
                                onResult={(res, err) => {
                                    let text = res?.getText();
                                    if (text === undefined || text === null) {
                                        return;
                                    }

                                    if (text.length > 0) {
                                        let data = JSON.parse(text);

                                        const qrData: QrData = {electionId: data.ElectionId, ballotId: data.BallotId};

                                        loadElection(qrData);
                                        loadBallot(qrData);
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
                    </Card>
                    <BallotChoiceForm ballot={ballot} electionId={election.id ?? ""} onSubmit={() => setBallot({})}/>
                </Stack>
            </Stack>
        </>
    )
        ;
};

//https://stackoverflow.com/questions/51940157/how-to-align-horizontal-icon-and-text-in-mui
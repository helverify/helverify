import React, {useEffect, useState} from 'react';
import './App.css';
import {
    AppBar, Box,
    Card,
    Container,
    createTheme,
    CssBaseline, IconButton,
    Stack,
    ThemeProvider,
    Toolbar,
    Typography
} from '@mui/material';
import Web3 from "web3";
import {ElectionABI} from "./contract/electionContract";
import {QrReader} from "react-qr-reader";
import {ElectionParameters, ElectionResults} from "./election/election";
import {DarkMode, HowToVote, LightMode} from "@mui/icons-material";
import {ValidationStepper} from "./components/ValidationStepper";
import {QrData} from "./helper/qrData";

function App() {
    const [themeMode, setThemeMode] = useState(0);
    const [qrData, setQrData] = useState<QrData>({electionId: "", ballotId: "", contractAddress: ""});

    const switchThemeMode = () => {
        setThemeMode((themeMode + 1) % 2);
    }

    const mode = themeMode === 0 ? "light" : "dark";

    const theme = createTheme({
        palette: {
            mode: mode
        }
    });

    const hasBallot = qrData.electionId !== "";

    return (
        <>
            <ThemeProvider theme={theme}>
                <CssBaseline/>
                <AppBar>
                    <Toolbar>
                        <HowToVote style={{marginRight: "10px"}}/>
                        <Typography variant={"button"} sx={{flexGrow: 1}}>helverify - Verifiable Postal Voting</Typography>
                        <IconButton onClick={switchThemeMode}>
                            {themeMode === 0 ? <DarkMode/> : <LightMode/>}
                        </IconButton>
                    </Toolbar>
                </AppBar>
                <Container maxWidth={"xl"} style={{flexGrow: 1, width: "100%"}}>
                    <Box style={{marginTop: "80px", marginBottom: "80px"}}>
                        <Stack direction="column" spacing={1}>
                            {!hasBallot && (
                                <Card style={{width: "100%", maxWidth: "800px"}}>
                                    <QrReader
                                        onResult={(result, error) => {
                                            let text = result?.getText();
                                            if (!!text) {
                                                let data = JSON.parse(text);

                                                const qrData: QrData = {
                                                    electionId: data.ElectionId,
                                                    ballotId: data.BallotId,
                                                    contractAddress: data.ContractAddress
                                                };
                                                setQrData(qrData);
                                            }
                                        }}
                                        constraints={{facingMode: "environment"}}
                                        containerStyle={{width: "100%"}}
                                    />
                                </Card>
                            )}
                            {hasBallot && (
                                <ValidationStepper
                                    qrData={qrData}
                                />
                            )}
                            {/*<EncryptedBallotsView*/}
                            {/*    ballotId={qrData.ballotId}*/}
                            {/*    ballots={ballots}*/}
                            {/*    electionParameters={electionParameters}*/}
                            {/*/>*/}
                            {/*<Stack*/}
                            {/*    direction="column"*/}
                            {/*    spacing={1}*/}
                            {/*    style={{width: "100%", maxWidth: "1200px"}}*/}
                            {/*>*/}
                            {/*    {castBallot !== undefined && (*/}
                            {/*        <CastBallotView ballot={castBallot}/>*/}
                            {/*    )}*/}
                            {/*    {spoiltBallot !== undefined && electionParameters !== undefined && ballots !== undefined && (*/}
                            {/*        <SpoiltBallotView ballot={spoiltBallot} electionParameters={electionParameters} encryptions={ballots}/>*/}
                            {/*    )}*/}
                            {/*    {electionResults !== undefined && (*/}
                            {/*        <ResultsView electionResults={electionResults}/>*/}
                            {/*    )}*/}
                            {/*    {evidence !== undefined && electionParameters !== undefined  && (*/}
                            {/*        <EvidenceView electionEvidence={evidence} electionParameters={electionParameters}/>*/}
                            {/*    )}*/}
                            {/*</Stack>*/}
                        </Stack>
                    </Box>
                </Container>
            </ThemeProvider>
        </>
    );
}

export default App;

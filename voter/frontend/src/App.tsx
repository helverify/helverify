import React, {useState} from 'react';
import './App.css';
import {
    AppBar, Box,
    Card,
    Container,
    createTheme,
    CssBaseline, IconButton, List, ListItem, ListItemButton, ListItemText,
    Stack,
    ThemeProvider,
    Toolbar,
    Typography
} from '@mui/material';
import {QrReader} from "react-qr-reader";
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
                        <List>
                            <ListItem>
                                <ListItemButton href={"/ballot_instructions.pdf"}>
                                    <ListItemText>Instructions</ListItemText>
                                </ListItemButton>
                            </ListItem>
                        </List>
                        <IconButton onClick={switchThemeMode}>
                            {themeMode === 0 ? <DarkMode/> : <LightMode/>}
                        </IconButton>
                    </Toolbar>
                </AppBar>
                <Container maxWidth={"xl"} style={{flexGrow: 1, width: "100%"}}>
                    <Box style={{marginTop: "120px", marginBottom: "80px"}}>
                        <Stack direction="column" spacing={1}>
                            {!hasBallot && (
                                <Card>
                                    <Box sx={{m:1}}>
                                        <Typography variant={"h4"}>Ballot Verification</Typography>
                                        <Typography>Please scan the QR code on your ballot to start the verification process.</Typography>
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
                                    </Box>
                                </Card>
                            )}
                            {hasBallot && (
                                <ValidationStepper
                                    qrData={qrData}
                                />
                            )}
                        </Stack>
                    </Box>
                </Container>
            </ThemeProvider>
        </>
    );
}

export default App;

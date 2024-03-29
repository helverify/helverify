import React, {useState} from 'react';
import './App.css';
import {Elections} from "./components/election/Elections";
import {
    AppBar,
    BottomNavigation,
    BottomNavigationAction,
    Box, Container,
    createTheme,
    CssBaseline, IconButton,
    Toolbar,
    Typography
} from "@mui/material";
import {DarkMode, HowToVote, LightMode, Menu, QrCodeScanner, Settings} from "@mui/icons-material";
import {Route, Routes} from "react-router-dom";
import {ElectionSetup, setupSteps, settingsSteps} from "./components/setup/ElectionSetup";
import {ThemeProvider} from "@mui/material";
import {useNavigate} from "react-router-dom";
import {BallotScanningView} from "./components/ballot/BallotScanningView";
import {ErrorBoundary} from "react-error-boundary";
import {ErrorHandler} from "./ErrorHandler";

function App() {
    const navigate = useNavigate();

    const [themeMode, setThemeMode] = useState(0);
    const [menuOpen, setMenuOpen] = useState<boolean>(true);
    const [navigationItem, setNavigationItem] = useState<number>(0);

    const mode = themeMode === 0 ? "light" : "dark";

    const theme = createTheme({
        palette: {
            mode: mode
        }
    });

    const switchThemeMode = () => {
        setThemeMode((themeMode + 1) % 2);
    }

    const toggleMenu = () => {
        setMenuOpen(!menuOpen);
    }

    const closeMenu = () => {
        setMenuOpen(false);
    }

    return (
        <>
            <ThemeProvider theme={theme}>
                <CssBaseline/>
                <Box>
                    <AppBar position="fixed" sx={{zIndex: (theme) => theme.zIndex.drawer + 1}}>
                        <Toolbar>
                            <IconButton style={{marginRight: "10px"}} onClick={toggleMenu}>
                                <Menu/>
                            </IconButton>
                            <Typography variant={"button"} sx={{flexGrow: 1}}>helverify - Verifiable Postal
                                Voting</Typography>
                            <IconButton onClick={switchThemeMode}>
                                {themeMode === 0 ? <DarkMode/> : <LightMode/>}
                            </IconButton>
                        </Toolbar>
                    </AppBar>
                </Box>
                <Container maxWidth={"xl"} style={{flexGrow: 1, width: "100%"}}>
                    <Box>
                        <ErrorBoundary FallbackComponent={ErrorHandler}>
                            <div style={{marginTop: "80px", marginBottom: "80px"}}>
                                <Routes>
                                    <Route path="/">
                                        <Route index element={<Elections menuOpen={menuOpen} closeMenu={closeMenu}
                                                                         toggleMenu={toggleMenu}/>}/>
                                        <Route path="elections"
                                               element={<Elections menuOpen={menuOpen} closeMenu={closeMenu}
                                                                   toggleMenu={toggleMenu}/>}/>
                                        <Route path="settings" element={<ElectionSetup steps={settingsSteps}/>}/>
                                        <Route path="elections/create" element={<ElectionSetup steps={setupSteps}/>}/>
                                        <Route path="ballots/scan" element={<BallotScanningView/>}/>
                                        <Route path="setup"/>
                                        <Route path="tally"/>
                                    </Route>
                                </Routes>
                            </div>
                        </ErrorBoundary>
                        <BottomNavigation
                            showLabels
                            style={{position: "fixed", bottom: 0, left: 0, right: 0}}
                            value={navigationItem}
                            onChange={(event, value) => setNavigationItem(value)}
                        >
                            <BottomNavigationAction label="Elections" icon={<HowToVote/>}
                                                    onClick={() => navigate("/elections")}/>
                            <BottomNavigationAction label="Blockchain Settings" icon={<Settings/>}
                                                    onClick={() => navigate("/settings")}/>
                            <BottomNavigationAction label="Tallying" icon={<QrCodeScanner/>}
                                                    onClick={() => navigate("/ballots/scan")}/>
                        </BottomNavigation>
                    </Box>
                </Container>
            </ThemeProvider>
        </>
    );
}

export default App;

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
    Paper, Toolbar,
    Typography
} from "@mui/material";
import {Add, DarkMode, HowToVote, LightMode, Menu, QrCodeScanner} from "@mui/icons-material";
import {Route, Routes} from "react-router-dom";
import {ElectionSetup, setupSteps} from "./components/setup/ElectionSetup";
import {ThemeProvider} from "@mui/material";
import {useNavigate} from "react-router-dom";
import {BallotCreateForm} from "./components/ballot/BallotCreateForm";
import {BallotPrintForm} from "./components/ballot/BallotPrintForm";
import {BallotRegistrationView} from "./components/ballot/BallotRegistrationView";
import {ErrorBoundary} from "react-error-boundary";
import {ErrorHandler} from "./ErrorHandler";

function App() {
    const navigate = useNavigate();

    const [themeMode, setThemeMode] = useState(0);
    const [menuOpen, setMenuOpen] = useState<boolean>(true);

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
                                        <Route path="elections/create" element={<ElectionSetup steps={setupSteps}/>}/>
                                        <Route path="elections/:electionId/ballots/create"
                                               element={<BallotCreateForm/>}/>
                                        <Route path="elections/:electionId/ballots/print" element={<BallotPrintForm/>}/>
                                        <Route path="ballots/register" element={<BallotRegistrationView/>}/>
                                        <Route path="setup"/>
                                        <Route path="tally"/>
                                    </Route>
                                </Routes>
                            </div>
                        </ErrorBoundary>
                        <Paper>
                            <BottomNavigation
                                showLabels
                                style={{position: "fixed", bottom: 0, left: 0, right: 0}}
                            >
                                <BottomNavigationAction label="Elections" icon={<HowToVote/>}
                                                        onClick={() => navigate("/elections")}/>
                                <BottomNavigationAction label="Setup" icon={<Add/>}
                                                        onClick={() => navigate("/elections/create")}/>
                                <BottomNavigationAction label="Ballot Registration" icon={<QrCodeScanner/>}
                                                        onClick={() => navigate("/ballots/register")}/>
                            </BottomNavigation>
                        </Paper>
                    </Box>
                </Container>
            </ThemeProvider>
        </>
    );
}

export default App;

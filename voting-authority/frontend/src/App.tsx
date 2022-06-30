import React from 'react';
import './App.css';
import {Elections} from "./components/election/Elections";
import {BottomNavigation, BottomNavigationAction, Box, createTheme, CssBaseline, Paper} from "@mui/material";
import {Add, HowToVote, PieChart, Link} from "@mui/icons-material";
import {Route, Routes} from "react-router-dom";
import {ElectionSetup, setupSteps} from "./components/setup/ElectionSetup";
import {ThemeProvider} from "@mui/material";
import {useNavigate } from "react-router-dom";
import {BallotCreateForm} from "./components/ballot/BallotCreateForm";
import {BallotPrintForm} from "./components/ballot/BallotPrintForm";

function App() {
    const theme = createTheme({
        palette:{
            mode: 'dark'
        }
    });

    const navigate = useNavigate();

    return (
        <>
            <ThemeProvider theme={theme}>
                <CssBaseline/>
                <Box sx={{margin: "20px"}}>

                        <Routes>
                            <Route path="/">
                                <Route index element={<Elections/>}/>
                                <Route path="elections" element={<Elections/>}/>
                                <Route path="elections/create" element={<ElectionSetup steps={setupSteps}/>}/>
                                <Route path="elections/:electionId/ballots/create" element={<BallotCreateForm />}/>
                                <Route path="elections/:electionId/ballots/print" element={<BallotPrintForm />}/>
                                <Route path="setup"/>
                                <Route path="tally"/>
                            </Route>
                        </Routes>

                    <Paper>
                        <BottomNavigation
                            showLabels
                            style={{position: "fixed", bottom: 0, left: 0, right: 0}}
                        >
                            <BottomNavigationAction label="Elections" icon={<HowToVote/>} onClick={() => navigate("/elections")}/>
                            <BottomNavigationAction label="Setup" icon={<Add/>} onClick={() => navigate("/elections/create")}/>
                            <BottomNavigationAction label="Tallying" icon={<PieChart/>}/>
                            <BottomNavigationAction label="Blockchain" icon={<Link/>} onClick={() => navigate("/blockchain")}/>
                        </BottomNavigation>
                    </Paper>
                </Box>
            </ThemeProvider>
        </>
    );
}

export default App;

import React from 'react';
import './App.css';
import {Elections} from "./components/Elections";
import {BottomNavigation, BottomNavigationAction, Box, createTheme, CssBaseline, Paper} from "@mui/material";
import {Add, HowToVote, PieChart} from "@mui/icons-material";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import {ElectionSetup, setupSteps} from "./components/setup/ElectionSetup";
import {ThemeProvider} from "@mui/material";

function App() {
    const theme = createTheme({
        palette:{
            mode: 'dark'
        }
    });

    return (
        <>

            <ThemeProvider theme={theme}>
                <CssBaseline/>
                <Box sx={{margin: "20px"}}>
                    <BrowserRouter>
                        <Routes>
                            <Route path="elections" element={<Elections/>}/>
                            <Route path="elections/create" element={<ElectionSetup steps={setupSteps}/>}/>
                            <Route path="setup"/>
                            <Route path="tally"/>
                        </Routes>
                    </BrowserRouter>
                    <Paper>
                        <BottomNavigation
                            showLabels
                            style={{position: "fixed", bottom: 0, left: 0, right: 0}}
                        >
                            <BottomNavigationAction label="Elections" icon={<HowToVote/>}/>
                            <BottomNavigationAction label="Setup" icon={<Add/>}/>
                            <BottomNavigationAction label="Tallying" icon={<PieChart/>}/>
                        </BottomNavigation>
                    </Paper>
                </Box>
            </ThemeProvider>
        </>
    );
}

export default App;

import React from 'react';
import './App.css';
import {Box, createTheme, CssBaseline, ThemeProvider} from '@mui/material';
import Web3 from "web3";

function App() {
  const theme = createTheme({
    palette:{
      mode: 'dark'
    }
  });

  const web3 = new Web3("http://localhost:8545");

  web3.eth.getAccounts().then(console.log);

  return (
    <>
      <ThemeProvider theme={theme}>
        <CssBaseline/>
        <Box>

        </Box>
      </ThemeProvider>
    </>
  );
}

export default App;

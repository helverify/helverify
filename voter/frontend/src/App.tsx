import React, {useEffect, useState} from 'react';
import './App.css';
import {Card, Container, createTheme, CssBaseline, Stack, ThemeProvider} from '@mui/material';
import Web3 from "web3";
import {ElectionABI} from "./contract/electionContract";
import {QrReader} from "react-qr-reader";
import {EncryptedBallotVerification} from "./components/EncryptedBallotVerification";
import {BallotService} from "./services/ballotService";
import {EncryptedBallot} from "./cryptography/encryptedBallot";
import {ElectionParameters} from "./election/election";
import {BigNumberHelper} from "./helper/bigNumberHelper";
import bigInt from "big-integer";
import {EncryptedBallotsView} from "./components/EncryptedBallotsView";

type QrData = {
    electionId: string,
    ballotId: string,
    contractAddress: string
}

function App() {
    const theme = createTheme({
        palette: {
            mode: 'dark'
        }
    });

    const [qrData, setQrData] = useState<QrData>({electionId: "", ballotId: "", contractAddress: ""});
    const [ballots, setBallots] = useState<EncryptedBallot[]>();
    const [electionParameters, setElectionParameters] = useState<ElectionParameters>();

    const web3 = new Web3("ws://localhost:8546");

    const loadContractData = () => {
        const electionContract = new web3.eth.Contract(ElectionABI, qrData.contractAddress);

        electionContract.methods.castBallots(qrData.ballotId).call().then(async (res: any) => {
            console.log(res.ballotId);
        });

        const ballotService: BallotService = new BallotService(qrData.contractAddress);

        ballotService.getEncryptedBallots(qrData.ballotId).then((encryptedBallots) => {
            setBallots(encryptedBallots);
        });

        electionContract.methods.getElectionParameters().call().then((result: any) => {
            let cnPublicKeys: bigInt.BigInteger[] = [];

            result.consensusNodePublicKeys.forEach((key: string) => {
                cnPublicKeys.push(bigInt(key, 16));
            });

            let electionParams: ElectionParameters = {
                p: BigNumberHelper.fromHexString(result.elGamalParameters.p),
                g: BigNumberHelper.fromHexString(result.elGamalParameters.g),
                publicKey: BigNumberHelper.fromHexString(result.publicKey),
                consensusNodePublicKeys: cnPublicKeys
            };

            setElectionParameters(electionParams);
        });

        //electionContract.methods.spoiltBallots(qrData.ballotId).call().then(console.log);
    }

    useEffect(() => {
        if (qrData.electionId !== "") {
            loadContractData();
        }
    }, [qrData])

    const hasBallot = qrData.electionId !== "";

    return (
        <>
            <ThemeProvider theme={theme}>
                <CssBaseline/>
                <Container>
                    <Stack direction="row" spacing={1}>
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
                        <EncryptedBallotsView ballotId={qrData.ballotId} ballots={ballots}
                                              electionParameters={electionParameters}/>
                    </Stack>
                </Container>

            </ThemeProvider>
        </>
    );
}

export default App;

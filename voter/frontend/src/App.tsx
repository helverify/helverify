import React, {useEffect, useState} from 'react';
import './App.css';
import {
    AppBar,
    Card,
    Container,
    createTheme,
    CssBaseline,
    Stack,
    ThemeProvider,
    Toolbar,
    Typography
} from '@mui/material';
import Web3 from "web3";
import {ElectionABI} from "./contract/electionContract";
import {QrReader} from "react-qr-reader";
import {BallotService} from "./services/ballotService";
import {EncryptedBallot} from "./ballot/encryptedBallot";
import {ElectionParameters, ElectionResults} from "./election/election";
import {BigNumberHelper} from "./helper/bigNumberHelper";
import bigInt from "big-integer";
import {EncryptedBallotsView} from "./components/EncryptedBallotsView";
import {SpoiltBallotView} from "./components/SpoiltBallotView";
import {SpoiltBallot} from "./ballot/spoiltBallot";
import {CastBallot} from "./ballot/castBallot";
import {CastBallotView} from "./components/CastBallotView";
import {HowToVote} from "@mui/icons-material";
import {ElectionService} from "./services/electionService";
import {ResultsView} from "./components/ResultsView";
import {ResultEvidence} from "./election/resultEvidence";
import {EvidenceView} from "./components/EvidenceView";

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
    const [spoiltBallot, setSpoiltBallot] = useState<SpoiltBallot>();
    const [castBallot, setCastBallot] = useState<CastBallot>();
    const [electionParameters, setElectionParameters] = useState<ElectionParameters>();
    const [electionResults, setElectionResults] = useState<ElectionResults>();
    const [evidence, setEvidence] = useState<ResultEvidence>();

    const web3 = new Web3("ws://localhost:8546");

    const loadContractData = () => {
        const electionContract = new web3.eth.Contract(ElectionABI, qrData.contractAddress);

        const ballotService: BallotService = new BallotService(qrData.contractAddress);

        const electionService: ElectionService = new ElectionService(qrData.contractAddress);

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

        ballotService.getCastBallot(qrData.ballotId).then((ballot: CastBallot) => {
            setCastBallot(ballot);
        })

        ballotService.getSpoiltBallot(qrData.ballotId).then((ballot: SpoiltBallot) => {
            setSpoiltBallot(ballot);
        });

        electionService.getFinalResults().then((results: ElectionResults) => {
            setElectionResults(results);
        });

        electionService.getFinalResultEvidence().then((evidence: ResultEvidence) => {
           setEvidence(evidence);
        });
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
                <AppBar>
                    <Toolbar>
                        <HowToVote style={{marginRight: "10px"}}/>
                        <Typography variant={"button"}>helverify - Verifiable Postal Voting</Typography>
                    </Toolbar>
                </AppBar>
                <Container style={{marginTop: "100px"}}>
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
                        <EncryptedBallotsView ballotId={qrData.ballotId} ballots={ballots}
                                              electionParameters={electionParameters}/>
                        <Stack direction="row" spacing={1}>
                            {castBallot !== undefined && (
                                <CastBallotView ballot={castBallot}/>
                            )}
                            {spoiltBallot !== undefined && (
                                <SpoiltBallotView ballot={spoiltBallot}/>
                            )}
                            {electionResults !== undefined && (
                                <ResultsView electionResults={electionResults}/>
                            )}
                            {evidence !== undefined && electionParameters !== undefined  && (
                                <EvidenceView electionEvidence={evidence} electionParameters={electionParameters}/>
                            )}
                        </Stack>
                    </Stack>
                </Container>
            </ThemeProvider>
        </>
    );
}

export default App;

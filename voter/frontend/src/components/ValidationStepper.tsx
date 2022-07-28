import {
    Box, Button,
    Container, Divider,
    Paper, Stack,
    Step,
    StepLabel,
    Stepper,
    Typography
} from "@mui/material";
import React, {useEffect, useState} from "react";
import {ValidationStep} from "./validation/validationStep";
import {AutomaticValidationStep} from "./validation/AutomaticValidationStep";
import {ManualValidationStep} from "./validation/ManualValidationStep";
import {ResultsStep} from "./validation/ResultsStep";
import {EncryptedBallot} from "../ballot/encryptedBallot";
import {ElectionParameters, ElectionResults} from "../election/election";
import {QrData} from "../helper/qrData";
import {BallotService} from "../services/ballotService";
import {ElectionABI} from "../contract/electionContract";
import {ElectionService} from "../services/electionService";
import bigInt from "big-integer";
import {BigNumberHelper} from "../helper/bigNumberHelper";
import {CastBallot} from "../ballot/castBallot";
import {SpoiltBallot} from "../ballot/spoiltBallot";
import {ResultEvidence} from "../election/resultEvidence";
import Web3 from "web3";
import {SummaryStep} from "./validation/SummaryStep";

export type ValidationStepperProps = {
    qrData: QrData;
}

export const ValidationStepper = (props: ValidationStepperProps) => {
    const [step, setStep] = useState(0);
    const [ballots, setBallots] = useState<EncryptedBallot[]>();
    const [spoiltBallot, setSpoiltBallot] = useState<SpoiltBallot>();
    const [castBallot, setCastBallot] = useState<CastBallot>();
    const [electionResults, setElectionResults] = useState<ElectionResults>();
    const [evidence, setEvidence] = useState<ResultEvidence>();
    const [electionParameters, setElectionParameters] = useState<ElectionParameters>();

    const qrData = props.qrData;

    useEffect(() => {
        const web3 = new Web3(process.env.REACT_APP_GETH_WS ?? "");

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

        if (qrData.electionId !== "") {
            loadContractData();
        }
    }, [qrData])

    const goToNextStep = () => {
        if (step < steps.length) {
            setStep((step) => step + 1);
        }
    };

    const goToPreviousStep = () => {
        if (step > 0) {
            setStep((step) => step - 1);
        }
    };

    const stepComponent = () => {
        let comp;
        if (step < steps.length) {
            comp = React.cloneElement(steps[step].component);
        } else {
            comp = <></>;
        }
        return comp;
    }

    const steps: ValidationStep[] = [
        {
            caption: "Before Casting",
            component: <AutomaticValidationStep
                ballots={ballots}
                ballotId={qrData.ballotId}
                electionParameters={electionParameters}
            />
        },
        {
            caption: "After Tallying",
            component: <ManualValidationStep
                ballots={ballots}
                ballotId={qrData.ballotId}
                electionParameters={electionParameters}
                castBallot={castBallot}
                spoiltBallot={spoiltBallot}
            />
        },
        {
            caption: "Results",
            component: <ResultsStep
                ballots={ballots}
                ballotId={qrData.ballotId}
                electionParameters={electionParameters}
                electionResults={electionResults}
                evidence={evidence}
            />
        },
        {
            caption: "Summary",
            component: <SummaryStep
                ballots={ballots}
                electionParameters={electionParameters}
                ballotId={qrData.ballotId}
            />
        }
    ];

    return (
        <>
            {electionParameters !== undefined && (
                <Container maxWidth="lg">
                    <Paper variant="outlined" style={{minWidth: "450px"}}>
                        <Stack direction="column" spacing={1} sx={{p: 2}}>
                            <Typography variant="h4" align="center" sx={{m: 2}}>Verification</Typography>
                            <Stepper activeStep={step} style={{marginTop: "30px", marginBottom: "10px"}} sx={{m: 1}}>
                                {steps.map((s) => {
                                    return (
                                        <Step key={s.caption}>
                                            <StepLabel>{s.caption}</StepLabel>
                                        </Step>
                                    );
                                })}
                            </Stepper>
                            <Box sx={{m: 2}}>
                                {stepComponent()}
                            </Box>
                            <Divider/>
                            <Stack direction="row" spacing={1} style={{marginTop: "10px"}}>
                                <Box display="flex" justifyContent="left" alignItems="left" flexGrow={1}>
                                    {step > 0 && (
                                        <Button onClick={goToPreviousStep} variant="outlined">Previous</Button>
                                    )}
                                </Box>
                                <Box display="flex" justifyContent="right" alignItems="right">
                                    {step < steps.length - 1 && (
                                        <Button onClick={goToNextStep} variant="contained">Next</Button>
                                    )}
                                </Box>
                            </Stack>
                        </Stack>
                    </Paper>
                </Container>
            )}
        </>
    );
}


import {Box, Container, Paper, Step, StepLabel, Stepper, Typography} from "@mui/material";
import React, {useEffect, useState} from "react";
import {ElectionSetupStep} from "./electionSetupStep";
import {ElectionForm} from "./ElectionForm";
import {BlockchainDto, ElectionDto} from "../../api/Api";
import {BlockchainForm} from "./BlockchainForm";
import {PublicKeyForm} from "./PublicKeyForm";
import {useNavigate} from "react-router-dom";
import {ContractDeploymentForm} from "./ContractDeploymentForm";
import {ElectionInfo} from "../election/ElectionInfo";

export function ElectionSetup(props: { steps: ElectionSetupStep[] }) {
    const navigate = useNavigate();

    const steps = props.steps;

    const [step, setStep] = useState(0);
    const [election, setElection] = useState<ElectionDto>({});
    const [blockchain, setBlockchain] = useState<BlockchainDto>({});

    useEffect(() => {
        if (step === steps.length) {
            navigate("/elections");
        }
    }, [step, steps, navigate])

    const goToNextStep = (election: ElectionDto, blockchain: BlockchainDto) => {
        if (step < steps.length) {
            setElection(election);
            setBlockchain(blockchain);
            setStep(step + 1)
        }
    };

    const stepComponent = () => {
        let comp;
        if (step < steps.length) {
            comp = React.cloneElement(steps[step].component, {
                next: goToNextStep,
                election: election,
                blockchain: blockchain
            });
        } else {
            comp = <></>;
        }
        return comp;
    }

    return (
        <>
            <Container maxWidth="md">
                <Paper variant="outlined" style={{minWidth: "450px"}}>
                    <Typography variant="h4" align="center" sx={{m: 2}}>Election Setup</Typography>
                    <Stepper activeStep={step} sx={{m: 1}} style={{marginTop: "30px", marginBottom: "10px"}}>
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
                </Paper>
                {election.id !== undefined && (
                    <Paper variant="outlined" style={{minWidth: "450px", marginTop: "18px"}}>
                        <Box>
                            <ElectionInfo election={election}/>
                        </Box>
                    </Paper>
                )}
            </Container>

        </>
    );
}

export const setupSteps: ElectionSetupStep[] = [
    new ElectionSetupStep(
        "Consensus Nodes",
        <BlockchainForm next={() => {
        }} election={{}} blockchain={{}}/>
    ),
    new ElectionSetupStep(
        "Election",
        <ElectionForm next={() => {
        }} election={{}} blockchain={{}}/>
    ),
    new ElectionSetupStep(
        "Public Key",
        <PublicKeyForm next={() => {
        }} election={{}} blockchain={{}}/>
    ),
    new ElectionSetupStep(
        "Contract Deployment",
        <ContractDeploymentForm next={() => {
        }} election={{}} blockchain={{}}/>
    )
];
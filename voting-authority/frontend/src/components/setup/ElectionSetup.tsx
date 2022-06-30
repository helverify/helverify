import {Box, Grid, Step, StepLabel, Stepper} from "@mui/material";
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
    })

    const goToNextStep = (election: ElectionDto, blockchain: BlockchainDto) => {
        if (step < steps.length) {
            setStep(step + 1)
            setElection(election);
            setBlockchain(blockchain);
        }
    };

    const stepComponent = () => {
        let comp;
        if (step < steps.length) {
            comp = React.cloneElement(steps[step].component, {next: goToNextStep, election: election, blockchain: blockchain});
        } else {
            comp = <></>;
        }
        return comp;
    }

    const electionInfo = election.id !== "" ? <ElectionInfo election={election}/> : <></>;

    return (
        <>
            <Box>
                <Stepper activeStep={step}>
                    {steps.map((s) => {
                        return (
                            <Step key={s.caption}>
                                <StepLabel>{s.caption}</StepLabel>
                            </Step>
                        );
                    })}
                </Stepper>
                <Grid container>
                    <Grid item xs={9}>
                        <Box sx={{m: 2}}>
                            {stepComponent()}
                        </Box>
                    </Grid>
                    <Grid item xs={3}>
                        {electionInfo}
                    </Grid>
                </Grid>
            </Box>
        </>
    );
}

export const setupSteps: ElectionSetupStep[] = [
    new ElectionSetupStep("Consensus Nodes", <BlockchainForm next={() => {
    }} election={{}} blockchain={{}}/>),
    new ElectionSetupStep("Election", <ElectionForm next={() => {
    }} election={{}} blockchain={{}}/>),
    new ElectionSetupStep("Public Key", <PublicKeyForm next={() => {
    }} election={{}} blockchain={{}}/>),
    new ElectionSetupStep("Contract Deployment", <ContractDeploymentForm next={() => {
    }} election={{}} blockchain={{}}/>)
];
import {Box, Button, Grid, Step, StepLabel, Stepper} from "@mui/material";
import React, {useEffect, useState} from "react";
import {ElectionSetupStep} from "./electionSetupStep";
import {ElectionForm} from "./ElectionForm";
import {Api, ElectionDto} from "../../Api";
import {ConsensusNodeRegistrationForm} from "./ConsensusNodeRegistrationForm";
import {PublicKeyForm} from "./PublicKeyForm";
import {BlockchainSetupForm} from "./BlockchainSetupForm";
import {useNavigate} from "react-router-dom";
import {ElectionInfo} from "./ElectionInfo";

export function ElectionSetup(props: { steps: ElectionSetupStep[] }) {

    const navigate = useNavigate();

    const steps = props.steps;

    const [step, setStep] = useState(0);
    const [election, setElection] = useState<ElectionDto>({});

    useEffect(() => {
        if (step === steps.length) {
            navigate("/elections");
        }
    })

    const goToNextStep = (election: ElectionDto) => {
        if (step < steps.length) {
            setStep(step + 1)
            setElection(election);
        }
    };

    const stepComponent = () => {
        loadElection();
        let comp;
        if (step < steps.length) {
            comp = React.cloneElement(steps[step].component, {next: goToNextStep, election: election});
        } else {
            comp = <></>;
        }
        return comp;
    }

    const loadElection = () => {
        if(election.id === undefined || election.id === null || election.id === ""){
            return;
        }

        const client = new Api({
            baseUrl: "http://localhost:5000"
        });

        client.api.electionsDetail(election.id).then((result)=>{
            setElection(result.data);
        });
    }

    const electionInfo = election.id !== "" ? <ElectionInfo election={election}/> : <></>;

    return (
        <>
            <Box>
                <Stepper activeStep={step}>
                    {steps.map((s, index) => {
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
    new ElectionSetupStep("Election", <ElectionForm next={() => {
    }} election={{}}/>),
    new ElectionSetupStep("Consensus Nodes", <ConsensusNodeRegistrationForm next={() => {
    }} election={{}}/>),
    new ElectionSetupStep("Public Key", <PublicKeyForm next={() => {
    }} election={{}}/>),
    new ElectionSetupStep("Blockchain", <BlockchainSetupForm next={() => {
    }} election={{}}/>)
];
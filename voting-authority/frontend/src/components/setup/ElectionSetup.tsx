import {Box, Button, Step, StepLabel, Stepper} from "@mui/material";
import React, {useState} from "react";
import {ElectionSetupStep} from "./electionSetupStep";
import {Ballot, CurrencyBitcoin, HowToReg, Key} from "@mui/icons-material";
import { ElectionForm } from "./ElectionForm";

export function ElectionSetup(props: {steps: ElectionSetupStep[]}) {

    const steps = props.steps;

    const [step, setStep] = useState(0);

    const goToNextStep = () => {
        if (step < steps.length) {
            setStep(step + 1)
        }
    };

    const goToPreviousStep = () => {
        if (step > 0) {
            setStep(step - 1)
        }
    }

    const stepComponent = () => {
        let comp;
        if(step < steps.length){
           comp = React.cloneElement(steps[step].component, {next: goToNextStep});
        } else {
            comp = <></>;
        }
        return comp;
    }

    return (
        <>
            <>
                <Button onClick={goToPreviousStep}>Previous</Button>
                <Button onClick={goToNextStep}>Next</Button>
            </>
            <Box>
                {stepComponent()}
            </Box>


            <Stepper activeStep={step}>
                {steps.map((s, index) => {
                    return (
                        <Step key={s.caption}>
                            <StepLabel>{s.caption}</StepLabel>
                        </Step>
                    );
                })}
            </Stepper>
        </>
    );
}

export const setupSteps: ElectionSetupStep[] = [
    new ElectionSetupStep("Election", <ElectionForm next={() => {}}/>),
    new ElectionSetupStep("Consensus Nodes", <HowToReg />),
    new ElectionSetupStep("Public Key", <Key/>),
    new ElectionSetupStep("Blockchain", <CurrencyBitcoin/>)
];
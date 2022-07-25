import React, {useState} from "react";
import {Box, Button, FormControl, Stack, TextField} from "@mui/material";
import {ProgressWithLabel} from "../progress/ProgressWithLabel";

export type BallotFormProps = {
    buttonCaption: string,
    loadingLabel: string,
    buttonAction: (numberOfBallots: number) => void,
    isLoading: boolean
}

export const BallotForm = (props: BallotFormProps) => {

    const styleVariant = "standard";

    const [numberOfBallots, setNumberOfBallots] = useState(0);

    const handleChange = (evnt: any) => {
        const value = evnt.target.value;
        setNumberOfBallots(value);
    };

    const submit = () => {
        props.buttonAction(numberOfBallots);
    }

    return (
        <>
            <Stack direction="column" spacing={1} sx={{m: 2}}>
                <FormControl>
                    <TextField id="numberOfBallots"
                               label="Number of Ballots"
                               variant={styleVariant}
                               value={numberOfBallots}
                               onChange={handleChange}/>
                </FormControl>
                <Box display="flex" alignItems="right" justifyContent="right">
                    <Button variant="contained" onClick={submit}>{props.buttonCaption}</Button>
                </Box>
            </Stack>
            <ProgressWithLabel isLoading={props.isLoading} label={props.loadingLabel}/>
        </>
    );
};
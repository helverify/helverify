import {BallotFormProps} from "./ballotForm";
import {useState} from "react";
import {Backdrop, Box, Button, CircularProgress, FormControl, Stack, TextField} from "@mui/material";

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
            <Backdrop open={props.isLoading}>
                <CircularProgress/>
            </Backdrop>
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
        </>
    );
};
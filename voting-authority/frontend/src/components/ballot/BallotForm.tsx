import {BallotFormProps} from "./ballotForm";
import {useState} from "react";
import {Backdrop, Button, Card, CircularProgress, FormControl, Grid, Stack, TextField} from "@mui/material";

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
            <Grid container spacing={1}>

                <Card>
                    <Stack direction="column" spacing={1} sx={{m: 2}}>
                        <FormControl>
                            <TextField id="numberOfBallots"
                                       label="Number of Ballots"
                                       variant={styleVariant}
                                       value={numberOfBallots}
                                       onChange={handleChange}/>
                        </FormControl>
                        <FormControl>
                            <Button variant="contained" onClick={submit}>{props.buttonCaption}</Button>
                        </FormControl>
                    </Stack>
                </Card>
            </Grid>
        </>
    );
};
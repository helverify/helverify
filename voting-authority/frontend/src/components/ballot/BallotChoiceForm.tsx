import {
    Alert,
    Box,
    Button,
    Checkbox,
    FormControl,
    FormControlLabel,
    FormLabel,
    Radio,
    RadioGroup, Snackbar,
    Stack,
    Typography
} from "@mui/material";
import {EvidenceDto, PrintBallotDto, PrintOptionDto} from "../../api/Api";
import {useState} from "react";
import {apiClient} from "../../api/apiClient";
import {ProgressWithLabel} from "../progress/ProgressWithLabel";

type BallotChoiceProps = {
    ballot: PrintBallotDto,
    electionId: string,
    onSubmit: () => void
};

export const BallotChoiceForm = (props: BallotChoiceProps) => {
    const optionPrefix: string = "option-";
    const ballot: PrintBallotDto = props.ballot;

    const [warning, setWarning] = useState<string>("");
    const [error, setError] = useState<string>("");

    const [choices, setChoices] = useState<number[]>([]);
    const [column, setColumn] = useState<number>(-1);
    const [isLoading, setLoading] = useState<boolean>(false);

    const handleColumnChange = (evnt: any) => {
        let selectedColumn: number = parseInt(evnt.target.value);

        setColumn(selectedColumn);
    }

    const handleOptionChange = (evnt: any) => {
        const {id, checked} = evnt.target;

        if (props.ballot === undefined || props.ballot === null || props.ballot.options === undefined || props.ballot.options === null) {
            return;
        }

        const index: number = parseInt(id.replace(optionPrefix, ""));

        let choicesClone;

        if (checked) {
            choicesClone = [...choices, index];
        } else {
            const optionIndex = choices.indexOf(index, 0);
            choicesClone = [...choices];
            choicesClone.splice(optionIndex, 1);
        }

        setChoices(choicesClone);
    }

    const handleSubmit = () => {
        if (!(column === 0 || column === 1)) {
            setWarning("Column not selected");
            return;
        }

        if (props.ballot === undefined || props.ballot === null) {
            setWarning("Ballot is not defined");
            return;
        }

        if(choices.length === 0){
            setWarning("Please select the marked choices")
            return;
        }

        const ballot = props.ballot;

        if (ballot.options === undefined || ballot.options === null) {
            setWarning("Ballot options are not defined");
            return;
        }

        const options = ballot.options;

        // get correct short codes for column selection
        let selection: string[] = [];
        choices.forEach(choice => {
            if (!options[choice]) {
                setWarning("Undefined option");
                return;
            }

            let shortCode;

            if (column === 0) {
                shortCode = options[choice].shortCode1
            } else {
                shortCode = options[choice].shortCode2
            }

            if (shortCode === undefined || shortCode === null) {
                setWarning("Undefined short code");
                return;
            }

            selection.push(shortCode);
        });

        let evidenceDto: EvidenceDto = {
            selectedOptions: selection,
            spoiltBallotIndex: 1 - column
        }

        if (ballot.ballotId === undefined || ballot.ballotId === null) {
            setWarning("BallotId not set");
            return;
        }

        setLoading(true);

        apiClient().api.electionsBallotsEvidenceCreate(props.electionId, ballot.ballotId, evidenceDto).then(() => {
            setColumn(-1);
            setChoices([]);
            setLoading(false);
            props.onSubmit();
        }, error => setError(error));
    };

    return (
        <>
            <Box sx={{m: 2}}>
                <Stack direction={"column"} spacing={1} sx={{m: 2}}>
                    <div>
                        <Typography variant="h4">Ballot</Typography>
                    </div>
                    <div>
                        <Typography style={{wordBreak: "break-all"}}>{ballot.ballotId}</Typography>
                    </div>
                    <div>
                        <FormControl>
                            <FormLabel>Column selection</FormLabel>
                            <RadioGroup row value={column} onChange={handleColumnChange}>
                                <FormControlLabel control={<Radio/>} label={"1"} value={0}/>
                                <FormControlLabel control={<Radio/>} label={"2"} value={1}/>
                            </RadioGroup>
                        </FormControl>

                    </div>
                    <div>
                        <FormControl>
                            <FormLabel>Candidates / Options</FormLabel>
                            {ballot.options && ballot.options.map((opt: PrintOptionDto, index: number) => {
                                return (
                                    <Stack key={index} direction={"row"}>
                                        <Checkbox id={`${optionPrefix}${index}`}
                                                  onChange={handleOptionChange}></Checkbox>
                                        <p>{opt.name}</p>
                                    </Stack>

                                );
                            })}
                        </FormControl>
                    </div>
                    <Box display="flex" alignItems="right" justifyContent="right">
                        <Button variant="contained" onClick={handleSubmit}>Save Ballot Choices</Button>
                    </Box>
                </Stack>
            </Box>
            <ProgressWithLabel isLoading={isLoading} label={"Saving ballot choices"}/>
            <Snackbar
                open={warning !== ""}
                onClose={() => setWarning("")}
                autoHideDuration={3000}
            >
                <Alert severity="warning">{warning}</Alert>
            </Snackbar>
            <Snackbar
                open={error !== ""}
                onClose={() => setError("")}
                autoHideDuration={3000}
            >
                <Alert severity="error">{error}</Alert>
            </Snackbar>
        </>
    );
}
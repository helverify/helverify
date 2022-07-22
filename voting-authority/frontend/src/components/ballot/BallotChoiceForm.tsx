import {
    Backdrop, Box,
    Button,
    Card,
    CardContent,
    Checkbox,
    CircularProgress,
    FormControl,
    FormControlLabel,
    FormLabel,
    Radio,
    RadioGroup,
    Stack,
    Typography
} from "@mui/material";
import {EvidenceDto, PrintBallotDto, PrintOptionDto} from "../../api/Api";
import {useState} from "react";
import {apiClient} from "../../api/apiClient";

type BallotChoiceProps = {
    ballot: PrintBallotDto,
    electionId: string,
    onSubmit: () => void
};

export const BallotChoiceForm = (props: BallotChoiceProps) => {

    const optionPrefix: string = "option-";
    const ballot: PrintBallotDto = props.ballot;

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
            throw new Error("Column not selected");
        }

        if (props.ballot === undefined || props.ballot === null) {
            throw new Error("Ballot is not defined");
        }

        const ballot = props.ballot;

        if (ballot.options === undefined || ballot.options === null) {
            throw new Error("Ballot options are not defined");
        }

        const options = ballot.options;

        // get correct short codes for column selection
        let selection: string[] = [];
        choices.forEach(choice => {
            if (!options[choice]) {
                throw new Error("Undefined option");
            }

            let shortCode;

            if (column === 0) {
                shortCode = options[choice].shortCode1
            } else {
                shortCode = options[choice].shortCode2
            }

            if (shortCode === undefined || shortCode === null) {
                throw new Error("Undefined short code");
            }

            selection.push(shortCode);
        });

        let evidenceDto: EvidenceDto = {
            selectedOptions: selection,
            spoiltBallotIndex: 1 - column
        }

        if (ballot.ballotId === undefined || ballot.ballotId === null) {
            throw new Error("BallotId not set");
        }

        setLoading(true);
        apiClient().api.electionsBallotsEvidenceCreate(props.electionId, ballot.ballotId, evidenceDto).then(() => {
            setColumn(-1);
            setChoices([]);
            setLoading(false);
            props.onSubmit();
        });
    };

    return (
        <>
            <Backdrop open={isLoading}>
                <CircularProgress/>
            </Backdrop>
            <Box sx={{m: 2}}>
                <Stack direction={"column"} spacing={1} sx={{m: 1}}>
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
                    <div>
                        <Typography style={{wordBreak: "break-all"}}>{}</Typography>
                    </div>
                    <Button variant={"outlined"} onClick={handleSubmit}>Store Ballot Choices</Button>
                </Stack>

            </Box>

        </>

    );
}
import {
    Box,
    Button,
    FormControl, FormHelperText,
    IconButton, InputLabel,
    List,
    ListItem,
    MenuItem, Select, SelectChangeEvent,
    Stack, TextField, Typography
} from "@mui/material";
import {ElectionDto, ElectionOption, ElectionOptionDto} from "../../api/Api";
import {useState} from "react";
import {Add, DeleteForever} from "@mui/icons-material";
import {dhGroups, DiffieHellmanGroup, SetupStepProps} from "./electionSetupStep";
import {apiClient} from "../../api/apiClient";
import {CandidateInfo} from "../election/CandidateInfo";
import {ProgressWithLabel} from "../progress/ProgressWithLabel";

export const ElectionForm = (props: SetupStepProps) => {

    const styleVariant = "standard";
    const stylingParams = {minWidth: 200};
    const typographyStyle = {marginTop: "25px", marginBottom: "15px"};

    const [election, setElection] = useState<ElectionDto>({
        name: "",
        question: "",
        p: "",
        g: "",
        options: [],
        blockchainId: props.blockchain.id
    });

    const [isLoading, setLoading] = useState<boolean>(false);
    const [currentOption, setCurrentOption] = useState<ElectionOptionDto>({name: ""});
    const [diffieHellmanGroup, setDiffieHellmanGroup] = useState<DiffieHellmanGroup>({name: "", p: "", g: ""});


    const addOption = (option: ElectionOption | undefined) => {
        if (option === undefined || option.name === "") {
            return;
        }

        let newElection = {...election};

        if (election.options === undefined || election.options === null) {
            newElection.options = [option];
        } else {
            newElection.options = [...election.options, option];
        }


        setElection((oldElection) => ({...oldElection, ...newElection}));
        setCurrentOption({name: ""});
    }

    const removeOption = (index: number) => {
        let optionsClone: ElectionOptionDto[];
        if (election.options === undefined || election.options === null) {
            optionsClone = [];
        } else {
            optionsClone = [...election.options];
        }

        optionsClone.splice(index, 1); // https://stackoverflow.com/questions/5767325/how-can-i-remove-a-specific-item-from-an-array

        let newElection = {...election};

        newElection.options = optionsClone;

        setElection((oldElection) => ({...oldElection, ...newElection}));
    }

    const handleChange = (evnt: any) => {
        const {name, value} = evnt.target;

        let newElection: ElectionDto = {};

        newElection[name as keyof ElectionDto] = value; // https://www.nadershamma.dev/blog/2019/how-to-access-object-properties-dynamically-using-bracket-notation-in-typescript/

        newElection.blockchainId = props.blockchain.id;

        setElection((oldElection) => ({...oldElection, ...newElection})); // https://blog.logrocket.com/using-react-usestate-object/
    };

    const handleCurrentOptionChange = (evnt: any) => {
        const value = evnt.target.value;

        let newOption: ElectionOption = {name: value};

        setCurrentOption((oldOption) => ({...oldOption, ...newOption}));
    }

    const setDhGroup = (evnt: SelectChangeEvent) => {
        let groupName: string = evnt.target.value;

        let newElection: ElectionDto = {...election};

        let dhGroup: DiffieHellmanGroup = dhGroups.find(group => group.name === groupName) ?? {name: "", p: "", g: ""};

        if (dhGroup.name === "") {
            return;
        }

        setDiffieHellmanGroup(dhGroup);

        newElection.p = dhGroup.p;
        newElection.g = dhGroup.g;

        setElection((oldElection) => ({...oldElection, ...newElection}));
    }

    const saveElection = () => {
        setLoading(true);
        apiClient().api.electionsCreate(election).then((result) => {
            setLoading(false);
            props.next(result.data, props.blockchain);
        });
    }

    const onKeyDown = (evnt: any) => {
        if (evnt.keyCode === 13) { // https://stackoverflow.com/questions/43384039/how-to-get-the-textfield-value-when-enter-key-is-pressed-in-react
            addOption(currentOption);
        }
    };

    let electionOptions = election.options ?? [];

    return (
        <>
            <Stack direction="column" spacing={1}>
                <Box>
                    <Stack direction="column" spacing={1}>
                        <Typography variant={"h5"} style={typographyStyle}>Election Parameters</Typography>
                        <FormControl variant={styleVariant} sx={stylingParams}>
                            <TextField fullWidth id="election-name" name="name" label="Name" variant={styleVariant}
                                       value={election?.name} onChange={handleChange}/>
                        </FormControl>
                        <FormControl variant={styleVariant} sx={stylingParams}>
                            <TextField fullWidth id="election-question" name="question" label="Question"
                                       variant={styleVariant} value={election?.question} onChange={handleChange}/>
                        </FormControl>
                        <FormControl variant={styleVariant} sx={stylingParams}>
                            <Stack direction="column">
                                <TextField fullWidth id="election-option" label="Add option / candidate"
                                           variant={styleVariant} value={currentOption?.name}
                                           onChange={handleCurrentOptionChange}
                                           onKeyDown={onKeyDown}
                                           style={{marginBottom: "20px"}}
                                />
                                <Button id="election-option-add"
                                        variant="outlined"
                                        onClick={() => addOption(currentOption)}
                                        disabled={currentOption.name === ""}
                                        style={{marginBottom: "15px"}}
                                ><Add/></Button>
                            </Stack>
                        </FormControl>
                        <FormControl variant={styleVariant} sx={stylingParams}>
                            <InputLabel htmlFor="election-dh-group">Diffie-Hellman Group</InputLabel>
                            <Select
                                id="election-dh-group"
                                onChange={setDhGroup}
                                value={diffieHellmanGroup.name}
                            >
                                <FormHelperText>Diffie-Hellman Group</FormHelperText>
                                {dhGroups.map((dhGroup, index) => {
                                    return (
                                        <MenuItem key={index} value={dhGroup.name}>{dhGroup.name}</MenuItem>
                                    );
                                })}
                            </Select>
                        </FormControl>
                        <FormControl>
                            <TextField id="election-p" label="ElGamal Prime p" variant="standard"
                                       value={election.p ?? ""} disabled/>
                        </FormControl>
                        <FormControl>
                            <TextField id="election-q" label="ElGamal Generator g" variant="standard"
                                       value={election.g ?? ""} disabled/>
                        </FormControl>
                    </Stack>
                </Box>
                <Box>
                    <Typography variant={"h5"} style={typographyStyle}>Candidates / Options</Typography>
                    <List>
                        {electionOptions.map((option, index) => {
                            return (
                                <ListItem key={index}
                                          secondaryAction={<IconButton
                                              onClick={() => removeOption(index)}><DeleteForever/></IconButton>}
                                >
                                    <CandidateInfo name={option.name ?? ""}/>
                                </ListItem>
                            )
                        })}
                    </List>
                </Box>
                <Box display="flex" alignItems="right" justifyContent="right">
                    <Button variant="contained" onClick={saveElection}>Next</Button>
                </Box>
                <ProgressWithLabel isLoading={isLoading} label="Setting up election"/>
            </Stack>
        </>
    );
};
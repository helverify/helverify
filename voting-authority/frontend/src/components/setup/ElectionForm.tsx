import {
    Box,
    Button,
    FormControl, FormHelperText,
    IconButton, InputLabel,
    List,
    ListItem,
    MenuItem, Select, SelectChangeEvent,
    Stack, TextField, Tooltip, Typography
} from "@mui/material";
import {ElectionDto, ElectionOption, ElectionOptionDto} from "../../api/Api";
import React, {useEffect, useState} from "react";
import {Add, DeleteForever} from "@mui/icons-material";
import {dhGroups, DiffieHellmanGroup, ProcessStepProps} from "./processStep";
import {apiClient} from "../../api/apiClient";
import {CandidateInfo} from "../election/CandidateInfo";
import {ProgressWithLabel} from "../progress/ProgressWithLabel";
import {useNavigate} from "react-router-dom";

export const ElectionForm = (props: ProcessStepProps) => {
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

    const navigate = useNavigate();

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

    const isElectionValid = () => {
        return election.p !== "" && election.g !== ""
            && election.name && election.name.length > 0
            && election.question && election.question.length >= 0
            && election.options && election.options.length > 0;
    }

    let electionOptions = election.options ?? [];

    return (
        <>
            <Stack direction="column" spacing={1}>
                <Box>
                    <Typography variant={"h5"} style={typographyStyle}>Election Parameters</Typography>
                    <Typography>In this step, you can specify all the parameters of your election or vote, including the
                        question to vote on as well as the options or candidates.</Typography>
                    <Stack direction="column" spacing={1} style={{marginTop: "10px"}}>
                        <FormControl variant={styleVariant} sx={stylingParams}>
                            <Tooltip title={"Name of your new election"} arrow>
                                <TextField
                                    fullWidth
                                    required
                                    id="election-name"
                                    name="name"
                                    label="Name"
                                    variant={styleVariant}
                                    value={election?.name}
                                    onChange={handleChange}
                                />
                            </Tooltip>
                        </FormControl>
                        <FormControl variant={styleVariant} sx={stylingParams}>
                            <Tooltip
                                title={"The question you want to ask the voter (e.g., do you want to accept the suggestion proposed by the referendum?)"}
                                arrow
                            >
                                <TextField
                                    fullWidth
                                    required
                                    id="election-question"
                                    name="question"
                                    label="Question"
                                    variant={styleVariant}
                                    value={election?.question}
                                    onChange={handleChange}
                                />
                            </Tooltip>
                        </FormControl>
                        <FormControl variant={styleVariant} sx={stylingParams}>
                            <Stack direction="column">
                                <Tooltip title={"The name of a candidate or option respectively."} arrow>
                                    <TextField
                                        fullWidth
                                        required
                                        id="election-option"
                                        label="Add option / candidate"
                                        variant={styleVariant}
                                        value={currentOption?.name}
                                        onChange={handleCurrentOptionChange}
                                        onKeyDown={onKeyDown}
                                        style={{marginBottom: "20px"}}
                                    />
                                </Tooltip>
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
                            <Tooltip title={"Diffie-Hellman parameters used for the ElGamal cryptosystem."}
                                     placement="top" arrow>
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
                            </Tooltip>
                        </FormControl>
                        <FormControl>
                            <TextField
                                required
                                id="election-p"
                                label="ElGamal Prime p"
                                variant="standard"
                                value={election.p ?? ""}
                                disabled/>
                        </FormControl>
                        <FormControl>
                            <TextField
                                required
                                id="election-g"
                                label="ElGamal Generator g"
                                variant="standard"
                                value={election.g ?? ""}
                                disabled/>
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
                    <Button variant="contained" onClick={saveElection} disabled={!isElectionValid()}>Next</Button>
                </Box>
                <ProgressWithLabel isLoading={isLoading} label="Setting up election"/>
            </Stack>
        </>
    );
};
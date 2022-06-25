import {
    Box,
    Button,
    Card, FormControl, FormHelperText,
    Grid, IconButton, InputLabel,
    List,
    ListItem,
    ListItemText, ListSubheader, MenuItem, OutlinedInput,
    Select, SelectChangeEvent,
    Stack, TextareaAutosize,
    TextField
} from "@mui/material";
import {Api, ElectionDto, ElectionOption, ElectionOptionDto} from "../../Api";
import {useState} from "react";
import {Add, DeleteForever, Output, PlusOne} from "@mui/icons-material";
import {dhGroups, DiffieHellmanGroup, SetupStepProps} from "./electionSetupStep";

export function ElectionForm(props: SetupStepProps) {

    const styleVariant = "standard";
    const stylingParams = {minWidth: 200};

    const [election, setElection] = useState<ElectionDto>({
        name: "",
        question: "",
        p: "",
        g: "",
        options: []
    });

    const [currentOption, setCurrentOption] = useState<ElectionOptionDto>({name: ""});
    const [diffieHellmanGroup, setDiffieHellmanGroup] = useState<DiffieHellmanGroup>({name: "", p: "", g: ""});


    const addOption = (option: ElectionOption | undefined) => {
        if (option === undefined || option.name === "") {
            return;
        }

        let newElection = {...election};

        if(election.options === undefined || election.options === null){
            newElection.options = [option];
        } else {
            newElection.options = [...election.options, option];
        }


        setElection((oldElection) => ({...oldElection, ...newElection}));
        setCurrentOption({name: ""});
    }

    const removeOption = (index: number) => {
        let optionsClone: ElectionOptionDto[];
        if(election.options === undefined || election.options === null){
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

        setElection((oldElection) => ({...oldElection, ...newElection})); // https://blog.logrocket.com/using-react-usestate-object/
    };

    const handleCurrentOptionChange = (evnt: any) => {
        const {name, value} = evnt.target;

        let newOption: ElectionOption = {name: value};

        setCurrentOption((oldOption) => ({...oldOption, ...newOption}));
    }

    const setDhGroup = (evnt: SelectChangeEvent) => {
        let groupName: string = evnt.target.value;

        let newElection: ElectionDto = {...election};

        let dhGroup: DiffieHellmanGroup = dhGroups.find(group => group.name == groupName) ?? {name: "", p: "", g: ""};

        if (dhGroup.name === "") {
            return;
        }

        setDiffieHellmanGroup(dhGroup);

        newElection.p = dhGroup.p;
        newElection.g = dhGroup.g;

        setElection((oldElection) => ({...oldElection, ...newElection}));
    }

    const saveElection = async() => {
        const client = new Api({
            baseUrl: "http://localhost:5000"
        });

        await client.api.electionsCreate(election).then((result) => {
            props.next();
        });
    }

    let electionOptions = election.options ?? [];

    return (
        <>
            <Grid container spacing={1}>
                <Grid item xs={6}>
                    <Card>
                        <Stack spacing={1} sx={{m: 2}}>

                            <FormControl variant={styleVariant} sx={stylingParams}>
                                <TextField fullWidth id="election-name" name="name" label="Name" variant={styleVariant}
                                           value={election?.name} onChange={handleChange}/>
                            </FormControl>

                            <FormControl variant={styleVariant} sx={stylingParams}>
                                <TextField fullWidth id="election-question" name="question" label="Question"
                                           variant={styleVariant} value={election?.question} onChange={handleChange}/>
                            </FormControl>


                            <FormControl variant={styleVariant} sx={stylingParams}>
                                <Stack direction="row" spacing={1}>
                                    <TextField fullWidth id="election-option" label="Add option / candidate"
                                               variant={styleVariant} value={currentOption?.name}
                                               onChange={handleCurrentOptionChange}/>
                                    <Button id="election-option-add"
                                            variant="outlined"
                                            onClick={() => addOption(currentOption)}
                                            disabled={currentOption.name === ""}
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
                            <FormControl>
                                <Button variant="contained" onClick={async() => await saveElection()}>Next</Button>
                            </FormControl>
                        </Stack>
                    </Card>
                </Grid>
                <Grid item xs={6}>
                    <Card>
                        <List>
                            <ListSubheader component="div">Candidates / Options</ListSubheader>
                            {electionOptions.map((option, index) => {
                                return (
                                    <ListItem key={index}
                                              secondaryAction={<IconButton
                                                  onClick={() => removeOption(index)}><DeleteForever/></IconButton>}
                                    >
                                        <ListItemText>{option.name}</ListItemText>
                                    </ListItem>
                                )
                            })}
                        </List>
                    </Card>
                </Grid>
            </Grid>
        </>
    );
}
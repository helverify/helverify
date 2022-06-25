import {SetupStepProps} from "./electionSetupStep";
import {
    Button, Card,
    FormControl,
    Grid,
    IconButton,
    List,
    ListItem,
    ListItemText,
    ListSubheader,
    Stack,
    TextField
} from "@mui/material";
import {Add, DeleteForever} from "@mui/icons-material";
import {Api, RegistrationDto} from "../../Api";
import {useState} from "react";

export const ConsensusNodeRegistrationForm = (props: SetupStepProps) => {
    const styleVariant = "standard";
    const stylingParams = {minWidth: 200};

    const [currentRegistration, setCurrentRegistration] = useState<RegistrationDto>({name: "", endpoint: ""});

    const [registrations, setRegistrations] = useState<RegistrationDto[]>([]);

    const handleChange = (evt: any) => {
        const {name, value} = evt.target;

        let newRegistration: RegistrationDto = {};

        newRegistration[name as keyof RegistrationDto] = value;

        setCurrentRegistration((oldRegistration) => ({...oldRegistration, ...newRegistration}));
    }

    const addRegistration = () => {
        setRegistrations((oldRegistrations) => [...oldRegistrations, currentRegistration]);
        setCurrentRegistration({name: "", endpoint: ""});
    }

    const removeRegistration = (index: number) => {
        let registrationsClone: RegistrationDto[];

        if (registrations === undefined || registrations === null) {
            registrationsClone = [];
        } else {
            registrationsClone = [...registrations];
        }

        registrationsClone.splice(index, 1);

        setRegistrations(registrationsClone);
    }

    const registerNodes = () => {
        if (props.election.id === undefined || props.election.id === null) {
            return;
        }

        const client = new Api({
            baseUrl: "http://localhost:5000"
        });

        const electionId: string = props.election.id;

        // https://stackoverflow.com/questions/46027244/wait-for-promise-in-a-foreach-loop

        let promises = registrations.map((registration, index) => {
            return client.api.electionsRegistrationsCreate(electionId, registration);
        })

        Promise.all(promises).then(() => props.next(props.election));
    }

    return (
        <>
            <Grid container spacing={1}>
                <Grid item xs={6}>
                    <Card>
                        <Stack direction="column" spacing={1} sx={{m: 2}}>
                            <FormControl>
                                <Stack direction="row" spacing={1} sx={{m: 2}}>
                                    <FormControl variant={styleVariant} sx={stylingParams}>
                                        <TextField fullWidth id="consensus-node-name"
                                                   name="name"
                                                   label="Name"
                                                   variant={styleVariant}
                                                   value={currentRegistration.name}
                                                   onChange={handleChange}
                                        />
                                    </FormControl>
                                    <FormControl variant={styleVariant} sx={stylingParams}>
                                        <TextField
                                            fullWidth
                                            id="consensus-node-endpoint"
                                            name="endpoint"
                                            label="Endpoint"
                                            variant={styleVariant}
                                            value={currentRegistration.endpoint}
                                            onChange={handleChange}
                                        />
                                    </FormControl>
                                    <FormControl variant={styleVariant}>
                                        <Button id="consensus-node-add"
                                                variant="outlined"
                                                onClick={addRegistration}
                                        ><Add/></Button>
                                    </FormControl>
                                </Stack>
                                <Button variant="contained" onClick={registerNodes}>Register Consensus Nodes</Button>
                            </FormControl>
                        </Stack>
                    </Card>
                </Grid>
                <Grid item xs={6}>
                    <Card>
                        <List>
                            <ListSubheader component="div">Consensus Nodes</ListSubheader>
                            {registrations.map((node, index) => {
                                return (
                                    <ListItem key={index}
                                              secondaryAction={<IconButton
                                                  onClick={() => removeRegistration(index)}>
                                                  <DeleteForever/>
                                              </IconButton>}
                                    >
                                        <ListItemText>{node.name} - {node.endpoint}</ListItemText>
                                    </ListItem>
                                )
                            })}
                        </List>
                    </Card>
                </Grid>
            </Grid>
        </>
    );
};
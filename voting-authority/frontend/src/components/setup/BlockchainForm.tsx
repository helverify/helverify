import {SetupStepProps} from "./electionSetupStep";
import {
    Backdrop,
    Button, Card, CircularProgress,
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
import {BlockchainDto, RegistrationDto} from "../../Api";
import {useEffect, useState} from "react";
import {apiClient} from "../../api/apiClient";

export const BlockchainForm = (props: SetupStepProps) => {
    const styleVariant = "standard";
    const stylingParams = {minWidth: 200};

    const [isBlockchainDefined, setIsBlockchainDefined] = useState<boolean>(false);

    const [isLoading, setLoading] = useState<boolean>(false);

    const [blockchain, setBlockchain] = useState<BlockchainDto>({ name: "", registrations: []});

    const [currentRegistration, setCurrentRegistration] = useState<RegistrationDto>({name: "", endpoint: ""});

    const [registrations, setRegistrations] = useState<RegistrationDto[]>([]);

    const handleChange = (evt: any) => {
        const {name, value} = evt.target;

        let newRegistration: RegistrationDto = {};

        newRegistration[name as keyof RegistrationDto] = value;

        setCurrentRegistration((oldRegistration) => ({...oldRegistration, ...newRegistration}));
    }

    const handleBlockchainChange = (evt: any) => {
        const {name, value} = evt.target;

        let newBlockchain: BlockchainDto = {};

        newBlockchain[name as keyof BlockchainDto] = value;

        setBlockchain((oldBlockchain) => ({...oldBlockchain, ...newBlockchain}));
    }

    const addRegistration = () => {
        let newRegistrations = ([...registrations, currentRegistration]);
        setRegistrations(newRegistrations);
        setCurrentRegistration({name: "", endpoint: ""});

        updateBlockchain(newRegistrations);
    }

    const updateBlockchain = (newRegistrations: RegistrationDto[]) => {
        let newBlockchain: BlockchainDto = { registrations: newRegistrations};

        setBlockchain((oldBlockchain) => ({...oldBlockchain, ...newBlockchain}));
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

        updateBlockchain(registrationsClone);
    }

    const registerNodes = () => {
        if(isBlockchainDefined){
            console.log(blockchain);
            props.next({}, blockchain);
        }

        setLoading(true);

        apiClient().api.blockchainCreate(blockchain).then((result) => {
            setLoading(false);

            props.next({}, result.data);
        });
    }

    useEffect(() => {
        apiClient().api.blockchainList().then((result) => {
            if(result.data.id !== undefined && result.data.id !== null){
                setBlockchain(result.data);
                setRegistrations(result.data.registrations ?? []);
                setIsBlockchainDefined(true);
            }
        })
    }, []);

    return (
        <>
            <Backdrop open={isLoading}>
                <CircularProgress />
            </Backdrop>
            <Grid container spacing={1}>
                <Grid item xs={6}>
                    <Card>
                        <Stack direction="column" spacing={1} sx={{m: 2}}>
                            <FormControl>
                                <FormControl>
                                    <TextField fullWidth id="blockchain-name"
                                               name="name"
                                               variant={styleVariant}
                                               value={blockchain.name}
                                               onChange={handleBlockchainChange}
                                               disabled={isBlockchainDefined}
                                    />
                                </FormControl>
                                <Stack direction="row" spacing={1} sx={{m: 2}}>
                                    <FormControl variant={styleVariant} sx={stylingParams}>
                                        <TextField fullWidth id="consensus-node-name"
                                                   name="name"
                                                   label="Name"
                                                   variant={styleVariant}
                                                   value={currentRegistration.name}
                                                   onChange={handleChange}
                                                   disabled={isBlockchainDefined}
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
                                            disabled={isBlockchainDefined}
                                        />
                                    </FormControl>
                                    <FormControl variant={styleVariant}>
                                        <Button id="consensus-node-add"
                                                variant="outlined"
                                                onClick={addRegistration}
                                                disabled={isBlockchainDefined}
                                        ><Add/></Button>
                                    </FormControl>
                                </Stack>
                                <Button variant="contained" onClick={registerNodes}>Next</Button>
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
                                                  disabled={isBlockchainDefined}
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
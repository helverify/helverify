import {SetupStepProps} from "./electionSetupStep";
import {
    Backdrop, Box,
    Button, CircularProgress,
    FormControl,
    IconButton,
    List,
    ListItem,
    ListItemText,
    Stack,
    TextField, Typography
} from "@mui/material";
import {Add, DeleteForever} from "@mui/icons-material";
import {BlockchainDto, RegistrationDto} from "../../api/Api";
import {useEffect, useState} from "react";
import {apiClient} from "../../api/apiClient";

export const BlockchainForm = (props: SetupStepProps) => {
    const styleVariant = "standard";
    const stylingParams = {width: "100%"};
    const typographyStyle = {marginTop: "25px",marginBottom: "15px"};

    const [isBlockchainDefined, setIsBlockchainDefined] = useState<boolean>(false);

    const [isLoading, setLoading] = useState<boolean>(false);

    const [blockchain, setBlockchain] = useState<BlockchainDto>({name: "", registrations: []});

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
        let newBlockchain: BlockchainDto = {registrations: newRegistrations};

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
        if (isBlockchainDefined) {
            console.log(blockchain);
            props.next({}, blockchain);
        }

        setLoading(true);

        apiClient().api.blockchainCreate(blockchain).then((result) => {
            setLoading(false);

            props.next({}, result.data);
        });
    }

    const onKeyDown = (evnt: any) => {
      if(evnt.keyCode === 13){ // https://stackoverflow.com/questions/43384039/how-to-get-the-textfield-value-when-enter-key-is-pressed-in-react
          addRegistration();
      }
    };

    useEffect(() => {
        apiClient().api.blockchainList().then((result) => {
            if (result.data.id !== undefined && result.data.id !== null) {
                setBlockchain(result.data);
                setRegistrations(result.data.registrations ?? []);
                setIsBlockchainDefined(true);
            }
        })
    }, []);

    return (
        <>
            <Backdrop open={isLoading}>
                <CircularProgress/>
            </Backdrop>
            <Box>
                <Typography variant={"h5"} style={typographyStyle}>Blockchain Configuration</Typography>
                <Stack direction="column" spacing={1}>
                    <FormControl>
                        <FormControl>
                            <TextField fullWidth id="blockchain-name"
                                       name="name"
                                       label="Blockchain Name"
                                       variant={styleVariant}
                                       value={blockchain.name}
                                       onChange={handleBlockchainChange}
                                       disabled={isBlockchainDefined}
                            />
                        </FormControl>
                        <Stack direction="column" spacing={1}>
                            <Typography variant={"h5"} style={typographyStyle}>Consensus Nodes</Typography>
                            <Stack direction="row" spacing={1} style={{marginBottom: "15px"}}>
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
                                        onKeyDown={onKeyDown}
                                        disabled={isBlockchainDefined}
                                    />
                                </FormControl>
                            </Stack>
                            <FormControl variant={styleVariant}>
                                <Button id="consensus-node-add"
                                        variant="outlined"
                                        onClick={addRegistration}
                                        disabled={isBlockchainDefined}
                                ><Add/></Button>
                            </FormControl>
                        </Stack>

                    </FormControl>
                </Stack>
            </Box>
            <Box>
                <List>
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
            </Box>
            <Box display="flex" alignItems="right" justifyContent="right">
                <Button variant="contained" onClick={registerNodes}>Next</Button>
            </Box>
        </>
    );
};
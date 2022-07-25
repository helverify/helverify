import {SetupStepProps} from "./electionSetupStep";
import {
    Box,
    Button,
    FormControl,
    IconButton,
    List,
    ListItem,
    ListItemText,
    Stack,
    TextField, Tooltip, Typography
} from "@mui/material";
import {Add, DeleteForever} from "@mui/icons-material";
import {BlockchainDto, RegistrationDto} from "../../api/Api";
import {useEffect, useState} from "react";
import {apiClient} from "../../api/apiClient";
import {ProgressWithLabel} from "../progress/ProgressWithLabel";

export const BlockchainForm = (props: SetupStepProps) => {
    const styleVariant = "standard";
    const stylingParams = {width: "100%"};
    const typographyStyle = {marginTop: "25px", marginBottom: "15px"};

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

    const isBlockchainValid = () => {
        return blockchain.name !== undefined && blockchain.name !== null && blockchain.name.length > 0
            && blockchain.registrations !== undefined && blockchain.registrations !== null && blockchain.registrations.length > 0;
    }

    const registerNodes = () => {
        if (isBlockchainDefined) {
            props.next({}, blockchain);
            return;
        }

        setLoading(true);

        apiClient().api.blockchainCreate(blockchain).then((result) => {
            setLoading(false);

            props.next({}, result.data);
        });
    }

    const onKeyDown = (evnt: any) => {
        if (evnt.keyCode === 13) { // https://stackoverflow.com/questions/43384039/how-to-get-the-textfield-value-when-enter-key-is-pressed-in-react
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
            <Box>
                <Typography variant={"h5"} style={typographyStyle}>Blockchain Configuration</Typography>
                <Typography>The first step we need to perform is to set up a Proof-of-Authority Blockchain network. For this purpose, you need to specify which consensus nodes are supposed to run the network. Note that this step can be skipped if you have already set up a Blockchain network before.</Typography>
                <Stack direction="column" spacing={1} style={{marginTop: "10px"}}>
                    <FormControl variant={styleVariant} sx={stylingParams}>
                        <Tooltip
                            title={"Please provide a name for the new Blockchain."}
                            arrow
                        >
                            <TextField fullWidth
                                       required
                                       id="blockchain-name"
                                       name="name"
                                       label="Blockchain Name"
                                       variant={styleVariant}
                                       value={blockchain.name}
                                       onChange={handleBlockchainChange}
                                       disabled={isBlockchainDefined}
                            />
                        </Tooltip>
                        <Stack direction="column" spacing={1}>
                            <Typography variant={"h5"} style={typographyStyle}>Consensus Nodes</Typography>
                            <Stack direction="row" spacing={1} style={{marginBottom: "15px"}}>
                                <Tooltip
                                    title={"Name a particular consensus node (e.g., name of the municipality running the consensus node)"}
                                    arrow
                                >
                                    <TextField fullWidth id="consensus-node-name"
                                               required
                                               name="name"
                                               label="Name"
                                               variant={styleVariant}
                                               value={currentRegistration.name}
                                               onChange={handleChange}
                                               disabled={isBlockchainDefined}
                                    />
                                </Tooltip>
                                <Tooltip
                                    title={"Endpoint address of a consensus node. If you want to connect to a local docker network, use \"host.docker.internal\" instead of \"localhost\" as a hostname."}
                                    arrow
                                >
                                    <TextField
                                        fullWidth
                                        required
                                        id="consensus-node-endpoint"
                                        name="endpoint"
                                        label="Endpoint"
                                        variant={styleVariant}
                                        value={currentRegistration.endpoint}
                                        onChange={handleChange}
                                        onKeyDown={onKeyDown}
                                        disabled={isBlockchainDefined}
                                    />
                                </Tooltip>
                            </Stack>
                            <Button id="consensus-node-add"
                                    variant="outlined"
                                    onClick={addRegistration}
                                    disabled={isBlockchainDefined}
                            ><Add/></Button>
                        </Stack>
                    </FormControl>
                </Stack>
            </Box>
            <Box>
                <List>
                    {registrations.map((node, index) => {
                        return (
                            <ListItem key={index}
                                      secondaryAction={
                                          <IconButton
                                              disabled={isBlockchainDefined}
                                              onClick={() => removeRegistration(index)}>
                                              <DeleteForever/>
                                          </IconButton>
                                      }
                            >
                                <ListItemText>{node.name} - {node.endpoint}</ListItemText>
                            </ListItem>
                        )
                    })}
                </List>
            </Box>
            <Box display="flex" alignItems="right" justifyContent="right">
                <Button variant="contained" onClick={registerNodes} disabled={!isBlockchainValid()}>Next</Button>
            </Box>
            <ProgressWithLabel isLoading={isLoading} label="Setting up blockchain"/>
        </>
    );
};
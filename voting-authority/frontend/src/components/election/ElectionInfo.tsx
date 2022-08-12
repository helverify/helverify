import {ElectionInfoProps} from "./electionInfo";
import {
    Box,
    Card,
    CardContent,
    Chip,
    Divider,
    Grid,
    Stack,
    TextField,
    Tooltip,
    Typography
} from "@mui/material";
import {ElectionOptionDto} from "../../api/Api";
import {CandidateInfo} from "./CandidateInfo";
import {ClipboardCopy} from "../../ClipboardCopy";

export const ElectionInfo = (props: ElectionInfoProps) => {
    const notAvailable: string = "n/a";
    const headerStyle = { marginTop: "10px", marginBottom: "6px"};

    if (props.election.id === undefined) {
        return (<></>);
    }

    const isPropertySet = (property: string | null | undefined) => {
        return !(property === undefined || property === null);
    }

    const areOptionsSet = (property: ElectionOptionDto[] | null | undefined) => {
        return !(property === undefined || property === null || property.length === 0);
    }

    const options = areOptionsSet(props.election.options) ? props.election.options : [];

    return (
        <>
            <Box>
                <Card variant="elevation">
                    <CardContent>
                        <Stack direction="column" spacing={1}>
                            <Box display="flex" justifyContent="space-between">
                                <Typography variant="h4">{props.election.name}</Typography>
                                {isPropertySet(props.election.id) && (
                                    <Tooltip title={"Election Identifier"}>
                                        <Chip label={props.election.id ?? notAvailable}/>
                                    </Tooltip>
                                )}
                            </Box>
                            <Divider/>
                            {isPropertySet(props.election.question) && (
                                <Box>
                                    <Typography variant="h5" style={headerStyle}>Question</Typography>
                                    <Typography
                                        color="text.secondary">{props.election.question ?? notAvailable}</Typography>
                                </Box>
                            )}
                            <Divider/>
                            {options && (
                                <Box>
                                    <Typography variant="h5" style={headerStyle}>Candidates / Options</Typography>
                                    <Grid container>
                                        {options.map((option, index) => {
                                            return (
                                                <Grid item key={index}>
                                                    <CandidateInfo name={option.name ?? ""} key={index}/>
                                                </Grid>
                                            )
                                        })}
                                    </Grid>
                                </Box>
                            )}
                            <Divider/>
                            {isPropertySet(props.election.publicKey) && (
                                <div>
                                    <Typography variant="h5" style={headerStyle}>Public Key</Typography>
                                    <Box display="flex" justifyContent="space-between">
                                        <TextField
                                            fullWidth
                                            variant="standard"
                                            value={props.election.publicKey}
                                            disabled/>
                                        <ClipboardCopy message={"Copied public key to clipboard."} value={props.election.publicKey ?? ""}/>
                                    </Box>
                                </div>
                            )}
                            {isPropertySet(props.election.contractAddress) && (
                                <div>
                                    <Typography variant="h5" style={headerStyle}>Contract Address</Typography>
                                    <Box display="flex" justifyContent="space-between">
                                        <TextField
                                            fullWidth
                                            variant="standard"
                                            value={props.election.contractAddress}
                                            disabled/>
                                        <ClipboardCopy message={"Copied contract address to clipboard."} value={props.election.contractAddress ?? ""}/>
                                    </Box>
                                </div>
                            )}
                        </Stack>
                    </CardContent>
                </Card>
            </Box>
        </>
    );
}
import {SpoiltBallot} from "../ballot/spoiltBallot";
import {Avatar, Box, Card, Stack, Typography} from "@mui/material";
import {EncryptedBallot} from "../ballot/encryptedBallot";
import {ElectionParameters} from "../election/election";
import {ValidityIcon} from "./ValidityIcon";
import {BallotService} from "../services/ballotService";

export type SpoiltBallotViewProps = {
    ballot: SpoiltBallot;
    encryptions: EncryptedBallot[];
    electionParameters: ElectionParameters;
}

// sorting according to: https://www.w3schools.com/js/js_array_sort.asp
export const SpoiltBallotView = (props: SpoiltBallotViewProps) => {

    const verifyEncryption = (): boolean => {
        const spoiltBallot: EncryptedBallot =  props.encryptions.filter(e => e.ballotId === props.ballot.ballotId)[0];

        return BallotService.verifyEncryptions(props.ballot, spoiltBallot, props.electionParameters);
    }

    return (
        <Card>
            <Box sx={{m: 1}}>
                <Stack direction="column" spacing={1}>
                    <Typography variant={"h4"}>Spoilt Ballot</Typography>
                    <Typography variant={"h5"}>Options</Typography>
                    {props.ballot.options.sort((a, b) => a.position - b.position).map((o, index) => {
                        return (
                            <Stack key={index} direction="row" spacing={2}>
                                <Avatar style={{
                                    backgroundColor: "#3a3a3a",
                                    color: "#FFFFFF",
                                    width: "25px",
                                    height: "25px"
                                }}>
                                    <Typography variant="overline">{o.shortCode}</Typography>
                                </Avatar>
                                <Typography color="text.secondary">{o.name}</Typography>
                            </Stack>
                        )
                    })}
                    <Typography variant="h5">Correct re-encryption?</Typography>
                    <ValidityIcon isValid={verifyEncryption()}/>
                </Stack>
            </Box>
        </Card>
    );
}
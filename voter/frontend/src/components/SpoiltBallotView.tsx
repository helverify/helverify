import {SpoiltBallot} from "../ballot/spoiltBallot";
import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Avatar,
    Box,
    Card,
    Divider,
    Grid,
    Stack,
    Typography
} from "@mui/material";
import {EncryptedBallot} from "../ballot/encryptedBallot";
import {ElectionParameters} from "../election/election";
import {ValidityIcon} from "./ValidityIcon";
import {BallotService} from "../services/ballotService";
import {ExpandMore} from "@mui/icons-material";
import React from "react";

export type SpoiltBallotViewProps = {
    ballot: SpoiltBallot;
    encryptions: EncryptedBallot[];
    electionParameters: ElectionParameters;
}

export const SpoiltBallotView = (props: SpoiltBallotViewProps) => {

    const verifyEncryption = (): boolean => {
        const spoiltBallot: EncryptedBallot = props.encryptions.filter(e => e.ballotCode === props.ballot.ballotCode)[0];

        return BallotService.verifyEncryptions(props.ballot, spoiltBallot, props.electionParameters);
    }

    // sorting according to: https://www.w3schools.com/js/js_array_sort.asp

    return (
        <Card sx={{m: 1}}>
            <Box sx={{m: 2}}>
                <Stack direction="column" spacing={1}>
                    <Typography variant={"h5"}>Spoilt Column Options</Typography>
                    {props.ballot.options.sort((a, b) => a.position - b.position).map((o, index) => {
                        return (
                            <Stack key={index} direction="row" spacing={2}>
                                <Avatar key={index} style={{
                                    backgroundColor: "#3a3a3a",
                                    color: "#FFFFFF"
                                }}>
                                    <Typography variant="h6" fontWeight={"bold"}>{o.shortCode}</Typography>
                                </Avatar>
                                <Typography variant="h6">{o.name}</Typography>

                            </Stack>
                        )
                    })}
                    <Divider/>
                    <Stack direction="row" spacing={1}>
                        <ValidityIcon isValid={verifyEncryption()}/>
                        <Typography style={{marginTop: "5px"}}>Does re-encrypting the spoilt column result in the same
                            encryption?</Typography>
                    </Stack>
                </Stack>
            </Box>
        </Card>
    );
}
import {
    Accordion,
    AccordionSummary,
    AccordionDetails,
    Box,
    Card,
    Checkbox,
    Stack,
    Typography,
    Avatar
} from "@mui/material";
import {EncryptedBallotVerification} from "./EncryptedBallotVerification";
import React, {useState} from "react";
import {EncryptedBallot} from "../cryptography/encryptedBallot";
import {ElectionParameters} from "../election/election";
import {HashHelper} from "../helper/hashHelper";
import {Check, Close, ExpandMore} from "@mui/icons-material";
import {ValidityIcon} from "./ValidityIcon";

export type EncryptedBallotViewProps = {
    ballots: EncryptedBallot[] | undefined;
    electionParameters: ElectionParameters | undefined;
    ballotId: string;
}

export const EncryptedBallotsView = (props: EncryptedBallotViewProps) => {
    const checkStyle = {color: "#00FF00"};
    const xmarkStyle = {color: "#FF0000"};

    const ballots: EncryptedBallot[] | undefined = props.ballots;
    const electionParameters: ElectionParameters | undefined = props.electionParameters;
    const ballotId: string = props.ballotId;

    const hasBallots = ballots?.length === 2 && electionParameters !== undefined;

    const [isBallot1Valid, setBallot1Valid] = useState<boolean>();
    const [isBallot2Valid, setBallot2Valid] = useState<boolean>();

    const isBallotIdCorrect = (): boolean => {
        if (!hasBallots) {
            return false;
        }

        return ballotId === HashHelper.getHashOfStrings([ballots[0].ballotId, ballots[1].ballotId]);
    }

    const handleBallot1Change = (isValid: boolean) => {
        setBallot1Valid(isValid);
    }

    const handleBallot2Change = (isValid: boolean) => {
        setBallot2Valid(isValid);
    }

    return (
        <>
            <Card>
                <Box sx={{m: 1}}>
                    <Typography variant={"h4"}>Encrypted Ballot Verification</Typography>

                    {hasBallots && (
                        <>
                            <Stack direction="column" spacing={1} sx={{m:1}}>
                                <Accordion>
                                    <AccordionSummary
                                        expandIcon={<ExpandMore/>}
                                    >
                                        <Stack direction="row" spacing={1}>
                                            <Typography variant={"h5"}>Paper Ballot</Typography>
                                            <Typography variant="overline">{ballotId}</Typography>
                                        </Stack>
                                    </AccordionSummary>
                                    <AccordionDetails>
                                        <Typography color="text.secondary">Is the Paper Ballot ID correct?</Typography>
                                        <ValidityIcon isValid={isBallotIdCorrect()}/>
                                    </AccordionDetails>
                                </Accordion>

                                <Accordion>
                                    <AccordionSummary
                                        expandIcon={<ExpandMore/>}
                                    >
                                        <Stack direction="row" spacing={1}>
                                            <ValidityIcon isValid={isBallot1Valid}/>
                                            <Typography variant="h5">Ballot 1</Typography>
                                            <Typography variant="overline">{ballots[0].ballotId}</Typography>
                                        </Stack>
                                    </AccordionSummary>
                                    <AccordionDetails>
                                        <EncryptedBallotVerification caption="Ballot 1" ballot={ballots[0]}
                                                                     electionParameters={electionParameters}
                                                                     setValidity={handleBallot1Change}
                                        />
                                    </AccordionDetails>
                                </Accordion>
                                <Accordion>
                                    <AccordionSummary
                                        expandIcon={<ExpandMore/>}
                                    >
                                        <Stack direction="row" spacing={1}>
                                            <ValidityIcon isValid={isBallot2Valid}/>
                                            <Typography variant="h5">Ballot 2</Typography>
                                            <Typography variant="overline">{ballots[1].ballotId}</Typography>
                                        </Stack>
                                    </AccordionSummary>
                                    <AccordionDetails>
                                        <EncryptedBallotVerification caption="Ballot 2" ballot={ballots[1]}
                                                                     electionParameters={electionParameters}
                                                                     setValidity={handleBallot2Change}
                                        />
                                    </AccordionDetails>
                                </Accordion>
                            </Stack>
                        </>
                    )}
                </Box>
            </Card>
        </>
    );
}
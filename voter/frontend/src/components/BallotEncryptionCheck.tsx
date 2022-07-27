import {
    Accordion,
    AccordionSummary,
    AccordionDetails,
    Box,
    Stack,
    Typography, Backdrop, CircularProgress, Divider, Chip, Tooltip, Toolbar
} from "@mui/material";
import {EncryptedBallotVerification} from "./EncryptedBallotVerification";
import React, {useEffect, useState} from "react";
import {EncryptedBallot} from "../ballot/encryptedBallot";
import {ElectionParameters} from "../election/election";
import {HashHelper} from "../helper/hashHelper";
import {ExpandMore} from "@mui/icons-material";
import {ValidityIcon} from "./ValidityIcon";

export type BallotEncryptionCheckProps = {
    ballots: EncryptedBallot[] | undefined;
    electionParameters: ElectionParameters | undefined;
    ballotId: string;
    isLoading: boolean;
    setLoading: (isLoading: boolean) => void;
}

export const BallotEncryptionCheck = (props: BallotEncryptionCheckProps) => {
    const textStyle = {marginTop: "5px"};
    const dividerStyle = {marginTop: "15px", marginBottom: "5px"}
    const titleStyle = {marginTop: "10px", marginBottom: "10px"}

    const ballots: EncryptedBallot[] | undefined = props.ballots;
    const electionParameters: ElectionParameters | undefined = props.electionParameters;
    const ballotId: string = props.ballotId;

    const hasBallots = ballots?.length === 2 && electionParameters !== undefined;

    const [isBallot1Verifying, setBallot1Verifying] = useState<boolean>(true);
    const [isBallot2Verifying, setBallot2Verifying] = useState<boolean>(true);
    const [isBallot1Valid, setBallot1Valid] = useState<boolean>();
    const [isBallot2Valid, setBallot2Valid] = useState<boolean>();

    const isBallotIdCorrect = (): boolean => {
        if (!hasBallots) {
            return false;
        }

        return ballotId === HashHelper.getHashOfStrings([ballots[0].ballotCode, ballots[1].ballotCode]);
    }

    const handleBallot1Change = (isValid: boolean) => {
        setBallot1Valid(isValid);
        setBallot1Verifying(false);
    }

    const handleBallot2Change = (isValid: boolean) => {
        setBallot2Valid(isValid);
        setBallot2Verifying(false);
    }

    const isBallotValid = () => {
        return isBallot1Valid && isBallot2Valid && isBallotIdCorrect()
    };

    const isDoneVerfying = () => {
        return !(isBallot1Verifying || isBallot2Verifying);
    }

    useEffect(() => {
        props.setLoading(true);
        if(isDoneVerfying()){
            props.setLoading(false);
        }
    })

    return (
        <>
            <Box>
                {hasBallots && (
                    <>

                        <Accordion>
                            <AccordionSummary
                                expandIcon={<ExpandMore/>}
                            >
                                <Stack direction="row" spacing={1}>
                                    <ValidityIcon isValid={isBallotValid()}/>
                                    {isBallotValid() && (
                                        <Typography variant={"h5"}>Your ballot has been generated and encrypted correctly.</Typography>
                                    )}
                                    {!isBallotValid() && (
                                        <Typography variant={"h5"}>Your ballot is invalid, please expand the
                                            advanced options to learn about the problems with your
                                            ballot.</Typography>
                                    )}
                                </Stack>
                            </AccordionSummary>
                            <AccordionDetails>
                                <Stack direction={"column"}>
                                    <Typography variant={"h5"} style={titleStyle}>General</Typography>
                                    <Stack direction="row" spacing={1}>
                                        <ValidityIcon isValid={isBallotIdCorrect()}/>
                                        <Typography style={textStyle}>Is the Paper Ballot
                                            ID correct?</Typography>
                                    </Stack>
                                    <Divider style={dividerStyle}/>
                                    <Stack direction={"row"} spacing={1} style={titleStyle}>
                                        <Tooltip
                                            title="This section shows the verification results of the first column of short codes on your ballot."
                                            placement="top"
                                            arrow
                                        >
                                            <Typography variant="h5">Column 1</Typography>
                                        </Tooltip>
                                    </Stack>
                                    <Stack direction="row" spacing={1}>
                                        <EncryptedBallotVerification caption="Ballot 1" ballot={ballots[0]}
                                                                     electionParameters={electionParameters}
                                                                     setValidity={handleBallot1Change}
                                        />
                                    </Stack>
                                    <Divider style={dividerStyle}/>
                                    <Stack direction={"row"} spacing={1} style={titleStyle}>
                                        <Tooltip
                                            title="This section shows the verification results of the second column of short codes on your ballot."
                                            placement="top"
                                            arrow
                                        >
                                            <Typography variant="h5">Column 2</Typography>
                                        </Tooltip>
                                    </Stack>
                                    <Stack direction="row" spacing={1}>
                                        <EncryptedBallotVerification caption="Ballot 2" ballot={ballots[1]}
                                                                     electionParameters={electionParameters}
                                                                     setValidity={handleBallot2Change}
                                        />
                                    </Stack>
                                </Stack>
                            </AccordionDetails>
                        </Accordion>
                    </>
                )}
            </Box>
            <Backdrop open={props.isLoading}>
                <Stack direction="row" spacing={1}>
                    <Typography variant="overline" style={{marginTop: "4px"}}>Verifying Ballot Encryption
                        ...</Typography>
                    <CircularProgress/>
                </Stack>
            </Backdrop>
        </>
    );
}
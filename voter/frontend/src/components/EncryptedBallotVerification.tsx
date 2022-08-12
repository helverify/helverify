import {EncryptedBallot} from "../ballot/encryptedBallot";
import {ElectionParameters} from "../election/election";
import {Box, Typography, Stack} from "@mui/material";
import {useEffect} from "react";
import {ValidityIcon} from "./ValidityIcon";

export type EncryptedBallotVerificationProps = {
    caption: string;
    ballot: EncryptedBallot;
    electionParameters: ElectionParameters;
    setValidity: (isValid: boolean) => void;
}

export const EncryptedBallotVerification = (props: EncryptedBallotVerificationProps) => {
    const textStyle = {marginTop: "5px"};

    const areRowsValid: boolean = props.ballot.verifyRowProofs(props.electionParameters);
    const areColumnsValid: boolean = props.ballot.verifyColumnProofs(props.electionParameters);
    const containsOnlyZeroOrOne: boolean = props.ballot.verifyContainsOnlyZeroOrOne(props.electionParameters);
    const areShortCodesCorrect: boolean = props.ballot.verifyShortCodes();
    const isBallotIdCorrect: boolean = props.ballot.verifyBallotId();

    useEffect(() => {
        const isValid: boolean | undefined = areRowsValid && areColumnsValid && containsOnlyZeroOrOne && areShortCodesCorrect && isBallotIdCorrect;

        props.setValidity(isValid ?? false);
    }, [props, areColumnsValid, areRowsValid, areShortCodesCorrect, containsOnlyZeroOrOne, isBallotIdCorrect]);

    return (
        <Box>
            <Stack direction="column" spacing={1}>
                <Stack direction="row" spacing={1}>
                    <ValidityIcon isValid={areRowsValid}/>
                    <Typography style={textStyle}>Each encrypted option represents exactly one candidate (row sums
                        up to 1)</Typography>
                </Stack>
                <Stack direction="row" spacing={1}>
                    <ValidityIcon isValid={areColumnsValid}/>
                    <Typography style={textStyle}>Each candidate is represented by exactly one encrypted option
                        (column sums up to 1)</Typography>
                </Stack>
                <Stack direction="row" spacing={1}>
                    <ValidityIcon isValid={containsOnlyZeroOrOne}/>
                    <Typography style={textStyle}>Each encrypted value is either an encryption of 0 or 1</Typography>
                </Stack>
                <Stack direction="row" spacing={1}>
                    <ValidityIcon isValid={areShortCodesCorrect}/>
                    <Typography style={textStyle}>Are the short codes correct?</Typography>
                </Stack>
                <Stack direction="row" spacing={1}>
                    <ValidityIcon isValid={isBallotIdCorrect}/>
                    <Typography style={textStyle}>Is the ballot code correct?</Typography>
                </Stack>
            </Stack>
        </Box>
    );
}
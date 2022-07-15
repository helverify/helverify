import {EncryptedBallot} from "../cryptography/encryptedBallot";
import {ElectionParameters} from "../election/election";
import {Box, Card, CardContent, Typography, Stack} from "@mui/material";
import bigInt from "big-integer";
import {useEffect} from "react";
import {ValidityIcon} from "./ValidityIcon";

export type EncryptedBallotVerificationProps = {
    caption: string;
    ballot: EncryptedBallot;
    electionParameters: ElectionParameters;
    setValidity: (isValid: boolean) => void;
}

export const EncryptedBallotVerification = (props: EncryptedBallotVerificationProps) => {
    const publicKey: bigInt.BigInteger = props.electionParameters.publicKey;
    const p: bigInt.BigInteger = props.electionParameters.p;
    const g: bigInt.BigInteger = props.electionParameters.g;

    const areRowsValid: boolean = props.ballot.rowProofs.every(rp => rp.verify(publicKey, p, g));
    const areColumnsValid: boolean = props.ballot.columnProofs.every(cp => cp.verify(publicKey, p, g));
    const containsOnlyZeroOrOne: boolean = props.ballot.encryptedOptions.every(eo => eo.values.every(v => v.verifyProof(publicKey, p, g)));
    const areShortCodesCorrect: boolean = props.ballot.verifyShortCodes();
    const isBallotIdCorrect: boolean = props.ballot.verifyBallotId();

    useEffect(() => {
        props.setValidity(areRowsValid && areColumnsValid && containsOnlyZeroOrOne && areShortCodesCorrect && isBallotIdCorrect);
    }, []);

    return (
        <Box>
            <Card variant="elevation">
                <CardContent>
                    <Stack direction="column">
                        <div>
                            <Typography color="text.secondary">All encryptions in a row sum up to 1</Typography>
                            <ValidityIcon isValid={areRowsValid}/>
                        </div>
                        <div>
                            <Typography color="text.secondary">All encryptions in a column sum up to 1</Typography>
                            <ValidityIcon isValid={areColumnsValid}/>
                        </div>
                        <div>
                            <Typography color="text.secondary">Each encryption is either an encryption of 0 or
                                1</Typography>
                            <ValidityIcon isValid={containsOnlyZeroOrOne}/>
                        </div>
                        <div>
                            <Typography color="text.secondary">Are the short codes correct?</Typography>
                            <ValidityIcon isValid={areShortCodesCorrect}/>
                        </div>
                        <div>
                            <Typography color="text.secondary">Is the ballot ID correct?</Typography>
                            <ValidityIcon isValid={isBallotIdCorrect}/>
                        </div>
                    </Stack>
                </CardContent>
            </Card>
        </Box>
    );
}
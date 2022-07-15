import {EncryptedBallot} from "../cryptography/encryptedBallot";
import {ElectionParameters} from "../election/election";
import {Box, Card, CardContent, Typography, Stack} from "@mui/material";
import {Check, Close, Warning} from "@mui/icons-material";
import bigInt from "big-integer";

export type EncryptedBallotVerificationProps = {
    caption: string;
    ballot: EncryptedBallot;
    electionParameters: ElectionParameters;
}

export const EncryptedBallotVerification = (props: EncryptedBallotVerificationProps) => {
    const checkmarkStyle = {color: "#00FF00"};
    const xmarkStyle = {color: "#FF0000"};

    const publicKey: bigInt.BigInteger = props.electionParameters.publicKey;
    const p: bigInt.BigInteger = props.electionParameters.p;
    const g: bigInt.BigInteger = props.electionParameters.g;

    const areRowsValid: boolean = props.ballot.rowProofs.every(rp => rp.verify(publicKey, p, g));
    const areColumnsValid: boolean = props.ballot.columnProofs.every(cp => cp.verify(publicKey, p, g));
    const containsOnlyZeroOrOne: boolean = props.ballot.encryptedOptions.every(eo => eo.values.every(v => v.verifyProof(publicKey, p, g)));
    const areShortCodesCorrect: boolean = props.ballot.verifyShortCodes();
    const isBallotIdCorrect: boolean = props.ballot.verifyBallotId();

    return (
        <Box>
            <Card variant="elevation">
                <CardContent>
                    <Stack direction="column">
                        <Typography variant="h4">{props.caption}</Typography>
                        <Typography variant="overline">{props.ballot.ballotId}</Typography>
                        <div>
                            <Typography color="text.secondary">All encryptions in a row sum up to 1</Typography>
                            {areRowsValid ? <Check style={checkmarkStyle}/> : <Close style={xmarkStyle}/>}
                        </div>
                        <div>
                            <Typography color="text.secondary">All encryptions in a column sum up to 1</Typography>
                            {areColumnsValid ? <Check style={checkmarkStyle}/> : <Close style={xmarkStyle}/>}
                        </div>
                        <div>
                            <Typography color="text.secondary">Each encryption is either an encryption of 0 or
                                1</Typography>
                            {containsOnlyZeroOrOne ? <Check style={checkmarkStyle}/> : <Close style={xmarkStyle}/>}
                        </div>
                        <div>
                            <Typography color="text.secondary">Are the short codes correct?</Typography>
                            {areShortCodesCorrect ? <Check style={checkmarkStyle}/> : <Close style={xmarkStyle}/>}
                        </div>
                        <div>
                            <Typography color="text.secondary">Is the ballot ID correct?</Typography>
                            {isBallotIdCorrect ? <Check style={checkmarkStyle}/> : <Close style={xmarkStyle}/>}
                        </div>
                    </Stack>
                </CardContent>
            </Card>
        </Box>
    );
}
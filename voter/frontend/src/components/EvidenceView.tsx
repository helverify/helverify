import {ResultEvidence} from "../election/resultEvidence";
import {Box, Card, Stack, Typography} from "@mui/material";
import {ValidityIcon} from "./ValidityIcon";
import {ElectionParameters} from "../election/election";
import bigInt from "big-integer";
import React, {useEffect, useState} from "react";

export type EvidenceViewProps = {
    electionParameters: ElectionParameters;
    electionEvidence: ResultEvidence;
}

export const EvidenceView = (props: EvidenceViewProps) => {
    const [isValid, setIsValid] = useState<boolean>();

    useEffect(() => {
        const p: bigInt.BigInteger = props.electionParameters.p;
        const g: bigInt.BigInteger = props.electionParameters.g;

        props.electionEvidence.verifyDecryptionProofs(p, g).then((valid) => {
            setIsValid(valid);
        });
    }, [props])

    return (
        <>
            <Card>
                <Box sx={{m: 1}}>
                    <Stack spacing={1} direction="row">
                        <ValidityIcon isValid={isValid}/>
                        <Typography style={{marginTop: "5px"}}>Have the final results been decrypted
                            correctly?</Typography>
                    </Stack>
                </Box>
            </Card>
        </>);
}
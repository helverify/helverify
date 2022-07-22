import {ElectionInfoProps} from "./electionInfo";
import {Box, Card, CardContent, Grid, Stack, Typography} from "@mui/material";
import {ElectionOptionDto} from "../../api/Api";
import {CandidateInfo} from "./CandidateInfo";

export const ElectionInfo = (props: ElectionInfoProps) => {

    const notAvailable: string = "n/a";

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
        <Box>
            <Card variant="elevation">
                <CardContent>
                    <Stack direction="column" spacing={1}>
                        <Typography variant="h4">{props.election.name}</Typography>
                        {isPropertySet(props.election.id) && (
                            <div>
                                <Typography variant="h5">ID</Typography>
                                <Typography color="text.secondary">{props.election.id ?? notAvailable}</Typography>
                            </div>
                        )}

                        {isPropertySet(props.election.question) && (
                            <div>
                                <Typography variant="h5">Question</Typography>
                                <Typography
                                    color="text.secondary">{props.election.question ?? notAvailable}</Typography>
                            </div>
                        )}

                        {options && (
                            <>
                                <Typography variant="h5">Candidates / Options</Typography>
                                <Grid container>
                                    {options.map((option, index) => {
                                        return (
                                            <Grid item key={index}>
                                                <CandidateInfo name={option.name ?? ""} key={index}/>
                                            </Grid>
                                        )
                                    })}
                                </Grid>
                            </>
                        )}

                        {isPropertySet(props.election.publicKey) && (
                            <div>
                                <Typography variant="h5">Public Key</Typography>
                                <Typography color="text.secondary"
                                            style={{wordBreak: "break-all"}}>{props.election.publicKey ?? notAvailable}</Typography>
                            </div>
                        )}
                        {isPropertySet(props.election.contractAddress) && (
                            <div>
                                <Typography variant="h5">Contract Address</Typography>
                                <Typography
                                    color="text.secondary">{props.election.contractAddress ?? notAvailable}</Typography>
                            </div>
                        )}
                    </Stack>
                </CardContent>
            </Card>
        </Box>

    );
}
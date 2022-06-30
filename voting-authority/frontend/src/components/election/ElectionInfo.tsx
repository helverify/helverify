import {ElectionInfoProps} from "./electionInfo";
import {Box, Card, CardContent, Stack, Typography} from "@mui/material";
import {ElectionOptionDto} from "../../api/Api";

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
        <Box sx={{m: 2}}>
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
                                <Typography color="text.secondary">{props.election.question ?? notAvailable}</Typography>
                            </div>
                        )}

                        {options && (
                            <>
                                <Typography variant="h5">Candidates / Options</Typography>
                                {options.map((option, index) => {
                                    return (
                                        <Typography color="text.secondary" key={index}>{option.name}</Typography>
                                    )
                                })}
                            </>
                        )}

                        {isPropertySet(props.election.publicKey) && (
                            <div>
                                <Typography variant="h5">Public Key</Typography>
                                <Typography color="text.secondary" style={{wordBreak: "break-all"}}>{props.election.publicKey ?? notAvailable}</Typography>
                            </div>
                        )}
                        {isPropertySet(props.election.contractAddress) && (
                            <div>
                                <Typography variant="h5">Contract Address</Typography>
                                <Typography color="text.secondary">{props.election.contractAddress ?? notAvailable}</Typography>
                            </div>
                        )}
                    </Stack>
                </CardContent>
            </Card>
        </Box>

    );
}
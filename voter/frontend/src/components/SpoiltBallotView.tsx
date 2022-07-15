import {SpoiltBallot} from "../ballot/spoiltBallot";
import {Avatar, Box, Card, Stack, Typography} from "@mui/material";

export type SpoiltBallotViewProps = {
    ballot: SpoiltBallot;
}

// sorting according to: https://www.w3schools.com/js/js_array_sort.asp
export const SpoiltBallotView = (props: SpoiltBallotViewProps) => {
    return (
        <Card>
            <Box sx={{m: 1}}>
                <Stack direction="column" spacing={1}>
                    <Typography variant={"h4"}>Spoilt Ballot</Typography>
                    <Typography variant={"h5"}>Options</Typography>
                    {props.ballot.options.sort((a, b) => a.position - b.position).map(o => {
                        return (
                            <>
                                <Stack direction="row" spacing={2}>
                                    <Avatar style={{backgroundColor: "#3a3a3a", color: "#FFFFFF", width: "25px", height: "25px"}}>
                                        <Typography variant="overline">{o.shortCode}</Typography>
                                    </Avatar>
                                    <Typography color="text.secondary">{o.name}</Typography>
                                </Stack>
                            </>
                        )
                    })}
                </Stack>
            </Box>
        </Card>
    );
}
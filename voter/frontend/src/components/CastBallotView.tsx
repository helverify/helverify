import {CastBallot} from "../ballot/castBallot";
import {Avatar, Box, Card, Stack, Typography} from "@mui/material";

export type CastBallotViewProps = {
    ballot: CastBallot;
}

export const CastBallotView = (props: CastBallotViewProps) => {

    return (
        <Card>
            <Box sx={{m: 1}}>
                <Stack direction="column" spacing={1}>
                    <Typography variant="h4">Submitted Ballot</Typography>
                    <Typography variant="h5">Choices</Typography>
                    <Stack direction="row" spacing={1}>
                        {props.ballot.selection.map((selection,index) => {
                            return (
                                <Avatar key={index} style={{
                                    backgroundColor: "#3a3a3a",
                                    color: "#FFFFFF",
                                    width: "25px",
                                    height: "25px"
                                }}>
                                    <Typography variant="overline">{selection}</Typography>
                                </Avatar>
                            );
                        })}
                    </Stack>
                </Stack>
            </Box>
        </Card>
    );
}
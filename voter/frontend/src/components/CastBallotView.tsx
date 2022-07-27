import {CastBallot} from "../ballot/castBallot";
import {Avatar, Box, Card, Grid, Stack, Typography} from "@mui/material";

export type CastBallotViewProps = {
    ballot: CastBallot;
}

export const CastBallotView = (props: CastBallotViewProps) => {

    return (
        <Card sx={{m: 1}}>
            <Box sx={{m: 2}}>
                <Stack direction="column" spacing={1}>
                    <Typography variant="h5">Your Choices</Typography>
                    <Grid container spacing={1}>
                        {props.ballot.selection.map((selection, index) => {
                            return (
                                <Grid item>
                                    <Avatar key={index} style={{
                                        backgroundColor: "#3a3a3a",
                                        color: "#FFFFFF"
                                    }}>
                                        <Typography variant="h6" fontWeight={"bold"}>{selection}</Typography>
                                    </Avatar>
                                </Grid>
                            );
                        })}
                    </Grid>
                </Stack>
            </Box>
        </Card>
    );
}
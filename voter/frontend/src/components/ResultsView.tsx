import {Avatar, Box, Card, Stack, Typography} from "@mui/material";
import {ElectionResults} from "../election/election";

export type ResultsViewProps = {
    electionResults: ElectionResults;
}

export const ResultsView = (props: ResultsViewProps) => {

    return (
        <Card>
            <Box sx={{m: 1}}>
                <Stack spacing={1}>
                    <Typography variant="h4">Election</Typography>
                    <Typography variant="h5">Final Results</Typography>
                        {props.electionResults.results.map((option, index) => {
                            return (
                                <Stack key={index} direction="row" spacing={1}>

                                    <Typography variant="body1">{option.count}</Typography>

                                    <Typography color="text.secondary">{option.name}</Typography>
                                </Stack>
                            );
                        })}
                </Stack>
            </Box>
        </Card>
    );
}
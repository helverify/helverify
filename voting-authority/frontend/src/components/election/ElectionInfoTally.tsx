import {apiClient} from "../../api/apiClient";
import {
    Avatar,
    Box,
    Button,
    Card,
    CardActions,
    CardContent,
    CardHeader,
    Divider,
    Grid,
    LinearProgress,
    Typography
} from "@mui/material";
import {HowToVote, Percent, PieChart} from "@mui/icons-material";
import React, {useState} from "react";
import {ProgressWithLabel} from "../progress/ProgressWithLabel";
import {useErrorHandler} from "react-error-boundary";
import {ElectionDto, ElectionStatisticsDto} from "../../api/Api";

export type ElectionInfoTallyProps = {
    election: ElectionDto,
    statistics: ElectionStatisticsDto
};

export const ElectionInfoTally = (props: ElectionInfoTallyProps) => {
    const cardItemStyle = {minHeight: "140px"};

    const [isLoading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>();

    const calculateResult = (electionId: string) => {
        if (electionId === "") {
            return;
        }
        setLoading(true);
        apiClient().api.electionsTallyCreate(electionId).then((result) => {
            setLoading(false);
            if (result.status !== 200) {
                setError(result.error);
            }
        });
    };

    let progress: number = 0;

    if (props.statistics !== undefined && props.statistics !== null && props.statistics.numberOfBallotsCast !== undefined && props.statistics.numberOfBallotsTotal !== undefined) {
        progress = Math.round(props.statistics.numberOfBallotsCast / props.statistics.numberOfBallotsTotal * 100);
    }

    useErrorHandler(error);

    return (
        <>
            <Grid
                container
                spacing={2}
                justifyContent={"stretch"}
            >
                <Grid item xs={12} sm={12} md={12} lg={12} xl={12}>
                    <Card style={cardItemStyle}>
                        <CardHeader title={
                            <Typography variant="h5">{props.election.name}</Typography>
                        } avatar={
                            <Avatar>
                                <HowToVote/>
                            </Avatar>
                        }
                        >
                        </CardHeader>
                        <CardContent>
                            <Typography>{props.election.question}</Typography>
                        </CardContent>
                        <Divider/>
                        <CardActions>
                            <Button size={"medium"}
                                    onClick={() => calculateResult(props.election.id ?? "")}
                                    aria-label={"Calculate & Publish Results"}
                                    variant="contained"
                            ><PieChart/>&nbsp;Publish Results</Button>
                        </CardActions>
                    </Card>
                </Grid>
                <Grid item xs={12} sm={12} md={6} lg={6} xl={6}>
                    <Card style={cardItemStyle}>
                        <CardHeader title={
                            <Typography variant="h5">Number of Ballots</Typography>
                        } avatar={
                            <Avatar>
                                <HowToVote/>
                            </Avatar>
                        }
                        >
                        </CardHeader>
                        <CardContent>
                            <Grid container>
                                <Grid item xs={6}>
                                    <Typography
                                        variant={"h5"}>Tallied: {props.statistics.numberOfBallotsCast}</Typography>
                                </Grid>
                                <Grid item xs={6}>
                                    <Typography
                                        variant={"h5"}>Total: {props.statistics.numberOfBallotsTotal}</Typography>
                                </Grid>
                            </Grid>
                        </CardContent>
                    </Card>
                </Grid>
                <Grid item xs={12} sm={12} md={6} lg={6} xl={6}>
                    <Card style={cardItemStyle}>
                        <CardHeader title={
                            <Typography variant="h5">Tallying Progress</Typography>
                        } avatar={
                            <Avatar>
                                <Percent/>
                            </Avatar>
                        }
                        >
                        </CardHeader>
                        <CardContent>
                            {/* Inspired by https://mui.com/material-ui/react-progress/*/}
                            <Box style={{display: "flex", alignItems: "center"}}>
                                <Box style={{width: "100%"}}>
                                    <LinearProgress variant="determinate" value={progress}/>
                                </Box>
                                <Box style={{display: "flex", justifyContent: "center", minWidth: "70px"}}>
                                    <Typography variant={"h5"}>{progress}%</Typography>
                                </Box>
                            </Box>
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
            <ProgressWithLabel isLoading={isLoading} label="Publishing election results"/>
        </>
    )
        ;

};
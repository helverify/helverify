import {useEffect, useState} from "react";
import {Backdrop, CircularProgress, Container, Grid, SpeedDial, SpeedDialAction, Stack} from "@mui/material";
import {Ballot, MoreHoriz, PieChart, Print} from "@mui/icons-material";
import {useNavigate} from "react-router-dom";
import {apiClient} from "../../api/apiClient";
import {ElectionDto} from "../../api/Api";
import {ElectionInfo} from "./ElectionInfo";
import {ElectionResults} from "./ElectionResults";

export function Elections() {
    const [elections, setElections] = useState<ElectionDto[]>([]);

    const navigate = useNavigate();
    const [isLoading, setLoading] = useState<boolean>(false);

    useEffect(() => {
        apiClient().api.electionsList().then((result) => {
            setElections(result.data);
        });

    }, [])

    const calculateResult = (electionId: string) => {
        if (electionId === "") {
            return;
        }
        setLoading(true);
        apiClient().api.electionsTallyCreate(electionId).then((result) => {
            setLoading(false);
            console.log("results", result)
        });
    };

    return (
        <>
            <Backdrop open={isLoading}>
                <CircularProgress/>
            </Backdrop>
            <Container maxWidth={"xl"}>
                <Stack>
                    {elections.map((election, index) => {
                        return (
                            <Grid key={index} container spacing={1} sx={{m:1}}>
                                <Grid item xs={4}>
                                    <ElectionInfo election={election}/>
                                </Grid>
                                <Grid item xs={7}>
                                    <ElectionResults electionId={election.id ?? ""}/>
                                </Grid>
                                <Grid item xs={1}>
                                    <SpeedDial
                                        ariaLabel={"Election Actions"}
                                        icon={<MoreHoriz/>}
                                    >
                                        <SpeedDialAction
                                            icon={<Ballot/>}
                                            tooltipTitle={"Create Ballots"}
                                            onClick={() => navigate(`/elections/${election.id}/ballots/create`)}
                                        />
                                        <SpeedDialAction
                                            icon={<Print/>}
                                            tooltipTitle={"Print Ballots"}
                                            onClick={() => navigate(`/elections/${election.id}/ballots/print`)}
                                        />
                                        <SpeedDialAction
                                            icon={<PieChart/>}
                                            tooltipTitle={"Calculate & Publish Results"}
                                            onClick={() => calculateResult(election.id ?? "")}
                                        />
                                    </SpeedDial>

                                </Grid>
                            </Grid>
                        );
                    })}
                </Stack>
            </Container>

        </>

    );
}
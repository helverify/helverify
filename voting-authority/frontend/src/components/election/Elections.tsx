import {useEffect, useState} from "react";
import {
    Button,
    Card, CardActions, CardContent,
    CardMedia,
    Container,
    Stack
} from "@mui/material";
import {Ballot, PieChart, Print} from "@mui/icons-material";
import {useNavigate} from "react-router-dom";
import {apiClient} from "../../api/apiClient";
import {ElectionDto} from "../../api/Api";
import {ElectionInfo} from "./ElectionInfo";
import {ElectionResults} from "./ElectionResults";
import {ProgressWithLabel} from "../progress/ProgressWithLabel";
import {useErrorHandler} from "react-error-boundary";

export function Elections() {
    const [elections, setElections] = useState<ElectionDto[]>([]);

    const navigate = useNavigate();
    const [isLoading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>();
    useErrorHandler(error);

    useEffect(() => {
        apiClient().api.electionsList().then((result) => {
            if(result.status !== 200){
                setError(result.error);
            }
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
            if(result.status !== 200){
                setError(result.error);
            }
        });
    };

    return (
        <>
            <Container maxWidth={"md"}>
                <Stack>
                    {elections.map((election, index) => {
                        return (
                            <Card key={index} style={{marginTop: "18px"}}>
                                <CardMedia>
                                    <ElectionInfo election={election}/>
                                </CardMedia>
                                <CardContent>
                                    <ElectionResults electionId={election.id ?? ""} isLoading={isLoading} setError={setError}/>
                                </CardContent>
                                <CardActions>
                                    <Button size={"small"}
                                            onClick={() => navigate(`/elections/${election.id}/ballots/create`)}
                                            aria-label={"Create Ballots"}><Ballot/>&nbsp;Create Ballots</Button>
                                    <Button size={"small"}
                                            onClick={() => navigate(`/elections/${election.id}/ballots/print`)}
                                            aria-label={"Print Ballots"}><Print/>&nbsp;Print Ballots</Button>
                                    <Button size={"small"} onClick={() => calculateResult(election.id ?? "")}
                                            aria-label={"Calculate & Publish Results"}><PieChart/>&nbsp;Publish Results</Button>
                                </CardActions>
                            </Card>
                        );
                    })}
                </Stack>
            </Container>
            <ProgressWithLabel isLoading={isLoading} label="Publishing election results" />
        </>
    );
}
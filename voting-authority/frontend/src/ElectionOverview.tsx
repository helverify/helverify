import {useEffect, useState} from "react";
import {Api, ElectionDto} from "./Api";
import {SpeedDial, SpeedDialAction, Stack} from "@mui/material";
import {ElectionInfo} from "./components/setup/ElectionInfo";
import {Ballot, MoreHoriz, Print} from "@mui/icons-material";
import {useNavigate} from "react-router-dom";

export const ElectionOverview = () => {
    const [elections, setElections] = useState<ElectionDto[]>([]);

    const navigate = useNavigate();

    useEffect(() => {
        const client = new Api({
            baseUrl: "http://localhost:5000"
        });

        client.api.electionsList().then((result) => {
            setElections(result.data);
        });

    }, [])

    return(
        <Stack>
            {elections.map((election, index) => {
               return(
                   <Stack key={index} direction="row">
                        <ElectionInfo election={election}/>
                        <SpeedDial
                                ariaLabel={"Election Actions"}
                                icon={<MoreHoriz/>}
                        >
                            <SpeedDialAction
                                icon={<Ballot/>}
                                tooltipTitle={"Create Ballots"}
                                onClick={() => navigate(`/elections/${election.id}/create`)}
                            />
                            <SpeedDialAction
                                icon={<Print/>}
                                tooltipTitle={"Print Ballots"}
                                onClick={() => navigate(`/elections/${election.id}/print`)}
                            />
                        </SpeedDial>


                   </Stack>
               );
            })}
        </Stack>
    );
}
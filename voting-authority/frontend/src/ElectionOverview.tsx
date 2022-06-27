import {useEffect, useState} from "react";
import {ElectionDto} from "./Api";
import {SpeedDial, SpeedDialAction, Stack} from "@mui/material";
import {ElectionInfo} from "./components/setup/ElectionInfo";
import {Ballot, MoreHoriz, Print} from "@mui/icons-material";
import {useNavigate} from "react-router-dom";
import {apiClient} from "./api/apiClient";

export const ElectionOverview = () => {
    const [elections, setElections] = useState<ElectionDto[]>([]);

    const navigate = useNavigate();

    useEffect(() => {
        apiClient().api.electionsList().then((result) => {
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
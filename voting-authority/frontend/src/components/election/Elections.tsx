import {useEffect, useState} from "react";
import {SpeedDial, SpeedDialAction, Stack} from "@mui/material";
import {Ballot, MoreHoriz, Print} from "@mui/icons-material";
import {useNavigate} from "react-router-dom";
import {apiClient} from "../../api/apiClient";
import {ElectionDto} from "../../api/Api";
import {ElectionInfo} from "./ElectionInfo";


export function Elections() {
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
                                onClick={() => navigate(`/elections/${election.id}/ballots/create`)}
                            />
                            <SpeedDialAction
                                icon={<Print/>}
                                tooltipTitle={"Print Ballots"}
                                onClick={() => navigate(`/elections/${election.id}/ballots/print`)}
                            />
                        </SpeedDial>


                    </Stack>
                );
            })}
        </Stack>
    );
}
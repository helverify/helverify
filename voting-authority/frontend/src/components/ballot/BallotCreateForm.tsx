import {apiClient} from "../../api/apiClient";
import {useState} from "react";
import {BallotGenerationDto} from "../../api/Api";
import {useNavigate, useParams} from "react-router-dom";
import {BallotForm} from "./BallotForm";

export const BallotCreateForm = () => {
    const [isLoading, setLoading] = useState(false);

    const navigate = useNavigate();

    const {electionId} = useParams();


    const createBallots = (numberOfBallots: number) => {
        setLoading(true);

        if (electionId === undefined || electionId === null) {
            navigate("/elections");
        }

        const id: string = electionId ?? "";

        const ballotGenerationData: BallotGenerationDto = {
            numberOfBallots: numberOfBallots
        };

        apiClient().api.electionsBallotsCreate(id, ballotGenerationData).then(() => {
            setLoading(false);
            navigate("/elections");
        });
    };


    return (
        <BallotForm buttonCaption="Create Ballots" buttonAction={createBallots} isLoading={isLoading}/>
    );
}
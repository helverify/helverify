import {useState} from "react";
import {useNavigate, useParams} from "react-router-dom";
import {BallotForm} from "./BallotForm";

export const BallotPrintForm = () => {
    const [isLoading, setLoading] = useState(false);

    const navigate = useNavigate();

    const {electionId} = useParams();

    const printBallots = (numberOfBallots: number) => {
        if (electionId === undefined || electionId === null || numberOfBallots === 0) {
            navigate("/elections");
            return;
        }

        setLoading(true);

        const url: string = `${process.env.REACT_APP_VA_BACKEND}/api/elections/${electionId}/ballots/pdf/all?numberOfBallots=${numberOfBallots}`;

        // inspired by https://stackoverflow.com/questions/66811401/material-ui-how-to-download-a-file-when-clicking-a-button
        const downloadLink = document.createElement("a");

        downloadLink.href = url;
        downloadLink.target = "about:tab";

        try{
            downloadLink.click();
        } finally {
            setLoading(false);
            navigate("/elections");
        }
    };


    return (
        <>
            <BallotForm buttonCaption="Print Ballots" buttonAction={printBallots} isLoading={isLoading}/>
        </>

    );
}
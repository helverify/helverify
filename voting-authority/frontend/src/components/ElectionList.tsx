import React, {useEffect, useState} from "react";
import {Api, Election, ElectionDto} from "../Api";
import {DataGrid, GridColDef} from "@mui/x-data-grid";

export function ElectionList() {

    const [elections, setElections] = useState<ElectionDto[]>([]);

    useEffect(() => {
        const client = new Api({
            baseUrl: "http://localhost:5000"
        });

        client.api.electionsList().then((result) => {
            setElections(result.data);
        });

    }, [])

    const showElections = () => {
        if(elections.length > 0){
            const columns: GridColDef[] = [
                {
                    field: "id",
                    headerName: "ID",
                    width: 220
                },
                {
                    field: "name",
                    headerName: "Name",
                    width: 300
                },
                {
                    field: "question",
                    headerName: "Question",
                    width: 300
                },
                {
                    field: "contractAddress",
                    headerName: "Contract Address",
                    width: 400
                },
            ]

            return (
                <DataGrid columns={columns} rows={elections} autoHeight/>
            )
        }
    }

    return (
        <div>
            {showElections()}
        </div>
    );
}
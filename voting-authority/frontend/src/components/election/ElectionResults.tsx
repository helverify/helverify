import {useEffect, useState} from "react";
import {apiClient} from "../../api/apiClient";
import {ElectionResultDto, ElectionResultsDto} from "../../api/Api";
import {BarChart, PieChart, Cell, Pie, Bar, XAxis, YAxis} from "recharts";
import randomColor from "randomcolor";
import {Box, Tab, Tabs, Typography} from "@mui/material";
import {BarChartOutlined, PieChartOutlined, TableRows} from "@mui/icons-material";
import {DataGrid, GridColDef} from "@mui/x-data-grid";

type ElectionResultsProps = {
    electionId: string
}

export const ElectionResults = (props: ElectionResultsProps) => {

    const [currentTab, setCurrentTab] = useState<number>(0);
    const [colors, setColors] = useState<string[]>([]);
    const [electionResults, setElectionResults] = useState<ElectionResultsDto>({results: []});

    useEffect(() => {
        apiClient().api.electionsResultsDetail(props.electionId).then((result) => {
            setElectionResults(result.data);

            let optionColors: string[] = [];
            result.data.results?.forEach(() => {
                optionColors.push(randomColor())
            });

            setColors(optionColors);
        });
    }, [props.electionId]);

    const results = electionResults.results !== undefined &&
    electionResults.results !== null &&
    electionResults.results.length > 0 ? electionResults.results : [];

    // according to https://github.com/recharts/recharts/issues/1743
    const getLabel = (entry: ElectionResultDto) => {
        return `${entry.optionName} (${entry.count})`;
    };

    const handleTabChange = (event: any, value: number) => {
        setCurrentTab(value);
    }

    const dataGridColumns: GridColDef[] = [
        {field: "optionName", headerName: "Candidate / Option", width: 500},
        {field: "count", headerName: "Number of Votes", width: 300, align: "right"}
    ]

    return (
        <>
            {results.length > 0 && (
                <>
                    <Box>
                        <Typography variant={"h4"}>Results</Typography>
                    </Box>
                    <Box display="flex" justifyContent={"right"} style={{marginBottom: "30px"}}>
                        <Tabs value={currentTab} onChange={handleTabChange}>
                            <Tab icon={<PieChartOutlined/>} value={0}/>
                            <Tab icon={<BarChartOutlined/>} value={1}/>
                            <Tab icon={<TableRows/>} value={2}/>
                        </Tabs>
                    </Box>
                    <Box>
                        {currentTab === 0 && (
                            <PieChart
                                width={800}
                                height={600}
                            >
                                <Pie
                                    data={results}
                                    innerRadius={100}
                                    outerRadius={150}
                                    dataKey="count"
                                    nameKey="optionName"
                                    width={350}
                                    height={250}
                                    paddingAngle={3}
                                    label={getLabel}
                                >
                                    {results.map((result, index) => {
                                        return (<Cell key={`piechart-${index}`} fill={colors[index]}/>)
                                    })}
                                </Pie>
                            </PieChart>
                        )}
                        {currentTab === 1 && (

                            <BarChart
                                data={results}
                                width={800}
                                height={600}
                            >
                                <XAxis
                                    dataKey={"optionName"}
                                    allowDecimals={false}
                                />
                                <YAxis
                                    scale={"linear"}
                                    allowDecimals={false}
                                    interval={1}
                                />
                                <Bar dataKey="count">
                                    {results.map((result, index) => {
                                        return (<Cell key={`barchart-${index}`} fill={colors[index]}/>)
                                    })}
                                </Bar>
                            </BarChart>
                        )}
                        {currentTab === 2 && (
                            <DataGrid
                                autoHeight={true}
                                rows={results}
                                columns={dataGridColumns}
                                getRowId={(row: ElectionResultDto) => results.indexOf(row)}
                                hideFooterSelectedRowCount={true}
                                hideFooterPagination={true}
                                hideFooter={true}
                            />
                        )}
                    </Box>
                </>
            )}
        </>
    );
}
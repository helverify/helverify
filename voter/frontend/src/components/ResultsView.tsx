import {ElectionResults, OptionTally} from "../election/election";
import {useEffect, useState} from "react";
import {BarChart, Cell, PieChart, Pie, Bar, XAxis, YAxis, ResponsiveContainer, Legend, Tooltip} from "recharts";
import randomColor from "randomcolor";
import {BarChartOutlined, PieChartOutlined, TableRows} from "@mui/icons-material";
import {DataGrid, GridColDef} from "@mui/x-data-grid";
import {Box, Card, Tab, Tabs, Typography} from "@mui/material";

export type ResultsViewProps = {
    electionResults: ElectionResults;
}

export const ResultsView = (props: ResultsViewProps) => {

    const [currentTab, setCurrentTab] = useState<number>(0);
    const [colors, setColors] = useState<string[]>([]);
    const [electionResults, setElectionResults] = useState<ElectionResults>({results: []});

    useEffect(() => {
        setElectionResults(props.electionResults);

        let optionColors: string[] = [];
        props.electionResults.results?.forEach(() => {
            optionColors.push(randomColor())
        });

        setColors(optionColors);
    }, [props, props.electionResults]);

    const results: OptionTally[] = electionResults.results !== undefined &&
    electionResults.results !== null &&
    electionResults.results.length > 0 ? electionResults.results : [];

    const handleTabChange = (event: any, value: number) => {
        setCurrentTab(value);
    }

    const dataGridColumns: GridColDef[] = [
        {field: "name", headerName: "Candidate / Option", flex: 1},
        {field: "count", headerName: "Number of Votes", align: "right", flex: 1}
    ]

    return (
        <Card>
            <Box sx={{m: 2}}>
                    {results.length === 0 && (
                        <>
                            <Typography color={"text.secondary"}>Loading Results...</Typography>
                        </>
                    )}
                    {results.length > 0 && (
                        <>
                            <Box display="flex" justifyContent={"right"} style={{marginBottom: "30px"}}>
                                <Tabs value={currentTab} onChange={handleTabChange}>
                                    <Tab icon={<PieChartOutlined/>} value={0}/>
                                    <Tab icon={<BarChartOutlined/>} value={1}/>
                                    <Tab icon={<TableRows/>} value={2}/>
                                </Tabs>
                            </Box>
                            <Box>
                                {currentTab === 0 && (
                                    <ResponsiveContainer height={400}>
                                        <PieChart>
                                            <Tooltip/>
                                            <Legend formatter={(value) => (<Typography color={"#000000"} display={"inline"}>{value}</Typography>)}/>
                                            <Pie
                                                data={results}
                                                innerRadius={"70%"}
                                                outerRadius={"90%"}
                                                dataKey="count"
                                                nameKey="name"
                                                paddingAngle={3}
                                                label
                                            >
                                                {results.map((result, index) => {
                                                    return (
                                                        <Cell key={`piechart-${index}`} fill={colors[index]}/>
                                                    )
                                                })}
                                            </Pie>
                                        </PieChart>
                                    </ResponsiveContainer>
                                )}
                                {currentTab === 1 && (
                                    <ResponsiveContainer height={400}>
                                        <BarChart
                                            data={results}
                                            width={600}
                                        >
                                            <Tooltip/>
                                            <XAxis
                                                dataKey={"name"}
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
                                    </ResponsiveContainer>
                                )}
                                {currentTab === 2 && (
                                    <DataGrid
                                        autoHeight={true}
                                        rows={results}
                                        columns={dataGridColumns}
                                        getRowId={(row: OptionTally) => results.indexOf(row)}
                                        hideFooterSelectedRowCount={true}
                                        hideFooterPagination={true}
                                        hideFooter={true}
                                    />
                                )}
                            </Box>
                        </>
                    )}
            </Box>
        </Card>
    );
}
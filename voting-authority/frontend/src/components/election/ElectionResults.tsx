import {useEffect, useState} from "react";
import {apiClient} from "../../api/apiClient";
import {ElectionResultDto, ElectionResultsDto} from "../../api/Api";
import {BarChart, PieChart, Cell, Pie, Bar, XAxis, YAxis, ResponsiveContainer, Legend, Tooltip} from "recharts";
import randomColor from "randomcolor";
import {Box, Tab, Tabs, Typography} from "@mui/material";
import {BarChartOutlined, PieChartOutlined, TableRows} from "@mui/icons-material";
import {DataGrid, GridColDef} from "@mui/x-data-grid";

type ElectionResultsProps = {
    electionId: string,
    isLoading: boolean,
    setError: (error: string | undefined) => void;
}

export const ElectionResults = (props: ElectionResultsProps) => {

    const [currentTab, setCurrentTab] = useState<number>(0);
    const [colors, setColors] = useState<string[]>([]);
    const [electionResults, setElectionResults] = useState<ElectionResultsDto>({results: []});

    useEffect(() => {
            if (props.isLoading) {
                return;
            }
            apiClient().api.electionsResultsDetail(props.electionId).then((result) => {
                if (!!result.error) {
                    props.setError(result.error);
                    return;
                }

                setElectionResults(result.data);

                let optionColors: string[] = [];
                result.data.results?.forEach(() => {
                    optionColors.push(randomColor())
                });

                setColors(optionColors);
            });
        }, [props, props.electionId, props.isLoading]
    );

    const results = electionResults.results !== undefined &&
    electionResults.results !== null &&
    electionResults.results.length > 0 ? electionResults.results : [];

    // according to https://github.com/recharts/recharts/issues/1743
    const getLabel = (entry: ElectionResultDto) => {
        return `${entry.count}`;
    };

    const handleTabChange = (event: any, value: number) => {
        setCurrentTab(value);
    }

    const dataGridColumns: GridColDef[] = [
        {field: "optionName", headerName: "Candidate / Option", flex: 1},
        {field: "count", headerName: "Number of Votes", align: "right", flex: 1}
    ]

    return (
        <>
            {results.length === 0 && (
                <>
                    <Typography>There are no results available for this election yet.</Typography>
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
                                        outerRadius={"95%"}
                                        dataKey="count"
                                        nameKey="optionName"
                                        paddingAngle={3}
                                        label={getLabel}
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
                            </ResponsiveContainer>
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
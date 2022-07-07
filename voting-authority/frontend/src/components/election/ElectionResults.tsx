import {useEffect, useState} from "react";
import {apiClient} from "../../api/apiClient";
import {ElectionResultDto, ElectionResultsDto} from "../../api/Api";
import {PieChart, Cell, Pie} from "recharts";
import randomColor from "randomcolor";
import {Box, Card, CardContent} from "@mui/material";

type ElectionResultsProps = {
    electionId: string
}

export const ElectionResults = (props: ElectionResultsProps) => {

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

    const resultsChart = (
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
                    return (<Cell key={`result-${index}`} fill={colors[index]}/>)
                })}
            </Pie>
        </PieChart>
    );

    return (
        <>
            {results.length > 0 && (<Box>
                <Card variant="elevation">
                    <CardContent>
                        {resultsChart}
                    </CardContent>
                </Card>
            </Box>)}
        </>
    );
}
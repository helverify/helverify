import {ElectionDto, ElectionOptionDto} from "../../api/Api";
import {
    Avatar,
    Box,
    Card,
    CardContent,
    CardHeader,
    Chip,
    Grid, Modal, SpeedDial, SpeedDialAction,
    TextField,
    Tooltip,
    Typography
} from "@mui/material";

import {Ballot, Functions, HowToVote, Key, Link, MoreHoriz, PieChart, Print} from "@mui/icons-material";
import {ClipboardCopy} from "../../ClipboardCopy";
import {ElectionResults} from "./ElectionResults";
import {apiClient} from "../../api/apiClient";
import {useState} from "react";
import {useErrorHandler} from "react-error-boundary";
import {ProgressWithLabel} from "../progress/ProgressWithLabel";
import {BallotCreateForm} from "../ballot/BallotCreateForm";
import {BallotPrintForm} from "../ballot/BallotPrintForm";

export type ElectionGridProps = {
    election: ElectionDto;
}

export const ElectionGrid = (props: ElectionGridProps) => {
    const gridItemStyle = {};
    const cardItemStyle = {minHeight: "140px"};

    const areOptionsSet = (property: ElectionOptionDto[] | null | undefined) => {
        return !(property === undefined || property === null || property.length === 0);
    }

    const [isLoading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>();
    const [ballotCreateOpen, setBallotCreateOpen] = useState<boolean>(false);
    const [ballotPrintOpen, setBallotPrintOpen] = useState<boolean>(false);

    const openBallotCreateForm = () => {
        setBallotCreateOpen(true);
    }

    const openBallotPrintForm = () => {
        setBallotPrintOpen(true);
    }

    const closeBallotCreateForm = () => {
        setBallotCreateOpen(false);
    }

    const closeBallotPrintForm = () => {
        setBallotPrintOpen(false);
    }

    useErrorHandler(error);

    const options = areOptionsSet(props.election.options) ? props.election.options : [];

    const calculateResult = (electionId: string) => {
        if (electionId === "") {
            return;
        }
        setLoading(true);
        apiClient().api.electionsTallyCreate(electionId).then((result) => {
            setLoading(false);
            if (result.status !== 200) {
                setError(result.error);
            }
        });
    };

    return (
        <>
            <Grid
                container
                spacing={2}
                justifyContent="stretch"
            >
                <Grid item xs={12} sm={12} md={12} lg={12} xl={12} style={gridItemStyle}>
                    <Card style={cardItemStyle}>
                        <CardHeader title={
                            <Typography variant="h5">{props.election.name}</Typography>
                        } avatar={
                            <Avatar>
                                <HowToVote/>
                            </Avatar>
                        }
                        >
                        </CardHeader>
                        <CardContent>
                            <Typography>{props.election.question}</Typography>
                        </CardContent>
                    </Card>
                </Grid>

                <Grid item style={gridItemStyle} xs={12} sm={12} md={12} lg={6} xl={6}>
                    <Card style={cardItemStyle}>
                        <CardHeader title={
                            <Typography variant="h5">Candidates / Options</Typography>
                        } avatar={
                            <Avatar/>
                        }
                        >
                        </CardHeader>
                        <CardContent>
                            {options && (
                                <Grid
                                    container
                                    spacing={1}
                                >
                                    {options.map((option, index) => {
                                        const name: string = option.name ?? "";

                                        return (
                                            <Grid item key={index} xs={6} sm={4} md={4} lg={3} xl={3}>
                                                {option.name !== undefined && option.name !== null && (
                                                    <Tooltip
                                                        title={option.name}
                                                        arrow
                                                    >
                                                        <Chip label={name}/>
                                                    </Tooltip>
                                                )}
                                            </Grid>
                                        )
                                    })}
                                </Grid>

                            )}
                        </CardContent>
                    </Card>
                </Grid>

                <Grid item xs={12} sm={12} md={6} lg={3} xl={3} style={gridItemStyle}>
                    <Card style={cardItemStyle}>
                        <CardHeader title={
                            <Typography variant="h5">Public Key</Typography>
                        } avatar={
                            <Avatar>
                                <Key/>
                            </Avatar>
                        }
                        >
                        </CardHeader>
                        <CardContent>
                            <Box display="flex" justifyContent="space-between">
                                <TextField
                                    fullWidth
                                    variant="standard"
                                    value={props.election.publicKey}
                                    disabled/>
                                <ClipboardCopy message={"Copied public key to clipboard."}
                                               value={props.election.publicKey ?? ""}/>
                            </Box>
                        </CardContent>
                    </Card>
                </Grid>


                <Grid item xs={12} sm={12} md={6} lg={3} xl={3} style={gridItemStyle}>
                    <Card style={cardItemStyle}>
                        <CardHeader title={
                            <Typography variant="h5">Contract Address</Typography>
                        } avatar={
                            <Avatar>
                                <Link/>
                            </Avatar>
                        }
                        >
                        </CardHeader>
                        <CardContent>
                            <Box display="flex" justifyContent="space-between">
                                <TextField
                                    fullWidth
                                    variant="standard"
                                    value={props.election.contractAddress}
                                    disabled/>
                                <ClipboardCopy message={"Copied contract address to clipboard."} value={props.election.contractAddress ?? ""}/>
                            </Box>
                        </CardContent>
                    </Card>
                </Grid>



                <Grid item xs={12} sm={12} md={12} lg={12} xl={12} style={gridItemStyle}>
                    <Card style={cardItemStyle}>
                        <CardHeader title={
                            <Typography variant="h5">Results</Typography>
                        } avatar={
                            <Avatar>
                                <Functions/>
                            </Avatar>
                        }
                        >
                        </CardHeader>
                        <CardContent>
                            <Box >
                                <ElectionResults electionId={props.election.id ?? ""} isLoading={isLoading}
                                                              setError={setError}/>
                            </Box>
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
            <SpeedDial
                ariaLabel={"Election Actions"}
                icon={<MoreHoriz/>}
                sx={{ position: "fixed", right: "20px", bottom: "60px"}}
            >
                <SpeedDialAction
                    icon={<Ballot/>}
                    tooltipTitle={"Create Ballots"}
                    onClick={openBallotCreateForm}
                />
                <SpeedDialAction
                    icon={<Print/>}
                    tooltipTitle={"Print Ballots"}
                    onClick={openBallotPrintForm}
                />
                <SpeedDialAction
                    icon={<PieChart/>}
                    tooltipTitle={"Calculate & Publish Results"}
                    onClick={() => calculateResult(props.election.id ?? "")}
                />
            </SpeedDial>
            <ProgressWithLabel isLoading={isLoading} label="Publishing election results"/>
            <Modal open={ballotCreateOpen} onClose={closeBallotCreateForm}>
                <>
                    <BallotCreateForm electionId={props.election.id ?? ""} close={closeBallotCreateForm}/>
                </>
            </Modal>
            <Modal open={ballotPrintOpen} onClose={closeBallotPrintForm}>
                <>
                    <BallotPrintForm electionId={props.election.id ?? ""} close={closeBallotPrintForm}/>
                </>
            </Modal>
        </>
    );
}
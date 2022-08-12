import {ValidationStepProps} from "./validationStep";
import {Avatar, Box, Card, CardContent, CardHeader, Grid, Stack, Tooltip, Typography} from "@mui/material";
import {ValidityIcon} from "../ValidityIcon";
import {QuestionMark} from "@mui/icons-material";

export type SummaryStepProps = ValidationStepProps & {
    isCaI: boolean,
    isRaC: boolean,
    isTaR: boolean
}

export const SummaryStep = (props: SummaryStepProps) => {
    const validityIconSize: number = 50;

    return (
        <>
            <Box>
                <Stack direction="column" spacing={1}>
                    <Typography variant={"h5"}>Verification Complete</Typography>
                    <Typography>That's it, you have finished the verification for this election!</Typography>
                    <Grid container spacing={1}>
                        <Grid item xs={12} sm={12} md={6} lg={4} xl={4}>
                            <Card>
                                <CardHeader
                                    title={"Cast-as-Intended"}
                                />
                                <CardContent>
                                    <Box display={"flex"} alignItems={"center"}>
                                        <ValidityIcon isValid={props.isCaI} size={validityIconSize}/>
                                        {props.isCaI && (
                                            <Typography style={{marginLeft: "5px"}}>Your ballot has been
                                                cast-as-intended.</Typography>
                                        )}
                                        {!props.isCaI && (
                                            <Typography style={{marginLeft: "5px"}}>Your ballot has not been
                                                cast-as-intended or you have not verified this aspect when casting the
                                                ballot.</Typography>
                                        )}
                                    </Box>
                                </CardContent>
                            </Card>
                        </Grid>
                        <Grid item xs={12} sm={12} md={6} lg={4} xl={4}>
                            <Card>
                                <CardHeader
                                    title={"Recorded-as-Cast"}
                                />
                                <CardContent>
                                    <Box display={"flex"} alignItems={"center"}>
                                        <ValidityIcon isValid={props.isRaC} size={validityIconSize}/>
                                        {props.isRaC && (
                                            <Typography style={{marginLeft: "5px"}}>Your ballot has been
                                                recorded-as-cast.</Typography>
                                        )}
                                        {!props.isRaC && (
                                            <Typography style={{marginLeft: "5px"}}>Your ballot has not been
                                                recorded-as-cast, because your selection varies from the published
                                                selections.</Typography>
                                        )}
                                    </Box>
                                </CardContent>
                            </Card>
                        </Grid>
                        <Grid item xs={12} sm={12} md={6} lg={4} xl={4}>
                            <Card>
                                <CardHeader
                                    title={"Tallied-as-Recorded"}
                                    action={<Tooltip
                                        title={"Complete tally verification requires separate inspections of election evidence."}>
                                        <Avatar style={{width: "30px", height: "30px"}}><QuestionMark/></Avatar>
                                    </Tooltip>}
                                />
                                <CardContent>
                                    <Box display={"flex"} alignItems={"center"}>
                                        <ValidityIcon isValid={props.isTaR} size={validityIconSize}/>
                                        {props.isTaR && (
                                            <Typography style={{marginLeft: "5px"}}>The results have been decrypted
                                                correctly.</Typography>
                                        )}
                                        {!props.isTaR && (
                                            <Typography style={{marginLeft: "5px"}}>The results have not been decrypted
                                                correctly.</Typography>
                                        )}
                                    </Box>
                                </CardContent>
                            </Card>
                        </Grid>
                    </Grid>
                </Stack>
            </Box>
        </>
    );
}
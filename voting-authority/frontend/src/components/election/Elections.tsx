import {useEffect, useState} from "react";
import {
    Box,
    Container,
    Divider,
    Drawer,
    IconButton,
    List,
    ListItem,
    ListItemButton,
    ListItemIcon,
    ListItemText,
} from "@mui/material";
import {ChevronLeft, HowToVote} from "@mui/icons-material";
import {apiClient} from "../../api/apiClient";
import {ElectionDto} from "../../api/Api";

import {ElectionGrid} from "./ElectionGrid";
import {useErrorHandler} from "react-error-boundary";

export type ElectionsProps = {
    menuOpen: boolean;
    toggleMenu: () => void;
    closeMenu: () => void;
}

export function Elections(props: ElectionsProps) {
    const widthOfDrawer = 300;

    const [elections, setElections] = useState<ElectionDto[]>([]);

    const [error, setError] = useState<string>();
    useErrorHandler(error);

    const [selectedElection, setSelectedElection] = useState<ElectionDto>();

    useEffect(() => {
        apiClient().api.electionsList().then((result) => {
            if (result.status !== 200) {
                setError(result.error);
            }
            setElections(result.data);

            if (selectedElection === undefined) {
                setSelectedElection(result.data[0] ?? undefined);
            }
        });
    }, [])


    const contentDivStyle = {transform: "none", transition: "margin-left 225ms cubic-bezier(0, 0, 0.2, 1) 0ms"};


    const electionContent = props.menuOpen ? {
        marginLeft: widthOfDrawer,
        ...contentDivStyle
    } : contentDivStyle;

    return (
        <>
            <Drawer open={props.menuOpen} sx={{width: widthOfDrawer}} variant="persistent">
                <Box justifyContent="right" alignItems="right" display="flex" style={{padding: "10px"}}>
                    <IconButton onClick={props.toggleMenu} style={{marginTop: "60px"}}>
                        <ChevronLeft/>
                    </IconButton>
                </Box>
                <Divider/>
                <List>
                    {elections.map((election, index) => {
                        return (
                            <ListItem key={index}>
                                <ListItemButton onClick={() => setSelectedElection(election)}>
                                    <ListItemIcon>
                                        <HowToVote/>
                                    </ListItemIcon>
                                    <ListItemText primary={election.name}/>
                                </ListItemButton>
                            </ListItem>
                        );
                    })}
                </List>
            </Drawer>
            <div style={electionContent}>
                <Box style={{flexGrow: 1, width: "100%"}}>
                    {selectedElection !== undefined && (
                        <ElectionGrid
                            election={selectedElection}
                        />
                    )}
                </Box>
            </div>

        </>
    );
}
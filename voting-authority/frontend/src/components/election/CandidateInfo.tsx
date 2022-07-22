import {Avatar, Stack, Typography} from "@mui/material";

export type CandidateInfoProps = {
    name: string;
}

export const CandidateInfo = (props: CandidateInfoProps) => {
    return(
        <Stack direction="row">
            <Avatar></Avatar>
            <Typography color="text.secondary" style={{marginLeft:"10px", marginRight:"10px", marginTop: "8px"}}>{props.name}</Typography>
        </Stack>
    );
}
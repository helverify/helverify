import {Backdrop, CircularProgress, Stack, Typography} from "@mui/material";

export type ProgressWithLabelProps = {
    isLoading: boolean,
    label: string
}

export const ProgressWithLabel = (props: ProgressWithLabelProps) => {
    return(
        <>
            <Backdrop open={props.isLoading} style={{zIndex: 99}}>
                <Stack direction="row" spacing={1}>
                    <Typography variant="overline" style={{ marginTop: "4px"}}>{props.label} ...</Typography>
                    <CircularProgress />
                </Stack>
            </Backdrop>
        </>
    );
}
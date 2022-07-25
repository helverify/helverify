import {Alert, Snackbar} from "@mui/material";
import {FallbackProps} from "react-error-boundary";

export const ErrorHandler = (props: FallbackProps) => {
    return (
        <Snackbar
            open={props.error !== undefined}
            onClose={() => props.resetErrorBoundary()}
            autoHideDuration={5000}
        >
            <Alert severity="error" variant="outlined">{props.error}</Alert>
        </Snackbar>
    );
}
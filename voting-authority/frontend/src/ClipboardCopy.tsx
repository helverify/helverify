import {Alert, Button, Snackbar, Tooltip} from "@mui/material";
import {ContentCopy} from "@mui/icons-material";
import {useState} from "react";

export type ClipboardCopyProps = {
    message: string;
    value: string;
}

export const ClipboardCopy = (props: ClipboardCopyProps) => {

    const [copyMessage, setCopyMessage] = useState<string>("");

    const copyToClipboard = () => {
        // https://www.w3schools.com/howto/howto_js_copy_clipboard.asp
        navigator.clipboard.writeText(props.value).then(() => {
            setCopyMessage(props.message);
        });
    }

    return (
        <>
            <Tooltip title={"Copy to clipboard"}>
                <Button variant="outlined" size="small" style={{marginLeft: "10px"}}
                        onClick={copyToClipboard}>
                    <ContentCopy/>
                </Button>
            </Tooltip>
            <Snackbar
                open={copyMessage !== ""}
                autoHideDuration={3000}
                onClose={() => {
                    setCopyMessage("")
                }}
            >
                <Alert severity="success" variant="outlined">{copyMessage}</Alert>
            </Snackbar>
        </>
    );
}
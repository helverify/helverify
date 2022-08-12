import {Check, Close} from "@mui/icons-material";
import {Avatar, Theme} from "@mui/material";
import React from "react";

export type ValidityIconProps = {
    isValid: boolean | undefined,
    size?: number | undefined
}

export const ValidityIcon = (props: ValidityIconProps) => {
    const size = `${props.size ?? 30}px`;
    const checkStyle = {color: "#00ac00", fontSize: size};
    const xmarkStyle = {color: "#ff2913"};

    const getThemeStyle = (theme: Theme) => {
        return theme.palette.mode === "dark"? darkStyle : lightStyle;
    }

    const darkStyle = {
        backgroundColor: "#3a3a3a",
    };

    const lightStyle = {
        backgroundColor: "#ffffff",
        borderStyle: "solid",
        borderWidth: "1px",
        borderColor: "#c9c9c9"
    }

    return (
        <>
            {props.isValid !== undefined && (
                <Avatar style={{width: size, height: size}} sx={getThemeStyle}>
                    {props.isValid ? <Check style={checkStyle}/> :
                        <Close style={xmarkStyle}/>}
                </Avatar>
            )}
        </>
    );
}
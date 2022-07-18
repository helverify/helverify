import {Check, Close} from "@mui/icons-material";
import {Avatar} from "@mui/material";
import React from "react";

export type ValidityIconProps = {
    isValid: boolean | undefined;
}

export const ValidityIcon = (props: ValidityIconProps) => {
    const checkStyle = {color: "#00FF00"};
    const xmarkStyle = {color: "#FF0000"};

    return (
        <>
            {props.isValid !== undefined && (
                <Avatar style={{backgroundColor: "#3a3a3a", width: "30px", height: "30px"}}>
                    {props.isValid ? <Check style={checkStyle}/> :
                        <Close style={xmarkStyle}/>}
                </Avatar>
            )}
        </>
    );
}
import * as React from "react"

export interface ValidateTextboxProps extends React.InputHTMLAttributes<HTMLInputElement> {
    valid: boolean
    invalidMessage: string
}



export default function ValidatorMessage({ valid, invalidMessage, ...props }: ValidateTextboxProps) {
    return <>
        <input className={valid ? "rz-textbox valid" : "rz-textbox"} {...props} />
        {!valid && <div className="rz-message rz-messages-error">{invalidMessage}</div>}
    </>
}
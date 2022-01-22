import * as React from "react"

export interface IconButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
    icon: string
    buttonStyle: "success" | "warning" | "danger"
}

export default function IconButton({ icon, className, buttonStyle, children, ...props }: IconButtonProps) {
    if (!className)
        className = ""
    className = `${className} rz-button rz-button-md btn-${buttonStyle}`

    return <button className={className} {...props}>
        <i className="rz-button-icon-left rzi">{icon}</i>
        <span className="rz-button-text">{children}</span>
    </button>
}
import * as React from "react"


interface DialogPropsExplicit {
    title: string
    onClose?(): void
}

type DialogProps = React.PropsWithChildren<DialogPropsExplicit>

const dialogStyle: React.CSSProperties = {
    minWidth: "150px",
    minHeight: "150px",
    zIndex: "1001",
    opacity: 1,
    position: "absolute",
    width: "600px",
}

export default function Dialog(props: DialogProps) {
    return <div className="rz-dialog-wrapper" style={{ left: "0" }}>
        <div className="rz-dialog" role="dialog" style={dialogStyle}>
            <div className="rz-dialog-titlebar">
                <span className="rz-dialog-title">{props.title}</span>
                <a className="rz-dialog-titlebar-icon rz-dialog-titlebar-close" onClick={props.onClose} role="button">
                    <span className="rzi rzi-times" />
                </a>
            </div>
            <div className="rz-dialog-content">
                {props.children}
            </div>
        </div>
        <div className="rz-dialog-mask" style={{ zIndex: 1000 }} />
    </div>
}
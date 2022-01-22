import * as React from "react"
import IconButton from "./IconButton"

export interface ForceStartStopViewProps {
    onForceStart?(): void
    onForceStop?(): void
    onResumeScheduled?(): void

}

export default function ForceStartStopView(props: ForceStartStopViewProps) {
    
    return <><h4>Manage</h4>
        <IconButton children="Start" onClick={props.onForceStart} icon="power" buttonStyle="success" type="button"/>
        {"  "}
        <IconButton children="Stop" onClick={props.onForceStop} icon="power_off" buttonStyle="warning" type="button"/>
        {"  "}
        <IconButton children="Geplanter Modus" onClick={props.onResumeScheduled} icon="check_circle" buttonStyle="danger" type="button"/>
    </>
}
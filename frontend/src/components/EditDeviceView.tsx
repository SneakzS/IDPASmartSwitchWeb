import * as React from "react"
import { Device } from "../device"
import ValidateTextbox from "./ValidateTextbox"
import validator from "../validator"
import ForceStartStopView from "./ForceStartStopView";

export interface EditDeviceViewProps {
    device: Device
    onSave?(updated: Device): void
    onForceStart?(): void
    onForceStop?(): void
    onResumeSchedule?(): void
}

interface EditDeviceDialogState {
    name: string
    description: string
}

export default function EditDeviceView({ device, ...props }: EditDeviceViewProps) {
    const [state, setState] = React.useState<EditDeviceDialogState>({
        name: device.name,
        description: device.description
    });

    const onFormSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        props.onSave && props.onSave({
            ...device,
            name: state.name,
            description: state.description,
        })
    }

    return <form onSubmit={onFormSubmit}>
        <div className="row" style={{ marginBottom: "16px" }}>
            <div className="col-md-3">
                <label>Name</label>
            </div>
            <div className="col">
                <ValidateTextbox name="Name" tabIndex={0} autoComplete="on" placeholder="Description"
                    value={state.name} onChange={e => { const name = e.target.value; setState(s => ({ ...s, name })) }}
                    valid={validator.stringRequired(state.name)}
                    invalidMessage="Title is required"
                />
            </div>
        </div>
        <div className="row" style={{ marginBottom: "16px" }}>
            <div className="col-md-3">
                <label>Description</label>
            </div>
            <div className="col">
                <ValidateTextbox name="Description" tabIndex={0} autoComplete="on" placeholder="Description"
                    value={state.description} onChange={e => { const description = e.target.value; setState(s => ({ ...s, description })) }}
                    valid={validator.stringRequired(state.description)}
                    invalidMessage="Description is required" />
            </div>
        </div>
        <div className="row">
            <div className="col-md-3"></div>
            <div className="col">
                <button className="rz-button rz-button-md btn-primary" type="submit">Save</button>
            </div>
        </div>
        <ForceStartStopView onForceStart={props.onForceStart}
            onForceStop={props.onForceStop}
            onResumeScheduled={props.onResumeSchedule}
        />
    </form >

}
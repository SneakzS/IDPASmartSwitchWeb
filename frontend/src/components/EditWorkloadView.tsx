import React from "react";
import validator from "../validator";
import { RepeatPattern, Workload, WorkloadInner } from "../workloads"
import NumberTextbox from "./NumberTextbox";
import RepeatPatternView from "./RepeatPatternView";
import ValidateTextbox from "./ValidateTextbox"

export interface EditWorkloadViewProps {
    workload: Workload
    onSave?(updated: Workload): void
    onDelete?(): void
    editable?: boolean
}

interface EditWorkloadViewState {
    workload: WorkloadInner
    repeatPattern: RepeatPattern[]
}

export default function EditWorkloadView(props: EditWorkloadViewProps) {
    const [state, setState] = React.useState<EditWorkloadViewState>({
        workload: props.workload.workload,
        repeatPattern: props.workload.repeatPattern
    })

    const onSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        props.onSave && props.onSave({ workload: state.workload, repeatPattern: state.repeatPattern })
    }

    const addRepeatPattern = () => {
        setState(s => ({
            ...s,
            repeatPattern: [...s.repeatPattern || [], {
                monthFlags: 0,
                dayFlags: 0,
                hourFlags: 0,
                minuteFlagsLow: 0,
                minuteFlagsHigh: 0,
                weekdayFlags: 0xFF
            }]
        }))
    }

    const onRepeatPatternChange = (updated: RepeatPattern, index: number) => {
        setState(s => ({
            ...s,
            repeatPattern: s.repeatPattern.map((rp, i) => i === index ? updated : rp)
        }))
    }

    const onRepeatPatternRemove = (index: number) => {
        setState(s => ({
            ...s,
            repeatPattern: s.repeatPattern.filter((rp, i) => i !== index)
        }))
    }

    const rnd = Math.random()

    return <form onSubmit={onSubmit} style={{ marginBottom: "33px", borderRadius: "4px", paddingLeft: "6px", paddingRight: "6px", paddingTop: "12px", paddingBottom: "12px", border: "1px solid lightgray" }}>
        <div className="container">
            <div className="row">
                <div className="col-md-8 container">
                    <div className="row" style={{ marginBottom: "16px" }}>
                        <div className="col-md-3">
                            <label>Description</label>
                        </div>
                        <div className="col">
                            <ValidateTextbox name="Description" tabIndex={0} autoComplete="on" placeholder="Description" className="form-control"
                                value={state.workload.description} onChange={e => { const description = e.target.value; setState(s => ({ ...s, workload: { ...s.workload, description } })) }}
                                valid={validator.stringRequired(state.workload.description)}
                                invalidMessage="Description is required" />
                        </div>
                    </div>
                    <div className="row" style={{ marginBottom: "16px" }}>
                        <div className="col-md-3">
                            <label>Workload (W)</label>
                        </div>
                        <div className="col">
                            <NumberTextbox name="Description" tabIndex={0} autoComplete="on" placeholder="Workload" className="form-control"
                                numberValue={state.workload.workloadW} onNumberChange={workloadW => setState(s => ({ ...s, workload: { ...s.workload, workloadW } }))}
                                valid={state.workload.workloadW > 0}
                                invalidMessage="Workload must be greater than 0"
                                numberType="integer" />
                        </div>
                    </div>
                    <div className="row" style={{ marginBottom: "16px" }}>
                        <div className="col-md-3">
                            <label>Duration (M)</label>
                        </div>
                        <div className="col">
                            <NumberTextbox name="Duration" tabIndex={0} autoComplete="on" placeholder="Duration" className="form-control"
                                numberValue={state.workload.durationM} onNumberChange={durationM => setState(s => ({ ...s, workload: { ...s.workload, durationM } }))}
                                valid={state.workload.durationM > 0}
                                invalidMessage="Duration must be greater than 0"
                                numberType="integer" />
                        </div>
                    </div>
                    <div className="row" style={{ marginBottom: "16px" }}>
                        <div className="col-md-3">
                            <label>Tolerance Duration (M)</label>
                        </div>
                        <div className="col">
                            <NumberTextbox name="Tolerance Duration" tabIndex={0} autoComplete="on" placeholder="Tolerance Duration" className="form-control"
                                numberValue={state.workload.toleranceDurationM} onNumberChange={toleranceDurationM => setState(s => ({ ...s, workload: { ...s.workload, toleranceDurationM } }))}
                                valid={state.workload.toleranceDurationM > state.workload.durationM}
                                invalidMessage="Tolerance Duration must be bigger than the duration"
                                numberType="integer" />
                        </div>
                    </div>
                </div>
                <div className="col-md-4">
                    {props.editable && <button className="btn btn-primary" type="submit" style={actionButtonStyle}>Save</button>}
                    {props.editable && <button className="btn btn-danger" type="button" style={actionButtonStyle} onClick={() => props.onDelete && props.onDelete()}>Delete</button>}
                    {props.editable && <button className="btn btn-secondary" onClick={addRepeatPattern} style={actionButtonStyle} type="button">Add repeat pattern</button>}

                    <div className="form-check" style={{ marginTop: "30px" }}>
                        <input className="form-check-input"
                            type="checkbox"
                            checked={state.workload.isEnabled}
                            id={rnd + "-input-all"}
                            onChange={e => { let isEnabled = e.target.checked; setState(s => ({ ...s, workload: { ...s.workload, isEnabled } })) }}
                        />
                        <label style={{ marginBottom: "0" }} htmlFor={rnd + "-input-all"}>Is Enabled</label>
                    </div>
                </div>
            </div>

        </div>

        {state.repeatPattern && state.repeatPattern.map(
            (rp, i) => <RepeatPatternView editable={props.editable} pattern={rp} key={i}
                onChange={updated => onRepeatPatternChange(updated, i)}
                onRemove={() => onRepeatPatternRemove(i)}
            />
        )}
    </form>
}

const actionButtonStyle: React.CSSProperties = {
    display: "block",
    margin: "3px",
    width: "100%",
}
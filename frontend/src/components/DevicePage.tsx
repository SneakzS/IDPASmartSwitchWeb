import * as React from "react"
import { Device, deviceStatusToString, getAllDevices } from "../device"
import moment from "moment"
import Dialog from "./Dialog"
import EditDeviceView from "./EditDeviceView"
import errors from "../errors"
import api from "../api"
import { Link } from "react-router-dom"

export interface DeviceListItemProps {
    device: Device
    onEdit?(): void
    onDelete?(): void
}

export function DeviceListItem({ device, onEdit, onDelete }: DeviceListItemProps) {
    return <div className="rz-card card m-3" style={{ float: "inline-start", width: "400px", height: "300px" }}>
        <div>
            {device.status == 1 && <div style={{ float: "right" }}>
                <Link to={"/events/" + device.guid} style={{ color: "black", display: "inline-block", marginRight: "6px" }}>Events</Link>
                <Link to={"/workload/" + device.guid} style={{ color: "black", display: "inline-block", marginRight: "6px" }}>Workloads</Link>
                <a href={`/api/v1/sensor/${device.guid}/csv`} style={{ color: "black", display: "inline-block", marginRight: "6px" }}>Sensor Data</a>
            </div>}
            <h3 className="h5">{device.name}</h3>
        </div>

        <div className="container">
            <div className="row">
                <div className="col-sm col-8">
                    <div>Name</div>
                    <b>{device.name}</b>
                    <div className="mt-3">Beschreibung</div>
                    <b>{device.description}</b>
                    <div className="mt-3">Status</div>
                    <b>{deviceStatusToString(device.status)}</b>
                    {device.status != 1 && <i><br/>last seen: {moment(device.lastOnline).fromNow()}</i>}
                </div>
                <div className="col-sm col-4">

                    <div style={{ marginTop: "6px" }}>
                        <button className="rz-button rz-button-md btn-primary" onClick={() => onEdit && onEdit()} style={actionButtonStyle}>Edit</button>
                    </div>
                    <div style={{ marginTop: "6px" }}>
                        <button className="rz-button rz-button-md btn-danger" onClick={() => onDelete && onDelete()} style={actionButtonStyle}>Delete</button>
                    </div>
                </div>

            </div>
        </div>
    </div>
}

const actionButtonStyle: React.CSSProperties = { display: "inline-block", width: "100%", lineHeight: "2.1875rem" }

export interface DevicePageProps {
    reloadHandler: [() => void]
}



interface DevicePageState {
    devices: Device[]
    editDialogVisible: boolean
    editDevice?: Device
}

export default function DevicePage(props: DevicePageProps) {
    const [state, setState] = React.useState<DevicePageState>({
        devices: [],
        editDialogVisible: false,
    })

    const reloadDevices = async () => {
        try {
            let devices = await getAllDevices();
            setState(s => ({ ...s, devices }))
        } catch (err) {
            errors.defaultHandler(err)
        }
    }

    // handle navbar reload click
    props.reloadHandler[0] = reloadDevices

    React.useEffect(() => {
        reloadDevices()
    }, [])

    const onEditDevice = (device: Device) => {
        setState(s => ({ ...s, editDevice: device }))
    }

    const onForceStart = () => {
        api.post("/devices/switch/" + state.editDevice!.guid, {
            position: "force-on",
        }).catch(errors.defaultHandler)
    }

    const onForceStop = () => {
        api.post("/devices/switch/" + state.editDevice!.guid, {
            position: "force-off",
        }).catch(errors.defaultHandler)
    }

    const onResumeSchedule = () => {
        api.post("/devices/switch/" + state.editDevice!.guid, {
            position: "resume-schedule",
        }).catch(errors.defaultHandler)
    }

    const onChangeDevice = async (device: Device) => {
        try {
            await api.post("/devices", device)
            let devices = await getAllDevices();
            setState(s => ({ ...s, devices, editDevice: undefined }))

        } catch (err) {
            errors.defaultHandler(err)
        }
    }

    const onDeleteDevice = async (dev: Device) => {
        try {
            await api.delete("/devices", dev)
            let devices = await getAllDevices();
            setState(s => ({ ...s, devices, editDevice: undefined }))
        } catch (err) {
            errors.defaultHandler(err)
        }
    }


    return <>
        <h3>DeviceList</h3>
        {state.devices.map(device =>
            <DeviceListItem device={device} onEdit={() => onEditDevice(device)} key={device.guid} onDelete={() => onDeleteDevice(device)} />
        )}

        {state.editDevice && <Dialog title="Edit Device"
            onClose={() => setState(s => ({ ...s, editDevice: undefined }))}>
            <EditDeviceView
                device={state.editDevice}
                onForceStart={onForceStart}
                onForceStop={onForceStop}
                onResumeSchedule={onResumeSchedule}
                onSave={onChangeDevice}
            />
        </Dialog>}
    </>
}
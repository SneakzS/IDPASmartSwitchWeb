import moment from "moment";
import React from "react";
import { useParams } from "react-router";
import api from "../api";
import errors from "../errors";
import { Workload } from "../workloads";
import EditWorkloadView from "./EditWorkloadView";

export interface WorkloadPageProps {
    reloadHandler: [() => void]
}

interface WorkloadPageState {
    workloads: Workload[]
}

export default function WorkloadPage(props: WorkloadPageProps) {
    const [state, setState] = React.useState<WorkloadPageState>({
        workloads: [],
    })

    const params = useParams()
    let clientGuid = params.clientGuid

    const reloadWorkloads = async () => {
        try {
            let resp = await api.get<Workload[]>("/workloads/" + clientGuid)
            let workloads = await resp.json()
            setState(s => ({ ...s, workloads }))
        } catch (err) {
            errors.defaultHandler(err)
        }

    }

    const onSaveWorkload = async (wl: Workload) => {
        try {
            await api.post("/workloads/" + clientGuid, wl)
            let resp = await api.get<Workload[]>("/workloads/" + clientGuid)
            let workloads = await resp.json()
            setState(s => ({ ...s, workloads }))

        } catch (err) {
            errors.defaultHandler(err)
        }
    }

    const onDeleteWorkload = async (wl: Workload) => {
        try {
            await api.delete("/workloads/" + clientGuid, wl)
            let resp = await api.get<Workload[]>("/workloads/" + clientGuid)
            let workloads = await resp.json()
            setState(s => ({ ...s, workloads }))
        } catch (err) {
            errors.defaultHandler(err)
        }
    }

    const onAddWorkload = () => {
        setState(s => ({
            ...s,
            workloads: [
                ...s.workloads,
                {
                    workload: {
                        description: "new workload",
                        durationM: 0,
                        expiryDate: moment(new Date()).add(1, "year").toDate(),
                        isEnabled: false,
                        toleranceDurationM: 0,
                        workloadDefinitionId: 0,
                        workloadId: 0,
                        workloadW: 0,
                    },
                    repeatPattern: [],
                }
            ]
        }))
    }

    React.useEffect(() => {
        reloadWorkloads()
    }, [])

    return <>
        <button type="button" className="btn btn-primary" onClick={onAddWorkload} style={{marginBottom: "30px"}}>Add Workload</button>
        {state.workloads && state.workloads.map((wl, i) => <EditWorkloadView workload={wl} key={i}
            editable={true}
            onSave={onSaveWorkload}
            onDelete={() => onDeleteWorkload(wl)}
        />)}</>
}
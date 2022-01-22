import * as React from "react"
import { Calendar, momentLocalizer, Event } from "react-big-calendar"
import moment from "moment"
import "react-big-calendar/lib/css/react-big-calendar.css"
import { useParams } from "react-router"
import api from "../api"
import { ActiveWorkload, Workload } from "../workloads"
import errors from "../errors"

export interface EventsPageProps {
    reloadHandler: [() => void]
}

interface EventsPageState {
    events: Event[]
}

export default function EventsPage(props: EventsPageProps) {
    let localizer = momentLocalizer(moment)

    const params = useParams()
    const clientGuid = params.clientGuid

    const [state, setState] = React.useState<EventsPageState>({
        events: [],
    })

    const getEvents = async () => {
        try {
            let resp = await api.get<ActiveWorkload[]>("/workloads/events/" + clientGuid)
            let events = await resp.json()

            let resp2 = await api.get<Workload[]>("/workloads/" + clientGuid)
            let workloads = await resp2.json()

            setState(s => ({
                ...s,
                events: (events || []).map(wlev => ({
                    title: workloads.find(wl => wl.workload.workloadDefinitionId === wlev.WorkloadDefinitionId)?.workload.description,
                    start: moment(wlev.startTime).add(wlev.offsetM, "minutes").toDate(),
                    end: moment(wlev.startTime).add(wlev.offsetM, "minutes").add(wlev.durationM, "minutes").toDate()
                }))
            }))
        } catch (err) {
            errors.defaultHandler(err)
        }
    }

    React.useEffect(() => {
        getEvents()
    }, [])


    props.reloadHandler[0] = getEvents

    return <>
        <Calendar
            localizer={localizer}
            events={state.events}
            startAccessor="start"
            endAccessor="end"
            style={{ height: 500 }}
        ></Calendar>
    </>
}
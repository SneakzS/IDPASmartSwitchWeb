import api from "./api"

export interface Device {
    id: number
    name: string
    description: string
    guid: string
    lastOnline: Date
    status: number

}

const deviceStates = [
    "Offline",
    "Online",
    "Unknown",
    "RunningScheduled",
    "RunningManual",
    "StoppedManual",
]

export function deviceStatusToString(status: number) {
    return deviceStates[status] || "Unknown"
}

export async function getAllDevices(): Promise<Device[]> {
    let apiResult = await api.get<Device[]>("/devices")
    let devices = await apiResult.json()
    return devices
}
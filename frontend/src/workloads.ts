export interface Workload {
    workload: WorkloadInner
    repeatPattern: RepeatPattern[]
}

export interface RepeatPattern {
    monthFlags: number
    dayFlags: number
    hourFlags: number
    minuteFlagsLow: number
    minuteFlagsHigh: number
    weekdayFlags: number
}

export interface WorkloadInner {
    workloadDefinitionId: number
    workloadId: number
    description: string
    workloadW: number
    durationM: number
    toleranceDurationM: number
    isEnabled: boolean
    expiryDate: Date
}

export interface ActiveWorkload {
    WorkloadDefinitionId: number
    offsetM: number
    startTime: Date
    durationM: number
    workload: number
}

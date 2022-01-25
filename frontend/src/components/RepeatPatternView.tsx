import { RepeatPattern } from "../workloads";
import styles from "./RepeatPatternView.module.css"

export interface RepeatPatternViewProps {
    editable?: boolean
    pattern: RepeatPattern
    onChange?(updated: RepeatPattern): void
    onRemove?(): void
}

const months = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
]

const flagsAllMonths = 0xFFFF

const weekdays = [
    "Sunday",
    "Monday",
    "Tuesday",
    "Wednesday",
    "Thursday",
    "Friday",
    "Saturday",
]

const flagsAllWeekdays = 0xFF

const allDays: Number[] = []
for (let i = 0; i < 31; i++)
    allDays.push(i + 1)

const flagsAllDays = 0xFFFFFFFF

const allHours: Number[] = []
for (let i = 0; i < 24; i++)
    allHours.push(i)

const flagsAllHours = 0xFFFFFFFF

const allMinutes: Number[] = []
for (let i = 0; i < 60; i++)
    allMinutes.push(i)

const flagsAllMinutesLow = 0xFFFFFFFF
const flagsAllMinutesHigh = 0xFFFFFFFF

interface SelectableItem {
    value: string
    selected: boolean
}

interface SelectableGroupProps {
    onSelectAll?(): void
    onSelectNone?(): void
    onSelectChange?(index: number, selected: boolean): void
    items?: SelectableItem[]
    label?: string
    flex?: boolean
}

function SelectableGroup<T>(props: SelectableGroupProps) {
    let allSelected = true
    props.items && props.items.forEach(item => {
        if (!item.selected)
            allSelected = false;
    })

    let rnd = Math.random().toString()


    return <>
        <div className={styles["repeat-pattern-view"]}>
            <strong>{props.label}</strong>
            <div className="pull-right form-check form-check-inline" style={{ display: "inline-block", marginLeft: "20px" }}>
                <input className="form-check-input"
                    type="checkbox"
                    checked={allSelected}
                    id={rnd + "-input-all"}
                    onChange={e => e.target.checked ? props.onSelectAll && props.onSelectAll() : props.onSelectNone && props.onSelectNone()}
                />
                <label style={{ marginBottom: "0" }} htmlFor={rnd + "-input-all"}>All</label>
            </div>

            <div className={props.flex ? styles["flex-children"] : styles["children"]}>
                {(props.items || []).map((item, i) => <div key={i} className="form-check">
                    <input type="checkbox" id={rnd + "-input-" + i}
                        className="form-check-input"
                        checked={item.selected}
                        onChange={e => props.onSelectChange && props.onSelectChange(i, e.target.checked)}
                    />
                    <label className="form-check-label" htmlFor={rnd + "-input-" + i}>{item.value}</label>
                </div>)}
            </div>

        </div>


    </>
}

function setFlag(flags: number, index: number, set: boolean) {
    if (set)
        flags = flags | (1 << index)
    else
        flags = flags & (~(1 << index))
    return flags
}

export default function RepeatPatternView(props: RepeatPatternViewProps) {
    const onMonthSelect = (monthIdx: number, selected: boolean) => props.onChange && props.onChange({
        ...props.pattern,
        monthFlags: setFlag(props.pattern.monthFlags, monthIdx, selected),
    })

    const onDaySelect = (dayIdx: number, selected: boolean) => props.onChange && props.onChange({
        ...props.pattern,
        dayFlags: setFlag(props.pattern.dayFlags, dayIdx, selected),
    })


    const onWeekdaySelect = (dayIdx: number, selected: boolean) => props.onChange && props.onChange({
        ...props.pattern,
        weekdayFlags: setFlag(props.pattern.weekdayFlags, dayIdx, selected),
    })

    const onHourSelect = (hourIdx: number, selected: boolean) => props.onChange && props.onChange({
        ...props.pattern,
        hourFlags: setFlag(props.pattern.hourFlags, hourIdx, selected),
    })

    const onMinuteSelect = (minuteIdx: number, selected: boolean) => {
        let minuteFlagsLow = props.pattern.minuteFlagsLow
        let minuteFlagsHigh = props.pattern.minuteFlagsHigh
        if (minuteIdx < 31)
            minuteFlagsLow = setFlag(minuteFlagsLow, minuteIdx, selected)
        else
            minuteFlagsHigh = setFlag(minuteFlagsHigh, minuteIdx - 31, selected)
        props.onChange && props.onChange({
            ...props.pattern,
            minuteFlagsLow,
            minuteFlagsHigh,
        })
    }

    const invokeChange = (updated: RepeatPattern) => props.onChange && props.onChange(updated)


    return <div className="container" style={{ borderRadius: "4px", border: "1px solid lightgray", overflow: "hidden", marginTop: "30px", position: "relative" }}>
        <div style={{ margin: "6px" }}>
            <div className="row">
                <div className="col-sm">
                    <SelectableGroup label="Select Months"
                        onSelectAll={() => invokeChange({ ...props.pattern, monthFlags: flagsAllMonths })}
                        onSelectNone={() => invokeChange({ ...props.pattern, monthFlags: 0 })}
                        onSelectChange={onMonthSelect}
                        items={months.map((m, i) => ({ value: m, selected: (props.pattern.monthFlags & (1 << i)) > 0 }))}
                    />

                    <div className="spacer" style={{ height: "12px" }} />
                    <SelectableGroup label="Select Weekday"
                        onSelectAll={() => invokeChange({ ...props.pattern, weekdayFlags: 0xFF })}
                        onSelectNone={() => invokeChange({ ...props.pattern, weekdayFlags: 0 })}
                        onSelectChange={onWeekdaySelect}
                        items={weekdays.map((wd, i) => ({ value: wd, selected: (props.pattern.weekdayFlags & (1 << i)) > 0 }))}
                    />
                </div>
                <div className="col-sm">
                    <SelectableGroup label="Select Days"
                        onSelectAll={() => invokeChange({ ...props.pattern, dayFlags: flagsAllDays })}
                        onSelectNone={() => invokeChange({ ...props.pattern, dayFlags: 0 })}
                        onSelectChange={onDaySelect}
                        flex={true}
                        items={allDays.map((day, i) => ({ value: day.toString(), selected: (props.pattern.dayFlags & (1 << i)) > 0 }))}
                    />

                    <SelectableGroup label="Select Hours"
                        onSelectAll={() => invokeChange({ ...props.pattern, hourFlags: flagsAllHours })}
                        onSelectNone={() => invokeChange({ ...props.pattern, hourFlags: 0 })}
                        onSelectChange={onHourSelect}
                        flex={true}
                        items={allHours.map((day, i) => ({ value: day.toString(), selected: (props.pattern.hourFlags & (1 << i)) > 0 }))}
                    />

                    <SelectableGroup label="Select Minutes"
                        onSelectAll={() => invokeChange({ ...props.pattern, minuteFlagsLow: flagsAllMinutesLow, minuteFlagsHigh: flagsAllMinutesHigh })}
                        onSelectNone={() => invokeChange({ ...props.pattern, minuteFlagsLow: 0, minuteFlagsHigh: 0 })}
                        onSelectChange={onMinuteSelect}
                        flex={true}
                        items={allMinutes.map((day, i) => ({ value: day.toString(), selected: i < 31 ? (props.pattern.minuteFlagsLow & (1 << i)) > 0 : (props.pattern.minuteFlagsHigh & (1 << (i - 31))) > 0 }))}
                    />
                </div>
            </div>
        </div>
        <button type="button" onClick={() => props.onRemove && props.onRemove()} className="btn btn-danger btn-md" style={{ position: "absolute", bottom: "10px", right: "10px" }}>Remove</button>
    </div>
}
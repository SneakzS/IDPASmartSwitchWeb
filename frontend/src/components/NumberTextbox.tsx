import React from "react";
import ValidateTextbox from "./ValidateTextbox"

export interface NumberTextboxProps extends React.InputHTMLAttributes<HTMLInputElement> {
    valid: boolean
    invalidMessage: string
    numberValue: number
    onNumberChange?(v: number): void
    numberType: "integer" | "float"
}

interface NumberTextBoxState {
    value: string
    numberValue: number
}

export default function NumberTextbox({ valid, invalidMessage, numberValue, onNumberChange, numberType, ...props }: NumberTextboxProps) {
    const [state, setState] = React.useState<NumberTextBoxState>({
        value: numberValue.toString(),
        numberValue: numberValue,
    })

    React.useEffect(() => {
        setState(s => ({
            ...s,
            value: numberValue.toString(),
            numberValue
        }))
    }, [numberValue])

    if (props.onChange)
        throw new Error("NumberTextBox doesn't support onChange. Use OnNumberChange instead")

    if (props.value)
        throw new Error("NumberTextBox does't support value.Use numberValue instead")

    if (Number.isNaN(state.numberValue)) {
        valid = false
        invalidMessage = "A number is required"
    }

    const onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        let value = e.target.value
        let numberValue: number
        switch (numberType) {
            case "integer":
                numberValue = Number.parseInt(value)
                break;
            case "float":
                numberValue = Number.parseFloat(value)
                break;
        }
        setState(s => ({ ...s, value, numberValue }))

        if (!Number.isNaN(numberValue))
            onNumberChange && onNumberChange(numberValue)
    }

    return <ValidateTextbox valid={valid}
        invalidMessage={invalidMessage}
        onChange={onChange}
        value={state.value}
        {...props} />
}
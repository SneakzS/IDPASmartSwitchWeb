function validateStringRequired(str: string | null | undefined): boolean {
    if (!str)
        return false;
    return str.trim().length > 0
}


export default {
    stringRequired: validateStringRequired,
}
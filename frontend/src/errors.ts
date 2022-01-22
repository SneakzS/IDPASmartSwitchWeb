declare const location: Location
declare function confirm(message?: string | undefined): boolean 

export function defaultErrorHandler(err: any) {
    console.error(err);
    if (confirm("Unhandled Exception\nReload?")) {
        location.reload()
    }
}

export default {
    defaultHandler: defaultErrorHandler
}
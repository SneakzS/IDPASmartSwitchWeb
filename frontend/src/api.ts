const apiAddress = "/api/v1/"

export interface ApiResult<T> {
    status: number
    json(): Promise<T>
}

export function getApiResource<T = void>(resource: string): Promise<ApiResult<T>> {
    return invokeApi("GET", resource)
}

export function postApiResource<T = void>(resource: string, body: any): Promise<ApiResult<T>> {
    console.log(body)
    return invokeApi("POST", resource, body)
}

export function deleteApiResource<T = void>(resource: string, body: any = null): Promise<ApiResult<T>> {
    return invokeApi("DELETE", resource, body)
}

async function invokeApi<T = void>(method: string, resource: string, body: any = null): Promise<ApiResult<T>> {
    if (resource.startsWith("/"))
        resource = resource.substring(1)
    let resp = await fetch(apiAddress + resource, {
        method: method,
        body: body ? JSON.stringify(body) : null,
    })
    if (resp.status >= 400)
        throw new Error(`fetch /${resource}: ${resp.statusText} (${resp.status})`)

    return resp
}

export default {
    get: getApiResource,
    post: postApiResource,
    delete: deleteApiResource,
}
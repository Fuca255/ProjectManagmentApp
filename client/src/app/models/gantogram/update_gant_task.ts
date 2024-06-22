export interface UpdateGant {
    startTs?: number,
    endTs?: number,
    addDependentOn?: number[],
    removeDependentOn?: number[],
    stateId?: number,
    percentage?: number,
    title?: string,
    type?: string,
    description?: string,
    priorityId?: number
}

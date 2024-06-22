export interface UpdateTask {
    taskGroupId: number,
    userIds: number[],
    start: Date,
    end: Date,
    dependentOn: number[],
    stateId: number,
    percentage: number,
    title: string,
    type: string,
    description: string,
    priorityId: number,
    isClosed?:boolean
}

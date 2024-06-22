export interface CreateTask {
    owner : number,
    taskGroupId : number,
    assignees : number[],
    start : Date | number,
    end : Date | number,
    dependentOn : number[],
    title : string,
    type : string,
    description : string,
    priorityId : number,
    stateId : number

}

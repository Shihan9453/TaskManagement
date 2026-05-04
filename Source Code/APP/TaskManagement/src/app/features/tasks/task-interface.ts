export interface TaskInterface 
{
     id?:number,
     title:string,
     description:string,
     status:string,
     priorityLevel:string,
     assignedPerson:string,
     assignedPersonId:number,
     dueDate:string,
     completedDate?:string,
     remarks?:string
}
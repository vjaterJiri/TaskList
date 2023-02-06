import { TaskStatus } from "./TaskStatus";

export default interface ICreateTask {
    name: string;
    priority: number;
    status: TaskStatus;
}
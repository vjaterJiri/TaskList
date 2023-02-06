import { TaskStatus } from "./TaskStatus";

export default interface ITask {
    id: string;
    name: string;
    priority: number;
    status: TaskStatus;
}


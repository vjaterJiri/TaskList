import * as React from 'react';
import { Link, RouteProps } from 'react-router-dom';
import axios, { AxiosError } from 'axios';
import { AppSettings } from '../AppSettings';
import ITask from './ITask'
import IValidationError from './IValidationError'

interface IState {
    tasks: ITask[];
    deleteError: boolean;
    deleteErrorMessage: string | null;
}



export default class Home extends React.Component<RouteProps, IState> {
    constructor(props: RouteProps) {
        super(props);
        this.state =
        {
            tasks: [],
            deleteError: false,
            deleteErrorMessage: null
        }
    }
    public componentDidMount(): void {
        axios.get(`${AppSettings.API_ENDPOINT}`).then(data => {
            this.setState({ tasks: data.data })
        })
    }
    public deleteTask(id:string) {
        axios.delete(`${AppSettings.API_ENDPOINT}/${id}`).then(data => {
            const index = this.state.tasks.findIndex(task => task.id === id);
            this.state.tasks.splice(index, 1);
            this.setState({ deleteError: false, deleteErrorMessage: '' });
        }).catch((reason: AxiosError) => {
            const error = reason.response?.data as IValidationError;
            this.setState({ deleteError: true, deleteErrorMessage: error?.detail || 'Error during delete' });
        });
    }

    public render() {
        const { tasks, deleteError, deleteErrorMessage } = this.state;
        return (
            <div>
                {tasks.length === 0 && (
                    <div className="text-center">
                        <h2>No tasks found at the moment</h2>
                    </div>
                )}

                {deleteError && (
                    <div className="text-center col-md-12 form-wrapper">
                        <div className="alert alert-danger" role="alert">
                            {deleteErrorMessage}
                        </div>
                    </div>
                )}

                <div className="container">
                    <div className="row">
                        <table className="table table-bordered">
                            <thead className="thead-light">
                                <tr>
                                    <th scope="col">Task Name</th>
                                    <th scope="col">Priority</th>
                                    <th scope="col">Status</th>
                                    <th scope="col">Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {tasks && tasks.map(task =>
                                    <tr key={task.id}>
                                        <td>{task.name}</td>
                                        <td>{task.priority}</td>
                                        <td>{task.status}</td>
                                        <td>
                                            <div className="d-flex justify-content-between align-items-center">
                                                <div className="btn-group" style={{ marginBottom: "20px" }}>
                                                    <Link to={`edit/${task.id}`} className="btn btn-sm btn-outline-secondary">Edit Task</Link>
                                                    <button className="btn btn-sm btn-outline-secondary" onClick={() => this.deleteTask(task.id)}>Delete Task</button>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        )
    }
}
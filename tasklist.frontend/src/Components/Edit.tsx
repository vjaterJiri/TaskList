import * as React from 'react';
import { useParams, useNavigate, RouteProps, NavigateFunction } from 'react-router-dom';
import axios, { AxiosError } from 'axios';
import { AppSettings } from '../AppSettings';
import ITask from './ITask'
import IValidationError from './IValidationError';

export interface IValues {
    [key: string]: any;
}
export interface IFormState{
    id: string;
    task: ITask|null,
    values: IValues;
    submitSuccess: boolean;
    loading: boolean;
    submitError: boolean;
    submitErrorMessage: string | null;
}

interface EditProps {
    routeProps: RouteProps,
    taskId: string | undefined,
    navigate: NavigateFunction
}

class Edit extends React.Component<EditProps, IFormState> {
    constructor(props: EditProps) {
        super(props);

        this.state = {
            id: props.taskId || "",
            task: null,
            values: [],
            loading: false,
            submitSuccess: false,
            submitError: false,
            submitErrorMessage: ''
            
        }
    }

    public componentDidMount(): void {
        axios.get(`${AppSettings.API_ENDPOINT}/${this.state.id}`).then(data => {
            this.setState({ task: data.data });
        })
    }

    private processFormSubmission = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
        e.preventDefault();
        this.setState({ loading: true });
        
        axios.put(`${AppSettings.API_ENDPOINT}/${this.state.id}`, { ...this.state.task, ...this.state.values }).then(data => {
            this.setState({ submitSuccess: true, loading: false, submitError: false, submitErrorMessage: '' })
            setTimeout(() => {
                this.props.navigate('/');
            }, 1500)
        }).catch((reason: AxiosError) => {
            const error = reason.response?.data as IValidationError;
            this.setState({ submitSuccess: false, submitError: true, submitErrorMessage: error?.detail || 'Error during delete', loading: false });
        });
    }

    private setValues = (values: IValues) => {
        this.setState({ values: { ...this.state.values, ...values } });
    }

    private handleInputChanges = (e: React.FormEvent<HTMLInputElement>) => {
        e.preventDefault();
        if (e.currentTarget.id == 'priority') {
            this.setValues({[e.currentTarget.id]: Number.parseInt(e.currentTarget.value) });
        }
        else {
            this.setValues({ [e.currentTarget.id]: e.currentTarget.value })
        }
    }

    private handleStatusChanges = (e: React.FormEvent<HTMLSelectElement>) => {
        e.preventDefault();
        this.setValues({ [e.currentTarget.id]: e.currentTarget.value });
    }

    public render() {
        const { submitSuccess, loading, submitError, submitErrorMessage } = this.state;
        return (
            <div className="App">
                {this.state.task &&
                    <div>
                        <div>
                            <div className={"col-md-12 form-wrapper"}>
                                <h2> Edit Task</h2>
                                {submitSuccess && (
                                    <div className="alert alert-info" role="alert">
                                        Task's details has been edited successfully </div>
                                )}
                                {submitError && (
                                    <div className="alert alert-danger" role="alert">
                                        {submitErrorMessage}
                                    </div>
                                )}
                                <form id={"create-post-form"} onSubmit={this.processFormSubmission}>
                                    <div className="form-group col-md-12">
                                        <label htmlFor="name">Name</label>
                                        <input type="text" id="name" defaultValue={this.state.task.name} onChange={(e) => this.handleInputChanges(e)} name="name" className="form-control" placeholder="Enter task name" required />
                                    </div>
                                    <div className="form-group col-md-12">
                                        <label htmlFor="priority">Priority</label>
                                        <input type="number" min="1" id="priority" defaultValue={this.state.task.priority} onChange={(e) => this.handleInputChanges(e)} name="priority" className="form-control" placeholder="Enter task priority" required/>
                                    </div>
                                    <div className="form-group col-md-12">
                                        <label htmlFor="statis">Status</label>
                                        <select name="status" id="status" onChange={(e) => { this.handleStatusChanges(e) }} defaultValue="NotStarted" className="form-select">
                                            <option value="NotStarted">Not Started</option>
                                            <option value="InProgress">In Progress</option>
                                            <option value="Completed">Completed</option>
                                        </select>
                                    </div>
                                    <div className="form-group col-md-4 pull-right my-4">
                                        <button className="btn btn-success" type="submit">
                                            Edit Task
                                        </button>
                                        {loading &&
                                            <span className="fa fa-circle-o-notch fa-spin" />
                                        }
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                }
            </div>
        )
    }
}

export default (props: RouteProps) => {

    let { id } = useParams()
    let navigate = useNavigate();

    return (
        <Edit
            routeProps={props}
            taskId={id}
            navigate={navigate}
        />
    );
}
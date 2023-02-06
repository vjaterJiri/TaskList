import * as React from 'react';
import axios, { AxiosError } from 'axios';
import { NavigateFunction, RouteProps, useNavigate } from 'react-router-dom';
import { AppSettings } from '../AppSettings';
import ICreateTask from './ICreateTask';
import IValidationError from './IValidationError';


export interface IFormState {
    [key: string]: any;
    values: ICreateTask[];
    submitSuccess: boolean;
    loading: boolean;
    submitError: boolean;
    submitErrorMessage: string | null;
}

interface CreateProps {
    routeProps: RouteProps,
    navigate: NavigateFunction
}

class Create extends React.Component<CreateProps, IFormState> {
    constructor(props: CreateProps) {
        super(props);
        this.state = {
            name: '',
            priority: 0,
            status: 'NotStarted',
            values: [],
            loading: false,
            submitSuccess: false,
            submitError: false,
            submitErrorMessage: ''
        }
    }

    private processFormSubmission = (e: React.FormEvent<HTMLFormElement>): void => {
        e.preventDefault();
        this.setState({ loading: true });
        const formData: ICreateTask = {
            name: this.state.name,
            priority: Number.parseInt(this.state.priority),
            status: this.state.status,
        }

        console.log(formData);
        axios.post(`${AppSettings.API_ENDPOINT}/`, formData).then(data => [
            setTimeout(() => {
                this.props.navigate('/');
            }, 1500)
        ]).then((x) => {
            this.setState({ submitSuccess: true, submitError: false, submitErrorMessage: '', values: [...this.state.values, formData], loading: false });
        }).catch((reason: AxiosError) => {
            const error = reason.response?.data as IValidationError;
            this.setState({ submitSuccess: false, submitError: true, submitErrorMessage: error?.detail || 'Error during delete' , loading: false });
		});


    }

    private handleInputChanges = (e: React.FormEvent<HTMLInputElement>) => {
        e.preventDefault();
        this.setState({
            [e.currentTarget.name]: e.currentTarget.value,
        })
    }

    private handleStatusChanges = (e: React.FormEvent<HTMLSelectElement>) => {
        e.preventDefault();
        this.setState({
            [e.currentTarget.name]: e.currentTarget.value,
        })
    }

    public render() {
        const { submitSuccess, loading, submitError, submitErrorMessage } = this.state;
        return (
            <div>
                <div className={"col-md-12 form-wrapper"}>
                    <h2> Create Task </h2>
                    {!submitSuccess && (
                        <div className="alert alert-info" role="alert">
                            Fill the form below to create a new task
                        </div>
                    )}
                    {submitSuccess && (
                        <div className="alert alert-info" role="alert">
                            The task was successfully submitted!
                        </div>
                    )}
                    {submitError && (
                        <div className="alert alert-danger" role="alert">
                            {submitErrorMessage}
                        </div>
                    )}
                    <form id={"create-post-form"} onSubmit={this.processFormSubmission}>
                        <div className="form-group col-md-12">
                            <label htmlFor="name">Name</label>
                            <input type="text" min="1" id="name" onChange={(e) => this.handleInputChanges(e)} name="name" className="form-control" placeholder="Enter task name" required />
                        </div>
                        <div className="form-group col-md-12">
                            <label htmlFor="priority">Priority </label>
                            <input type="number" id="priority" onChange={(e) => this.handleInputChanges(e)} name="priority" className="form-control" placeholder="Enter task priority" required/>
                        </div>
                        <div className="form-group col-md-12">
                            <label htmlFor="status">Status</label>
                            <select name="status" id="status" onChange={(e) => { this.handleStatusChanges(e) }} defaultValue="NotStarted" className="form-select" required>
                                <option value="NotStarted">Not Started</option>
                                <option value="InProgress">In Progress</option>
                                <option value="Completed">Completed</option>
                            </select>


                        </div>
                        <div className="form-group col-md-4 pull-right my-4">
                            <button className="btn btn-success" type="submit">
                                Create Task
                            </button>
                            {loading &&
                                <span className="fa fa-circle-o-notch fa-spin" />
                            }
                        </div>
                    </form>
                </div>
            </div>
        )
    }
}

export default (props: RouteProps) => {

    let navigate = useNavigate();

    return (
        <Create
            routeProps={props}
            navigate={navigate}
        />
    );
}
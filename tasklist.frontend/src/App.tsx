import * as React from 'react';
import './App.css';
import { Routes, Link, Route } from 'react-router-dom';
import Home from './Components/Home';
import Edit from './Components/Edit';
import Create from './Components/Create';

class App extends React.Component {
    public render() {
        return (
            <div>
                <nav>
                    <ul>
                        <li>
                            <Link to={'/'}> Home </Link>
                        </li>
                        <li>
                            <Link to={'/create'}> Create Customer </Link>
                        </li>
                    </ul>
                </nav>

                <h1 className="text-center my-4">Task List App</h1>

                <Routes>
                    <Route path="/" element={<Home />}/>
                    <Route path="edit/:id" caseSensitive={false} element={<Edit />} />
                    <Route path="create" caseSensitive={false} element={<Create />} />
                
                </Routes>

            </div>
        );
    }
}
export default App;
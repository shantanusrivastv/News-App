import React from 'react';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import Login from './Views/Login';
import './App.css';

function App(props) {
  return (
    <div>
      <h1>{props.appTitle} </h1>
      <Router>
        <Switch>
          <Route path="/" exact={true} render={() => <Login />} />
        </Switch>
      </Router>
    </div>
  );
}

export default App;

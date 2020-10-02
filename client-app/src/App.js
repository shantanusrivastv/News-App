import React, { useReducer } from "react";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import "./App.css";
import Dashboard from "./Views/Dashboard";
import Login from "./Views/Login";
import { reducer } from "./store/reducer";

const initialState = {
  userInfo: {
    name: "",
    role: "",
    token: null,
  },
  authorised: false,
  articles: [],
  editForm: null,
};

function App(props) {
  const [store, dispatch] = useReducer(reducer, initialState);

  return (
    <div>
      <h1>{props.appTitle} </h1>
      <Router>
        <Switch>
          <Route
            path="/"
            exact={true}
            render={() => <Login {...store} dispatch={dispatch} />}
          />
          <Route
            path="/dashboard"
            render={() => <Dashboard {...store} dispatch={dispatch} />}
          />
        </Switch>
      </Router>
    </div>
  );
}

export default App;

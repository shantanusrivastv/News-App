import React, { useState } from "react";
import {
  Button,
  FormLabel,
  Grid,
  Paper,
  TextField,
  Alert,
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import { login } from '../Common/axios-news';
import { useNavigate } from 'react-router-dom';
import { RoleType } from '../Common/constants';
import { useAuth } from '../context/AuthContext';

const useStyles = makeStyles(() => ({
  root: {
    flexGrow: 1,
    background: "#3f51b5",
  },
  rootContainer: {
    height: "100vh",
  },
  container: {
    padding: 8,
  },
  input: {
    padding: "8px 0",
  },
  button: {
    padding: "8px 0",
    textAlign: "right",
  },
}));

const Login: React.FC = () => {
  const classes = useStyles();
  const [userName, setUserName] = useState("adminUser@pressford.com");
  const [password, setPassword] = useState("admin");
  const [errorMessage, setErrorMessage] = useState("");
  const navigate = useNavigate();
  const { setAuth } = useAuth();

  const handleLogin = async () => {
    try {
      const response = await login(userName, password);
      console.log("Login successful:", response);
	  const authData = {
		token: response.token,
		username: response.username,
		name: response.name,
		role: response.role as RoleType,
		isAuthenticated: true
     };
	 setAuth(authData);
      navigate("/Dashboard");
    } catch (error) {
      console.error("Login failed:", error);
      setErrorMessage(
        "Login failed. Please check your credentials and try again."
      );
    }
  };
  return (
    <div className={classes.root}>
      <Grid
        container
        justifyContent="center"
        alignItems="center"
        alignContent="center"
        className={classes.rootContainer}
      >
        <Grid item xs={4}>
          <Paper className={classes.container}>
            <Grid container>
              <FormLabel>
                <h4>Login</h4>
              </FormLabel>
              <Grid item xs={12} className={classes.input}>
                <TextField
                  label="User Name"
                  value={userName}
                  onChange={(e) => setUserName(e.target.value)}
                  variant="filled"
                  fullWidth
                />
              </Grid>

              <Grid item xs={12} className={classes.input}>
                <TextField
                  label="Password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  variant="filled"
                  fullWidth
                  type="password"
                />
              </Grid>

              <Grid container alignContent="flex-end" spacing={2}>
                <Grid item xs={12} className={classes.button}>
                  <Button
                    variant="contained"
                    color="primary"
                    onClick={handleLogin}>
                    Login
                  </Button>
                </Grid>
              </Grid>

              {errorMessage && <Alert severity="error">{errorMessage}</Alert>}
            </Grid>
          </Paper>
        </Grid>
      </Grid>
    </div>
  );
};

export default Login;

import { Button, FormLabel, Grid, makeStyles, Paper, TextField } from '@material-ui/core';
import React, { useState } from 'react';
import { withRouter } from 'react-router-dom';
import { actionTypes, RoleType } from '../Common/constants';
import axios from '../Common/axios-news';

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
        background: "#3f51b5"
    },
    rootContainer: {
        height: "100vh"
    },
    container: {
        padding: 8
    },
    input: {
        padding: "8px 0"
    },
    button: {
        padding: "8px 0",
        textAlign: "right"
    },
}));





const Login = (props) => {
    const { history, dispatch } = props;
    //const [loginDetails, setValue] = useState({useName:"adminUser@pressford.com",password: "admin"});
    // const [loginDetails, setValue] = useState({useName:"adminUser@pressford.com",password: "admin"});
    const [userName, setUserName] = useState("adminUser@pressford.com");
    const [password, setPassword] = useState("admin");
    const [message, setMessage] = useState(null)
    const classes = useStyles();
    const credentials = {
        "username": "adminUser@pressford.com",
        "password": "admin"
    }   
    
    const credentials2 = {
        "username": userName,
        "password": password
    }
    return <div className={classes.root}>
        <Grid
            container
            justify="center"
            alignItems="center"
            alignContent="center"
            className={classes.rootContainer}
        >
            <Grid item xs={4}>
                <Paper
                    className={classes.container}>
                    <Grid container>
                        <FormLabel>
                            <h4>Login</h4></FormLabel>
                        <Grid item xs={12}  className={classes.input}>
                            <TextField label="User Name" 
                            value={userName} 
                             onChange={(e)=> setUserName(e.target.value)}
                            // onChange={(e)=> setValue (state => ({
                            //     ...state,
                            //     useName : e.target.value
                            // }))}
                            variant="filled" fullWidth />
                        </Grid>       

                                               
     
                        
                        <Grid item xs={12}  className={classes.input}>
                            <TextField label="Password" 
                            value={password} 
                            onChange={(e)=> setPassword(e.target.value)} 
  
                            variant="filled" fullWidth />
                        </Grid>
                        <Grid
                            item
                            xs={12}
                            spacing={2}
                            alignContent="flex-end"
                            className={classes.button}
                        >
                            <Button
                                variant="contained"
                                color="primary"
                                onClick={() => {

                                    axios.post('Account/authenticate',credentials2)
                                        .then(response => {
                                            dispatch({
                                                type: actionTypes.USER_LOGIN,
                                                userInfo: response.data

                                            })

                                            history.push("/dashboard")
                                        })
                                        .catch(error => {
                                            console.error(error)
                                        });

                                }}
                            >Login</Button>
                        </Grid>
                        {
                            message &&
                            <Grid>
                                {message}
                            </Grid>
                        }
                    </Grid>
                </Paper>
            </Grid>
        </Grid>
    </div>
}

export default withRouter(Login)
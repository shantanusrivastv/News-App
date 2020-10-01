import { Button, FormLabel, Grid, makeStyles, Paper, TextField } from '@material-ui/core';
import React, { useState } from 'react';
import { Redirect, withRouter } from 'react-router-dom';
import { actionTypes} from '../Common/constants';
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
    const { dispatch, authorised } = props;
    //const [loginDetails, setValue] = useState({useName:"adminUser@pressford.com",password: "admin"});
    // const [loginDetails, setValue] = useState({useName:"adminUser@pressford.com",password: "admin"});
    const [userName, setUserName] = useState("adminUser@pressford.com");
    const [password, setPassword] = useState("admin");
    const [message] = useState(null);
    const [isLogin, setLogin] = useState(false);
    const classes = useStyles();

    
    const credentials = {
        "username": userName,
        "password": password
    }


    React.useEffect(() => {
        if( sessionStorage.getItem("userInfo") || isLogin){
            dispatch({
                type: actionTypes.USER_LOGIN,
                userInfo: JSON.parse(sessionStorage.getItem("userInfo"))
            })
        }

    }, [dispatch,isLogin])

if(authorised ){
    return <Redirect to={"/Dashboard"} />
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

                                    axios.post('Account/authenticate',credentials)
                                        .then(response => {
                                            console.log(JSON.stringify( response.data));
                                            sessionStorage.setItem("userInfo",
                                            JSON.stringify( response.data));
                                            setLogin(true);
                                            // dispatch({
                                            //     type: actionTypes.USER_LOGIN,
                                            //     userInfo: response.data
                                            // })

                                            //history.push("/dashboard")
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
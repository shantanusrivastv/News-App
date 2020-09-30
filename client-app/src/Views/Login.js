import { Button, FormLabel, Grid, makeStyles, Paper, TextField } from '@material-ui/core';
import React, { useState } from 'react';
import {withRouter } from 'react-router-dom';

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

    const classes = useStyles();
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
                        <Grid item xs={12} spacing={2} className={classes.input}>
                            <TextField label="User Name" variant="filled" fullWidth />
                        </Grid>
                        <Grid item xs={12} spacing={2} className={classes.input}>
                            <TextField label="Password" variant="filled" fullWidth />
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
                                    //TODO: Calling API

                                    alert("I am clicked");

                                   
                                }
                                }
                            >Login</Button>
                        </Grid>

                    </Grid>
                </Paper>
            </Grid>
        </Grid>
    </div>
}

export default withRouter(Login)
import React, { useEffect } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Header from '../Components/Header';
import Articles from '../Components/Articles';
import PublishArticle from '../Components/PublishArticle'
import { Redirect, withRouter } from 'react-router-dom';
import { actionTypes, RoleType } from '../Common/constants';


const useStyles = makeStyles(() => ({
    root: {
        flexGrow: 1,
        padding: 8,
    },
}));



const rawArticles = [
    {
        Id: 1, Title: "Article-1", Description: "Article-1", Like: false
    },
    {
        Id: 2, Title: "Article-2", Description: "Article-2", Like: false
    },
    {
        Id: 3, Title: "Article-3", Description: "Article-3", Like: false
    },
];

const DashBoard = (props) => {
    const { userInfo,authorised, publisherArticles, editForm,dispatch } = props;
    const classes = useStyles();

    useEffect(() => {
        console.log("I am called");
        //TODO: Calling API <authToken>
        dispatch({
            type: actionTypes.LOAD_PUBLISHER_ARTICLES,
            payload: rawArticles
        })
    }, [dispatch])

    if (!authorised) {
        return <Redirect to={"/"} />
    }

    return (
        <div className={classes.root}>
            <Header />
            {
               userInfo.role === RoleType.PUBLISHER && <PublishArticle dispatch={dispatch} editForm={editForm} />
            }
            <Articles publisherArticles={publisherArticles} dispatch={dispatch} role={userInfo.role} />
        </div>
    );
}

export default withRouter(DashBoard)
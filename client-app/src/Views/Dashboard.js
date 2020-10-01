import React, { useEffect, useCallback  } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Header from '../Components/Header';
import Articles from '../Components/Articles';
import PublishArticle from '../Components/PublishArticle'
import { Redirect, withRouter } from 'react-router-dom';
import { actionTypes, RoleType } from '../Common/constants';
import axios from '../Common/axios-news';


const useStyles = makeStyles(() => ({
    root: {
        flexGrow: 1,
        padding: 8,
    },
}));


const DashBoard = (props) => {
    const { userInfo, authorised, articles, editForm, dispatch } = props;
    const classes = useStyles();

    const GetPublisherArtciles = useCallback(() => {
        axios.get('Dashboard/GetPublisherDashboard')
            .then(response => {
                dispatch({
                    type: actionTypes.LOAD_ARTICLES,
                    payload: response.data
                })
            })
            .catch(error => {
                console.error(error)
            })
    }, [dispatch])

    const GetAllArtciles = useCallback(() => {
        axios.get('Dashboard/')
            .then(response => {
                dispatch({
                    type: actionTypes.LOAD_ARTICLES,
                    payload: response.data
                })
            })
            .catch(error => {
                console.error(error)
            });
    }, [dispatch])

    const loadArticles = useCallback(() => {
        userInfo.role === RoleType.PUBLISHER ? GetPublisherArtciles()
        : GetAllArtciles()
      }, [userInfo.role,GetAllArtciles,GetPublisherArtciles])

      useEffect(() => {
        loadArticles();
    }, [loadArticles])


    if (!authorised) {
        return <Redirect to={"/"} />
    }


    return (
        <div className={classes.root}>
            <Header authorised={authorised} dispatch={dispatch}> </Header>
            {
             <PublishArticle dispatch={dispatch} editForm={editForm} role={userInfo.role} />

            }
            <Articles articles={articles} dispatch={dispatch} role={userInfo.role} />
        </div>
    );
}

export default withRouter(DashBoard)
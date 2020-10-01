import React from 'react';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import { Button } from '@material-ui/core';
import { actionTypes, RoleType } from '../Common/constants';
import { ThumbUp, ThumbDown, Edit, Explore, Delete } from '@material-ui/icons';
import axios from '../Common/axios-news';

const useStyles = makeStyles((theme) => ({
  root: {
    padding: '16px 0px',
  },
}));

export default function Articles(props) {
  const { articles, role, dispatch } = props;
  const classes = useStyles();

  const options = { year: 'numeric', month: 'long', day: 'numeric', weekday: 'long' };

  return (
    <React.Fragment>
      <Typography variant="h5" color="primary" className={classes.root} >
        {role === RoleType.PUBLISHER ? "My Published Articles" : "All Articles"}
      </Typography>
      <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell>Article Id</TableCell>
            <TableCell>Title</TableCell>
            <TableCell>Description</TableCell>
            <TableCell>Author</TableCell>
            <TableCell>DatePublished</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {articles && articles.map((row) => (
            <TableRow key={row.id}>
              <TableCell>{row.id}</TableCell>
              <TableCell>{row.title}</TableCell>
              <TableCell>{row.body}</TableCell>
              <TableCell>{row.author}</TableCell>
              <TableCell>{new Date(row.datePublished).toLocaleDateString("en-UK", options)}</TableCell>
              <TableCell>
                {

                  <Button onClick={
                    () => {
                      dispatch({
                        type: actionTypes.SETUP_DETAILS_VIEW,
                        payload: row
                      })
                    }
                  } color="primary">
                    {role === RoleType.PUBLISHER ? <Edit /> : <Explore />}
                  </Button>
                }
                <Button onClick={() => {
                  // const endPoint = row.isLiked? "UnLikeArticle/" :"LikeArticle/"
                  // axios.post(endPoint + row.id)
                  // .then(response => {
                  //   dispatch({
                  //     type:  actionTypes.TOGGLE_LIKE ,
                  //     payload: row
                  //   })
                  // })
                  // .catch(error => {
                  //     console.error(error)
                  // });

                  dispatch({
                    type: actionTypes.TOGGLE_LIKE,
                    payload: row
                  })

                }
                } color="primary">{
                    !row.isLiked ? <ThumbUp /> : <ThumbDown />
                  }</Button>
                <Button onClick={
                  () => {
                    axios.delete('Article/' + row.id)
                      .then(response => {
                        dispatch({
                          type: actionTypes.TOGGLE_LIKE,
                          payload: row
                        })
                      })
                      .catch(error => {
                        console.error(error)
                      });


                    dispatch({
                      type: actionTypes.DELETE_ARTICLE,
                      payload: row.id
                    })
                  }
                } color="primary">
                  <Delete />
                </Button>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </React.Fragment>
  );
}
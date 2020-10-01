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
import { ThumbUp, ThumbDown, Edit } from '@material-ui/icons';


const useStyles = makeStyles((theme) => ({
  root: {
    padding: '16px 0px',
  },
}));

export default function Articles(props) {
  const { publisherArticles,role, dispatch } = props;
  const classes = useStyles();
  
  return (
    <React.Fragment>
      <Typography variant="h5" color="primary" className={classes.root} >
        Articles
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
          {publisherArticles && publisherArticles.map((row) => (
            <TableRow key={row.id}>
              <TableCell>{row.id}</TableCell>
              <TableCell>{row.title}</TableCell>
              <TableCell>{row.body}</TableCell>
              <TableCell>{row.author}</TableCell>
              <TableCell>{row.datePublished}</TableCell>
              <TableCell>
                {
                  role === RoleType.PUBLISHER &&
                  <Button onClick={
                    () => {
                      //TODO: Calling API
                      dispatch({
                        type: actionTypes.EDIT_ARTICLE,
                        payload: row
                      })
                    }
                  } color="primary">
                    <Edit />
                  </Button>
                }
                <Button onClick={() => {
                  //TODO: Calling API
                  dispatch({
                    type:  actionTypes.TOGGLE_LIKE ,
                    payload: row
                  })
                }
                } color="primary">{
                    !row.Like ? <ThumbUp /> : <ThumbDown />
                  }</Button>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </React.Fragment>
  );
}
import React, { useCallback, useEffect } from 'react';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import { actionTypes } from '../Common/constants';
import axios from '../Common/axios-news'


export default function FormDialog(props) {
  const { dispatch, editForm } = props;
  const [open, setOpen] = React.useState(false);
  const [id, setId] = React.useState(0);
  const [title, setTitle] = React.useState(null);
  const [description, setDescription] = React.useState(null);

  const isEdit = React.useState(false);

  useEffect(() => {
    if (editForm) {
      setId(editForm.id)
      setTitle(editForm.title)
      setDescription(editForm.body)
      setOpen(true)
    }
  }, [editForm])


  const handleClose = useCallback(() => {
    setOpen(false);
  }, []);

  const onPublishArticle = useCallback(() => {
    let article = {
      "ArticleId" : id,
      "title": title,
      "body": description
  }   
    axios.post('Article',article)
    .then(response => {
        dispatch({
        type: actionTypes.PUBLISH_ARTICLE,
        payload: response.data
      });

    })
    .catch(error => {
        console.error(error)
    });
  
    setOpen(false);
  }, [dispatch, id, title, description]);

  const handleClickOpen = useCallback(() => {
    setOpen(true);
  }, []);

  return (
    <div>
      <Button variant="outlined" color="primary" onClick={handleClickOpen}>
        Publish Article
      </Button>
      <Dialog open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
        <DialogContent>
          <DialogContentText>
            <h3>Edit Article</h3>
          </DialogContentText>
          <TextField
            autoFocus
            margin="dense"
            id="name"
            label="Title"
            type="text"
            fullWidth
            defaultValue={title}
            onChange={(evtArg) => setTitle(evtArg.target.value)}
          />
          <TextField
            autoFocus
            margin="dense"
            id="branch"
            label="Description"
            type="text"
            fullWidth
            multiline
            rows={5}
            defaultValue={description}
            onChange={(evtArg) => setDescription(evtArg.target.value)}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={onPublishArticle} color="primary">
            Publish
           {/* {(isEdit) ? "Edit": "Publish" } */}
          </Button>
          <Button onClick={handleClose} color="primary">
            Cancel
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
}
import React, { useCallback, useEffect } from 'react';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import { actionTypes } from '../Common/constants';


export default function FormDialog(props) {
  const { dispatch, editForm } = props;
  const [open, setOpen] = React.useState(false);
  const [id, setId] = React.useState(0);
  const [title, setTitle] = React.useState(null);
  const [description, setDescription] = React.useState(null);


  useEffect(() => {
    if (editForm) {
      setId(editForm.Id)
      setTitle(editForm.Title)
      setDescription(editForm.Description)
      setOpen(true)
    }
  }, [editForm])


  const handleClose = useCallback(() => {
    setOpen(false);
  }, []);

  const onPublishArticle = useCallback(() => {
    //TODO: Calling API
    
    dispatch({
      type: id === 0 ? actionTypes.PUBLISH_ARTICLE : actionTypes.UPDATE_ARTICLE,
      payload: {
        Id: id,
        Title: title,
        Description: description
      }
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
            <h3>Article</h3>
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
          </Button>
          <Button onClick={handleClose} color="primary">
            Cancel
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
}
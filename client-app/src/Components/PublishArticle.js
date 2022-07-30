import React, { useCallback, useEffect } from "react";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from '@material-ui/core//DialogTitle';
import { actionTypes } from "../Common/constants";
import axios from "../Common/axios-news";
import { RoleType } from "../Common/constants";

export default function FormDialog(props) {
  const { dispatch, editForm, role } = props;
  const [open, setOpen] = React.useState(false);
  const [id, setId] = React.useState(0);
  const [title, setTitle] = React.useState(null);
  const [description, setDescription] = React.useState(null);
  const [datePublished, setPublishedDate] = React.useState(null);

  useEffect(() => {
    // setPublishedDate(editForm.datePublished);
    if (editForm) {
      setId(editForm.id);
      setTitle(editForm.title);
      setDescription(editForm.body);
      setPublishedDate(editForm.datePublished);
      setOpen(true);
    } else {
      setId(0);
      setTitle(null);
      setDescription(null);
    }
  }, [editForm]);

  const handleClose = useCallback(() => {
    setOpen(false);
    dispatch({
      type: actionTypes.CLEAR_ARTICLE,
    });
  }, [dispatch]);

  const publishNewArticle = (article) => {
    const newArticle = {
      title: title,
      body: description,
    };
    axios
      .post("Article", newArticle)
      .then((response) => {
        dispatch({
          type: actionTypes.PUBLISH_ARTICLE,
          payload: response.data,
        });
      })
      .catch((error) => {
        console.error(error);
      });
  };

  const updateArticle = () => {
    const article = {
      id: id,
      title: title,
      body: description,
      datePublished: datePublished,
    };
    axios
      .put("Article", article)
      .then((response) => {
        dispatch({
          type: actionTypes.UPDATE_ARTICLE,
          payload: response.data,
        });
      })
      .catch((error) => {
        console.error(error);
      });
  };

  const articleEventHandler = () => {
    editForm ? updateArticle() : publishNewArticle();

    setOpen(false);
  };

  const handleClickOpen = useCallback(() => {
    dispatch({
      type: actionTypes.CLEAR_ARTICLE,
    });
    setOpen(true);
  }, [dispatch]);

  return (
    <div>
      {role === RoleType.PUBLISHER && (
        <Button variant="outlined" color="primary" onClick={handleClickOpen}>
          Publish Article
        </Button>
      )}

      <Dialog
        open={open}
        onClose={handleClose}
        aria-labelledby="form-dialog-title"
      >
		<DialogTitle>{
		role === RoleType.PUBLISHER && (
			editForm ? "Edit Article" : "Publish Article"
            )}
		</DialogTitle>
        <DialogContent>
          <DialogContentText>
			{ role === RoleType.PUBLISHER && (
				 editForm ? "Update Article " : "Add Title and Description "
			)}
			then click on Publish to save the changes
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
            disabled={role === RoleType.USER}
          />
          <TextField
            autoFocus
            margin="dense"
            id="branch"
            label="Description"
            type="text"
            fullWidth
            multiline
            minRows={5}
            defaultValue={description}
            onChange={(evtArg) => setDescription(evtArg.target.value)}
            disabled={role === RoleType.USER}
          />
        </DialogContent>
        <DialogActions>
          {role === RoleType.PUBLISHER && (
            <Button onClick={articleEventHandler} color="primary">
              {editForm ? "Save Changes" : "Publish"}
            </Button>
          )}
          <Button onClick={handleClose} color="primary">
            Cancel
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
}

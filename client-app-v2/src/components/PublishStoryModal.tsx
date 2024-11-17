import React, { useEffect, useState } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
} from "@mui/material";
import { IArticle } from "./Article";
import { post } from "../Common/axios-news";

interface IPublishStoryModalProps {
  open: boolean;
  handleClose: () => void;
  onPublishSuccess: (article : IArticle) => void;
}

const PublishStoryModal: React.FC<IPublishStoryModalProps> = ({
  open,
  handleClose,
  onPublishSuccess,
}) => {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("Not Required for the moment");
  const [body, setBody] = useState("");

  useEffect(() => {
    if (open) {
      setTitle("");
	  setBody("");
    }
  }, [open]);

  const handlePublish = async () => {
    const newArticle = {
      title,
      body
    };

	const response = await post<IArticle>('/article', newArticle);
    onPublishSuccess(response);
  };

  return (
    <Dialog
      open={open}
      onClose={handleClose}
      maxWidth="md"
      disableEnforceFocus
      disableAutoFocus
    >
      <DialogTitle>Publish New Story</DialogTitle>
      <DialogContent>
        <TextField
          label="Title"
          fullWidth
          margin="normal"
          required
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />
        <TextField
          label="Description"
          fullWidth
          margin="normal"
          value={description}
          InputProps={{
            readOnly: true,
          }}
          onChange={(e) => setDescription(e.target.value)}
        />
        <TextField
          label="Body"
          multiline
          rows={6}
          fullWidth
          margin="normal"
          required
          value={body}
          onChange={(e) => setBody(e.target.value)}
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} color="secondary">
          Cancel
        </Button>
        <Button onClick={handlePublish} color="primary">
          Publish
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default PublishStoryModal;

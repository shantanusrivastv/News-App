import React, { useState } from "react";
import { IArticle } from "./Article";
import Modal from "@mui/material/Modal";
import Box from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import dayjs from "dayjs";
import { put } from "../Common/axios-news";

interface IEditArticleModalProps {
  article: IArticle;
  onClose: () => void;
  onSave: (article: IArticle) => void;
}

const EditArticleModal: React.FC<IEditArticleModalProps> = ({
  article,
  onClose,
  onSave,
}) => {
  const [editedArticle, setEditedArticle] = useState<IArticle>(article);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setEditedArticle({ ...editedArticle, [name]: value });
  };

  const handleSave = async () => {
	let payload = {
		articleId : editedArticle.articleId,
		title: editedArticle.title,
		body: editedArticle.body
	};
	try {
		 const response = await put<IArticle>("/article", payload);
		 onSave(response);
	} catch (error) {
		 console.error("Failed to save the article", error);
	}
  };

  return (
    <Modal open={true} onClose={onClose}>
      <Box
        sx={{
          position: "absolute",
          top: "50%",
          left: "50%",
          transform: "translate(-50%, -50%)",
          width: 400,
          bgcolor: "background.paper",
          boxShadow: 24,
          p: 4,
        }}
      >
        <h2>Edit Article</h2>
        <TextField
          label="Title"
          name="title"
          value={editedArticle.title}
          onChange={handleChange}
          fullWidth
          margin="normal"
        />
        <TextField
          label="Body"
          name="body"
          value={editedArticle.body}
          onChange={handleChange}
          fullWidth
          margin="normal"
        />
        <TextField
          label="Author"
          name="author"
          value={editedArticle.author}
          InputProps={{
            readOnly: true,
          }}
          fullWidth
          margin="normal"
        />
        <TextField
          label="Date Published"
          name="datePublished"
          InputProps={{
            readOnly: true,
          }}
          value={dayjs(editedArticle.datePublished).format("DD MMM YYYY")}
          fullWidth
          margin="normal"
        />
        <Button variant="contained" color="primary" onClick={handleSave}>
          Save
        </Button>
        <Button variant="outlined" color="secondary" onClick={onClose}>
          Cancel
        </Button>
      </Box>
    </Modal>
  );
};

export default EditArticleModal;

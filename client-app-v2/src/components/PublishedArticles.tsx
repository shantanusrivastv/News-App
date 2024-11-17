import React, { useEffect, useState } from "react";
import { IArticle, UserReaction } from "./Article";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import Button from "@mui/material/Button";
import EditArticleModal from "./EditArticleModal";
import ThumbUpIcon from "@mui/icons-material/ThumbUp";
import ThumbDownIcon from "@mui/icons-material/ThumbDown";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import dayjs from "dayjs";
import { del } from "../Common/axios-news";

interface IPublishedArticlesProps {
  articles: IArticle[];
}

const PublishedArticles: React.FC<IPublishedArticlesProps> = ({ articles }) => {
  const [selectedArticle, setSelectedArticle] = useState<IArticle | null>(null);
  const [articleList, setArticleList] = useState<IArticle[]>(articles); // Local state for article list

  useEffect(() => {
    setArticleList(articles);
  }, [articles]);

  const handleEditClick = (article: IArticle) => {
    setSelectedArticle(article);
  };

  const handleRemove = async (article: IArticle) => {
    const response = await del(`/article/${article.articleId}`);
    if (response.status === 202) {
      const updatedArticles = articleList.filter(
        (art) => article.articleId !== art.articleId
      );
      setArticleList(updatedArticles);
    }
  };

  const onCloseHandler = () => {
    setSelectedArticle(null);
  };

  const onSaveHandler = (updatedArticle: IArticle) => {
    const updatedArticles = articleList.map((article) =>
      article.articleId === updatedArticle.articleId
        ? { ...article, ...updatedArticle }
        : article
    );

    setArticleList(updatedArticles);
    setSelectedArticle(null);
  };

  const toggleLike = (articleId: number) => {
    const updatedArticles = articleList.map((article) =>
      article.articleId === articleId
        ? {
            ...article,
            articleReaction:
              article.articleReaction === UserReaction.Like
                ? UserReaction.Dislike
                : UserReaction.Like,
          }
        : article
    );

    setArticleList(updatedArticles);
  };

  return (
    <div>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Article Id</TableCell>
              <TableCell>
                <strong>Title</strong>
              </TableCell>
              <TableCell>Description</TableCell>
              <TableCell>Author</TableCell>
              <TableCell>Date Published</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {articleList.map((article) => (
              <TableRow key={article.articleId}>
                <TableCell>{article.articleId}</TableCell>
                <TableCell>{article.title}</TableCell>
                <TableCell>{article.body}</TableCell>
                <TableCell>{article.author}</TableCell>
                <TableCell>
                  {dayjs(article.datePublished).format("DD MMM YY")}
                </TableCell>

                <TableCell>
                  <Button
                    variant="contained"
                    color="primary"
                    onClick={() => handleEditClick(article)}
                  >
                    <EditIcon />
                  </Button>
                  <Button
                    variant="outlined"
                    color="error"
                    onClick={() => handleRemove(article)}
                  >
                    <DeleteIcon />
                  </Button>
                  <Button onClick={() => toggleLike(article.articleId)}>
                    {article.articleReaction === UserReaction.Like ? (
                      <ThumbUpIcon />
                    ) : (
                      <ThumbDownIcon />
                    )}
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      {selectedArticle && (
        <EditArticleModal
          article={selectedArticle}
          onClose={onCloseHandler}
          onSave={onSaveHandler}
        />
      )}
    </div>
  );
};

export default PublishedArticles;

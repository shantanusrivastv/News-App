import React, { useEffect, useState } from "react";
import PublishedArticles from "../components/PublishedArticles";
import { IArticle } from "../components/Article";
import { get } from "../Common/axios-news";
import { Box, Container, Fab, Snackbar, Typography } from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import PublishStoryModal from "../components/PublishStoryModal";
import "./Dashboard.css";

interface DashboardProps {
  authorised: boolean;
  name: string | null;
}


//todo: Remove it in future most likely no need of mapping at Ui layer
//   const mapResponseToArticle = (response: any): IArticle => {
//     return {
//       articleId: response.id,
//       title: response.title,
//       body: response.body,
//       author: response.author,
//       datePublished: response.datePublished,
//       articleReaction: UserReaction.Like, // Default value or map from response if available
//     };
//   };

const Dashboard: React.FC<DashboardProps> = ({ authorised, name }) => {
  const [articles, setArticles] = useState<IArticle[]>([]);
  const [showPublishModal, setShowPublishModal] = useState<boolean>(false);
  const [isPublished, setIsPublished] = useState<boolean>(false);

  const handlePublishSuccess = (newArticle: IArticle) => {
    setArticles((prevArticle) => [...prevArticle, newArticle]);
    setShowPublishModal(false);
    setIsPublished(true); // Trigger Snackbar
  };

  useEffect(() => {
    const fetchArticles = async () => {
      const articles = await get<IArticle[]>("/article");
      setArticles(articles);
    };

    fetchArticles();
  }, []);

  return (
    <Container className="dashboard-root">
      <Typography variant="h6" component="h3">
        {authorised ? "Published Articles" : "Login to View"}
      </Typography>
      <Fab
        color="primary"
        aria-label="add"
        onClick={() => setShowPublishModal(true)}
        style={{ position: "fixed", bottom: 16, right: 16 }}
      >
        <AddIcon />
      </Fab>
      <PublishStoryModal
        open={showPublishModal}
        handleClose={() => setShowPublishModal(false)}
        onPublishSuccess={handlePublishSuccess}
      />
      <Snackbar
        open={isPublished}
        autoHideDuration={2000}
        onClose={() => setIsPublished(false)}
        message="Story published successfully!"
      />
      <Box className="dashboard-articles">
        <PublishedArticles articles={articles} />
      </Box>
    </Container>
  );
};
export default Dashboard;

namespace Pressford.News.Entities
{
    public class ArticleLikes
    {
        public int LikeId { get; set; }
        public int ArticleId { get; set; }
        public string UserName { get; set; }
    }
}
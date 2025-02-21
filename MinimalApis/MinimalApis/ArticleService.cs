namespace MinimalApis
{
    public class ArticleService
    {

        private List<Article> Articles = new List<Article>
        {
             new Article(1, "Marteaux"),
             new Article(2, "Scie"),
        };
        public List<Article> GetArticles ()=> Articles;

        public Article AddArticle(String titre)
        {
            var article = new Article(Articles.Max(a => a.Id + 1), titre);
            Articles.Add(article);
            return article;
        }
    }
}

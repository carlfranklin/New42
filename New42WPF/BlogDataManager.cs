using System.ServiceModel.Syndication;
using System.Xml;

public static class BlogDataManager
{
    private readonly static string url =
        "https://devblogs.microsoft.com/dotnet/category/maui/feed/";

    public static List<BlogPost> BlogPosts = new List<BlogPost>();

    /// <summary>
    /// Reads posts from the .NET MAUI Blog into the BlogPosts list
    /// </summary>
    /// <returns></returns>
    public static void GetBlogPosts()
    {
        var posts = new List<BlogPost>();

        var settings = new XmlReaderSettings();
        
        using var reader = XmlReader.Create(url);

        var feed = SyndicationFeed.Load(reader);

        foreach (var item in feed.Items)
        {
            var post = new BlogPost();

            // Publish Date
            post.PublishDate = item.PublishDate.DateTime;

            // Author. Use ElementExtensions to read the dc:creator tag
            var creators =
                item.ElementExtensions.ReadElementExtensions<string>
                ("creator", "http://purl.org/dc/elements/1.1/");
            if (creators != null && creators.Count > 0)
            {
                post.Author = creators.FirstOrDefault<string>();
            }

            // Title
            post.Title = item.Title.Text;

            // Description
            post.Description = item.Summary.Text;

            // Done. Add to list
            posts.Add(post);
        }

        // Refresh the list
        BlogPosts.Clear();
        BlogPosts.AddRange(posts);
    }
}
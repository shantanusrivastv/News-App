using Pressford.News.Model;

namespace Pressford.News.Services.Interfaces
{
    public interface IUserService
    {
        UserInfo Authenticate(Credentials credentials);
    }
}
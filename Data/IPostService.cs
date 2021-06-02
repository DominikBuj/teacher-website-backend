using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Entities;

namespace TeacherWebsiteBackEnd.Data
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetPosts();
        Task<Post?> GetPostById(int? id);
        Task<Post> AddPost(Post post);
        Task<Post> ReplacePost(Post post);
        void DeletePosts();
        Task<bool> DeletePostById(int id);
    }
}

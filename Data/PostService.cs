using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Helpers;

namespace TeacherWebsiteBackEnd.Data
{
    public class PostService : IPostService
    {
        private readonly DatabaseContext _context;

        public PostService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            List<Post> posts = await _context.Posts.ToListAsync();
            foreach (Post post in posts) post.Files = await _context.Files.Where(file => file.PostId == post.Id).ToListAsync();
            return posts;
        }

        public async Task<Post?> GetPostById(int? id)
        {
            Post? post = await _context.Posts.FirstOrDefaultAsync(post => post.Id == id);
            if (post != null) post.Files = await _context.Files.Where(file => file.PostId == post.Id).ToListAsync();
            return post;
        }

        public async Task<Post> AddPost(Post post)
        {
            EntityEntry<Post> _post = await _context.Posts.AddAsync(post);

            await _context.SaveChangesAsync();

            return _post.Entity;
        }

        private bool ShouldDetachFile(Post post, File file)
        {
            return ((file.PostId == post.Id) && (post.Files != null) ? !post.Files.Any(_file => _file.Id == file.Id) : true);
        }

        private void DetachPostFiles(Post post)
        {
            List<File> files = _context.Files.AsNoTracking().AsEnumerable().Where(file => ShouldDetachFile(post, file)).ToList();
            foreach (File file in files)
            {
                file.PostId = null;
                file.Post = null;
                _context.Entry(file).State = EntityState.Modified;
            }
        }

        public async Task<Post> ReplacePost(Post post)
        {
            DetachPostFiles(post);
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async void DeletePosts()
        {
            await _context.Files.LoadAsync();
            _context.Posts.RemoveRange(_context.Posts);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeletePostById(int id)
        {
            Post post = await _context.Posts.FirstOrDefaultAsync(post => post.Id == id);
            if (post == null) return false;

            await _context.Files.Where(file => file.PostId == post.Id).LoadAsync();
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

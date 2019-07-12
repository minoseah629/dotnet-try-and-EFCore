using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace testtry
{
    class Program
    {
        static void Main(string[] args)
        {
            var ctx = new BloggingContext();
            var blogService = new BlogService(ctx);
            blogService.Add("test");
            var one = blogService.Find("test");
            Console.WriteLine(one.First().BlogId);
        }
        
        public class BlogService
        {
            private BloggingContext _context;

            public BlogService(BloggingContext context)
            {
                _context = context;
            }

            public void Add(string url)
            {
                var blog = new BloggingContext.Blog { Url = url };
                _context.Blogs.Add(blog);
                _context.SaveChanges();
            }

            public IEnumerable<BloggingContext.Blog> Find(string term)
            {
                return _context.Blogs
                    .Where(b => b.Url.Contains(term))
                    .OrderBy(b => b.Url)
                    .ToList();
            }
        }
    }

    internal class BloggingContext : DbContext
    {
        public BloggingContext()
        { }

        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        { }
        public DbSet<Blog> Blogs { get; set; }

        #region OnConfiguring
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(databaseName: "Add_writes_to_database");
            }
        }
        #endregion
        public class Blog
        {
            public int BlogId { get; set; }
            public string Url { get; set; }
        }
    }
}

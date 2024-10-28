using MaqaletyCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaqaletyData.SqlEf
{
    public class AuthorPostsEntity : IDataHelper<AuthorPost>
    {
        private AppDbContext db;
        public AuthorPostsEntity() {
            db = new AppDbContext();
        }

        public int Add(AuthorPost table)
        {
            if (db.Database.CanConnect())
            {
                db.AuthorPosts.Add(table);
                db.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int delete(int id)
        {
            if (db.Database.CanConnect())
            {
                var AuthorPost = find(id);
                 db.AuthorPosts.Remove(AuthorPost);
                db.SaveChanges();
                return 1;
            }
            return 0;
        }

      

        public AuthorPost find(int id)
        {
           return db.AuthorPosts.Where(c => c.Id == id).FirstOrDefault();
        }

        public List<AuthorPost> GetAllData()
        {
            return db.AuthorPosts.ToList();


        }

        public List<AuthorPost> GetDataByUser(string id)
        {
            if (db.Database.CanConnect())
            {
                return db.AuthorPosts
                   .Where(x=>x.UserId==id).ToList();
            }
            return null;
        }

        public List<AuthorPost> Search(string SearchItem)
        {
            if (db.Database.CanConnect())
            {
                SearchItem = SearchItem.ToLower();
                string trimmedSearchItem = SearchItem.Trim();
                return db.AuthorPosts
                 .AsEnumerable() // Forces the rest of the query to run in memory

                   .Where(x
                     => x.FullName.Contains(SearchItem)
                   || x.AuthorId.ToString().ToLower().Contains(SearchItem)
                   || x.UserId.ToString().ToLower().Contains(SearchItem)
                   || x.PostDescription.ToLower().Contains(SearchItem)
                   || x.PostTitle.ToString().ToLower().Contains(SearchItem)
                   || x.CategoryId.ToString().ToLower().Contains(SearchItem)
                   || x.UserName.ToLower().Contains(SearchItem)
                   || x.PostCategory.ToLower().Contains(SearchItem)
                   || x.Id.ToString().ToLower().Contains(SearchItem)
                   || x.AddedTime.ToString().ToLower().Contains(SearchItem)

                   ).ToList();




                  

            }
            return null;
        }

        public int Update(AuthorPost table, int Id)
        {
           if (db.Database.CanConnect()){
                db.AuthorPosts.Update(table);
                db.SaveChanges();
                return 1;

            }
            else { return 0; }
        
        }
    }
}

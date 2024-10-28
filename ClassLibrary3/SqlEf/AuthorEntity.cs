using MaqaletyCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaqaletyData.SqlEf
{

    public class AuthorEntity : IDataHelper<Author>
    {
        private AppDbContext db;
        public AuthorEntity()
        {
            db = new AppDbContext();
        }

        public int Add(Author table)
        {
            if (db.Database.CanConnect())
            {
                db.Author.Add(table);
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
                var author = find(id);
                db.Author.Remove(author);
                db.SaveChanges();
                return 1;
            }
            return 0;
        }

     

        public Author find(int id)
        {
            return db.Author.Where(c => c.Id == id)
               . FirstOrDefault()
                ;
        }

        public List<Author> GetAllData()
        {
            return db.Author.ToList();


        }

        public List<Author> GetDataByUser(string id)
        {
            throw new NotImplementedException();
        }

        public List<Author> Search(string searchItem)
        {

            

            if (db.Database.CanConnect())
            {
                // Using ToLower for case-insensitive search
                searchItem = searchItem.ToLower();
                string trimmedSearchItem = searchItem.Trim();


                return db.Author
                    .Where(x =>
                        ( x.FullName.Contains(trimmedSearchItem)) ||
                        ( x.Twiter.ToLower().Contains(trimmedSearchItem)) ||
                        (x.Instgram.ToLower().Contains(trimmedSearchItem)) ||
                        ( x.FaceBook.ToLower().Contains(trimmedSearchItem)) ||
                        ( x.Bio.ToLower().Contains(trimmedSearchItem)) ||
                        ( x.UserName.Contains(trimmedSearchItem)))
                    .ToList();
            }

            return new List<Author>(); // Return an empty list if unable to connect
        }



            public int Update(Author table, int Id)
        {
            if (db.Database.CanConnect())
            {
                var author = find(Id);
                author.FullName = table.FullName;
                author.UserName = table.UserName;
                author.Bio = table.Bio;
                author.PictureImageUrl = table.PictureImageUrl;
                author.Twiter = table.Twiter;
                author.FaceBook = table.FaceBook;
                author.Instgram = table.Instgram;
                db.Author.Update(author);
                db.SaveChanges();
                return 1;

            }
            else { return 0; }

        }

    }
}

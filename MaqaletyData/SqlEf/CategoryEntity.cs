using MaqaletyCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaqaletyData.SqlEf
{
    public class CategoryEntity : IDataHelper<Category>
    {
        private AppDbContext db;
        public CategoryEntity() {
            db = new AppDbContext();
        }

        public int Add(Category table)
        {
            if (db.Database.CanConnect())
            {
                db.Category.Add(table);
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
                var category = find(id);
                 db.Category.Remove(category);
                db.SaveChanges();
                return 1;
            }
            return 0;
        }

      

        public Category find(int id)
        {
           return db.Category.Where(c => c.Id == id).FirstOrDefault();
        }

        public List<Category> GetAllData()
        {
            return db.Category.ToList();


        }

        public List<Category> GetDataByUser(int id)
        {
            throw new NotImplementedException();
        }

        public List<Category> Search(string SearchItem)
        {
            if (db.Database.CanConnect())
            {
                 return db.Category
                    .Where(x=>x.Name.Contains(SearchItem)).ToList();
            }
            return null;
        }

        public int Update(Category table, int Id)
        {
           if (db.Database.CanConnect()){
                var category = find(Id);
                category.Name = table.Name;
                db.Category.Update(category);
                db.SaveChanges();
                return 1;

            }
            else { return 0; }
        
        }
    }
}

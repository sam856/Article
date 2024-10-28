using MaqaletyCore;
using MaqaletyData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Maqalety.Pages
{
    public class AllUserModel : PageModel
    {
        public IDataHelper<MaqaletyCore.Author> DataHelperForUser { get; }
        public List<MaqaletyCore.Author> ListOfAuthor { get; set; }

        public int NoOfItem { get; set; } = 10;
        public int CurrentPage { get; set; } = 1; // Track current page
        public int TotalPages { get; set; } // Total number of pages

        public AllUserModel(IDataHelper<MaqaletyCore.Author> dataHelperForUser)
        {
            DataHelperForUser = dataHelperForUser;
            ListOfAuthor = new List<MaqaletyCore.Author>();
        }

        public void OnGet(string loadState, string search, int id, int currentPage = 1)
        {
            CurrentPage = currentPage; // Update current page

            if (loadState == "search")
            {
                SearchItem(search);
            }
            else if (loadState == "Next")
            {
                GetNextData(id);
            }
            else if (loadState == "Previous")
            {
                GetPreviousData(id);
            }
            else
            {
                GetAllAuthors(); // Default action to get all authors
            }
        }

        private void GetAllAuthors()
        {
            var allAuthors = DataHelperForUser.GetAllData().ToList();
            TotalPages = (int)Math.Ceiling((double)allAuthors.Count / NoOfItem);
            ListOfAuthor = allAuthors.Skip((CurrentPage - 1) * NoOfItem).Take(NoOfItem).ToList();
        }

        private void SearchItem(string search)
        {
            ListOfAuthor = DataHelperForUser.GetAllData()
                .Where(a => (a.FullName != null && a.FullName.Contains(search)) ||
                            (a.Bio != null && a.Bio.Contains(search)))
                .ToList();
        }

        private void GetNextData(int id)
        {
            var allAuthors = DataHelperForUser.GetAllData().ToList();
            var index = allAuthors.FindIndex(x => x.Id == id);

            if (index >= 0 && index + 1 < allAuthors.Count)
            {
                ListOfAuthor = allAuthors.Skip(index + 1).Take(NoOfItem).ToList();
                CurrentPage++; // Move to the next page
            }
            else
            {
                ListOfAuthor = new List<MaqaletyCore.Author>();
            }
        }

        private void GetPreviousData(int id)
        {
            var allAuthors = DataHelperForUser.GetAllData().ToList();
            var index = allAuthors.FindIndex(x => x.Id == id);

            if (index > 0)
            {
                ListOfAuthor = allAuthors.Skip(index - NoOfItem).Take(NoOfItem).ToList();
                CurrentPage--; // Move to the previous page
            }
            else
            {
                ListOfAuthor = new List<MaqaletyCore.Author>();
            }
        }
    }
}

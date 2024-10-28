using MaqaletyCore;
using MaqaletyData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Maqalety.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public IDataHelper<MaqaletyCore.Category> DataHelperForCategory { get; }
        public IDataHelper<MaqaletyCore.AuthorPost> DataHelperForAuthorPost { get; }

        public List<MaqaletyCore.Category> ListOfCategory { get; set; }
        public List<MaqaletyCore.AuthorPost> ListOfAuthorPost { get; set; }
        public int NoOfItem { get; set; } = 10;
        public int CurrentPage { get; set; } = 1; // Add this line
        public int TotalPages { get; set; } // Add this line

        public IndexModel(ILogger<IndexModel> logger,
            IDataHelper<MaqaletyCore.Category> dataHelperForCategory,
            IDataHelper<MaqaletyCore.AuthorPost> dataHelperForAuthorPost)
        {
            _logger = logger;
            DataHelperForCategory = dataHelperForCategory;
            DataHelperForAuthorPost = dataHelperForAuthorPost;
            ListOfCategory = new List<MaqaletyCore.Category>();
            ListOfAuthorPost = new List<MaqaletyCore.AuthorPost>();
        }

        public void OnGet(string LoadState, string CategoryName, string search, int id, int currentPage = 1)
        {
            CurrentPage = currentPage; // Update current page
            if (LoadState == null || LoadState == "All")
            {
                GetAllPosts();
            }
            else if (LoadState == "ByCategory")
            {
                GetByCategory(CategoryName);
            }
            else if (LoadState == "search")
            {
                SearchItem(search);
            }
            else if (LoadState == "Next")
            {
                GetNextData(id);
            }
            else if (LoadState == "Previous") // Handle previous
            {
                GetPreviousData(id);
            }

            GetAllCategories();
        }

        private void GetAllCategories()
        {
            ListOfCategory = DataHelperForCategory.GetAllData();
        }

        private void GetAllPosts()
        {
            var allPosts = DataHelperForAuthorPost.GetAllData().ToList();
            TotalPages = (int)Math.Ceiling((double)allPosts.Count / NoOfItem);
            ListOfAuthorPost = allPosts.Skip((CurrentPage - 1) * NoOfItem).Take(NoOfItem).ToList();
        }

        private void GetByCategory(string categoryname)
        {
            ListOfAuthorPost = DataHelperForAuthorPost.GetAllData()
                .Where(p => p.PostCategory == categoryname)
                .Take(NoOfItem).ToList();
        }

        private void SearchItem(string search)
        {
            ListOfAuthorPost = DataHelperForAuthorPost.GetAllData()
                .Where(p => p.PostTitle.Contains(search) || p.PostDescription.Contains(search)).Take(NoOfItem).ToList();
        }

        private void GetNextData(int id)
        {
            var allPosts = DataHelperForAuthorPost.GetAllData().ToList();
            var index = allPosts.FindIndex(x => x.Id == id);

            if (index >= 0 && index + 1 < allPosts.Count)
            {
                ListOfAuthorPost = allPosts.Skip(index + 1).Take(NoOfItem).ToList();
                CurrentPage++; // Move to the next page
            }
            else
            {
                ListOfAuthorPost = new List<MaqaletyCore.AuthorPost>();
            }
        }

        private void GetPreviousData(int id)
        {
            var allPosts = DataHelperForAuthorPost.GetAllData().ToList();
            var index = allPosts.FindIndex(x => x.Id == id);

            if (index > 0)
            {
                ListOfAuthorPost = allPosts.Skip(index - NoOfItem).Take(NoOfItem).ToList();
                CurrentPage--; 
            }
            else
            {
                ListOfAuthorPost = new List<MaqaletyCore.AuthorPost>();
            }
        }

    }
}

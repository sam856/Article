using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaqaletyCore;
using MaqaletyData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Maqalety.Pages
{
    public class ArticleModel : PageModel
    {

        private readonly IDataHelper<AuthorPost> dataHelperForPost;
        public AuthorPost authorPost;

        public ArticleModel(IDataHelper<AuthorPost> dataHelperForPost)

        {
            this.dataHelperForPost = dataHelperForPost;
            authorPost = new AuthorPost();
        }



        public void OnGet()
        {
            var id = HttpContext.Request.RouteValues["id"];
            authorPost = dataHelperForPost.find(Convert.ToInt32(id));
        }
    }
}

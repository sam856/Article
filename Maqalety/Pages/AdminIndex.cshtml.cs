using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using MaqaletyCore;
using MaqaletyData;

namespace Maqalety.Pages
{

    [Authorize]
    public class AdminIndexModel : PageModel

    {
        private readonly IDataHelper<AuthorPost> dataHelper;
        public int Allpost { get; set; }
        public int PostLastMonth { get; set; }
        public int PostLastYear { get; set; }
        public AdminIndexModel(IDataHelper<AuthorPost> dataHelper)
        {
            this.dataHelper = dataHelper;
        }
        public void OnGet()
        {
            var date = DateTime.Now.AddMonths(-1);
            var DateOfYear= DateTime.Now.AddYears(-1);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Allpost = dataHelper.GetDataByUser(userId).Count;
            PostLastMonth = dataHelper.GetDataByUser(userId).Where(x => x.AddedTime >=date).Count();
            PostLastYear = dataHelper.GetDataByUser(userId).Where(x => x.AddedTime >= date).Count();




        }
    }
}

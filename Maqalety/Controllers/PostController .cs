using Maqalety.Code;
using MaqaletyCore;
using MaqaletyData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Maqalety.Controllers
{
    [Authorize]
    [Route("Post")]
    public class PostController : Controller
    {
        private readonly IDataHelper<AuthorPost> dataHelper;
        private readonly IWebHostEnvironment webHost;
        private readonly FilesHelper filesHelper;
        private string UserId;
        private readonly IDataHelper<Author> dataHelperforUser;
        private readonly IDataHelper<Category> dataHelperforCategory;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IAuthorizationService authorizationService;
        private Task<AuthorizationResult> result;

        public PostController(
            IDataHelper<AuthorPost> dataHelper,
            IDataHelper<Author> dataHelperforUser,
            IWebHostEnvironment webHost,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IAuthorizationService authorizationService,
            IDataHelper<Category> dataHelperforCategory
        )
        {
            this.dataHelper = dataHelper;
            this.dataHelperforUser = dataHelperforUser;
            this.webHost = webHost;
            this.filesHelper = new Code.FilesHelper(this.webHost);
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.authorizationService = authorizationService;
            this.dataHelperforCategory = dataHelperforCategory;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public async Task<ActionResult> Index(int? id, int page = 1, bool isPrevious = false)
        {
            await SetUser();
            const int PAGEItem = 5; // Number of items per page

            if (result.Result.Succeeded)
            {
                var allData = dataHelper.GetAllData().OrderBy(x => x.Id).ToList();

                if (isPrevious && page > 1)
                {
                    page--;
                }

                var paginatedData = allData.Skip((page - 1) * PAGEItem).Take(PAGEItem).ToList();

                ViewBag.Page = page;
                ViewBag.HasPrevious = page > 1;
                ViewBag.HasNext = paginatedData.Count == PAGEItem && (page * PAGEItem) < allData.Count;

                return View(paginatedData);
            }
            else
            {
                var allData = dataHelper.GetDataByUser(UserId).OrderBy(x => x.Id).ToList();

                if (isPrevious && page > 1)
                {
                    page--;
                }

                var paginatedData = allData.Skip((page - 1) * PAGEItem).Take(PAGEItem).ToList();

                ViewBag.Page = page;
                ViewBag.HasPrevious = page > 1;
                ViewBag.HasNext = paginatedData.Count == PAGEItem && (page * PAGEItem) < allData.Count;

                return View(paginatedData);
            }
        }

        [HttpGet]
        [Route("Details")]
        public ActionResult Details(int id)
        {
            return View(dataHelper.find(id));
        }

        [Route("Create")]
        public ActionResult Create()
        {
            SetUser();
            return View();
        }

        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VoreView.AuthorPostView collection)
        {
            SetUser();

            try
            {
                var post = new AuthorPost
                {
                    AddedTime = DateTime.Now,
                    Author = collection.Author,
                    CategoryId = dataHelperforCategory.GetAllData()
                        .Where(x => x.Name == collection.PostCategory)
                        .Select(x => x.Id).FirstOrDefault(),
                    Category = collection.Category,
                    AuthorId = dataHelperforUser.GetAllData()
                        .Where(x => x.UserId == UserId)
                        .Select(x => x.Id).FirstOrDefault(),
                    FullName = dataHelperforUser.GetAllData()
                        .Where(x => x.UserId == UserId)
                        .Select(x => x.FullName).FirstOrDefault(),
                    PostCategory = collection.PostCategory,
                    PostDescription = collection.PostDescription,
                    PostTitle = collection.PostTitle,
                    UserId = UserId,
                    UserName = dataHelperforUser.GetAllData()
                        .Where(x => x.UserId == UserId)
                        .Select(x => x.UserName).FirstOrDefault(),
                    PostImageUrl = filesHelper.UploadFile(collection.PostImageUrl, "img")
                };
                dataHelper.Add(post);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $" Inner Exception: {ex.InnerException.Message}";
                }
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(collection);
            }
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public ActionResult Edit(int id)
        {

            var collection = dataHelper.find(id);
            var author = new VoreView.AuthorPostView
            {


                AddedTime =collection.AddedTime,
                Author = collection.Author,
                CategoryId = collection.CategoryId,
                Category = collection.Category,
                AuthorId = collection.AuthorId,
                FullName = collection.FullName,
                PostCategory = collection.PostCategory,
                PostDescription = collection.PostDescription,
                PostTitle = collection.PostTitle,
                UserId = collection.UserId,
                UserName =collection.UserName,
                Id=collection.Id
            };
            return View(author);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, VoreView.AuthorPostView collection)
        {
            SetUser();

            try
            {
                var post = new AuthorPost
                {
                    AddedTime = DateTime.Now,
                    Author = collection.Author,
                    CategoryId = dataHelperforCategory.GetAllData()
                        .Where(x => x.Name == collection.PostCategory)
                        .Select(x => x.Id).FirstOrDefault(),
                    Category = collection.Category,
                    AuthorId = dataHelperforUser.GetAllData()
                        .Where(x => x.UserId == UserId)
                        .Select(x => x.Id).FirstOrDefault(),
                    FullName = dataHelperforUser.GetAllData()
                        .Where(x => x.UserId == UserId)
                        .Select(x => x.FullName).FirstOrDefault(),
                    PostCategory = collection.PostCategory,
                    PostDescription = collection.PostDescription,
                    PostTitle = collection.PostTitle,
                    UserId = UserId,
                    UserName = dataHelperforUser.GetAllData()
                        .Where(x => x.UserId == UserId)
                        .Select(x => x.UserName).FirstOrDefault(),
                    PostImageUrl = filesHelper.UploadFile(collection.PostImageUrl, "img"),
                    Id=id
                };
                dataHelper.Update(post,id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $" Inner Exception: {ex.InnerException.Message}";
                }
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

       
            [HttpGet]
            [Route("Search")]
            public ActionResult Search(string searchItem)
            {
                // Handle null or empty search input
                if (string.IsNullOrWhiteSpace(searchItem))
                {
                    return RedirectToAction("Index");
                }

                var results = dataHelper.Search(searchItem);

                ViewBag.Page = 1; // Reset to the first page for new search results
                ViewBag.HasPrevious = false; // No previous page on first page
                ViewBag.HasNext = results.Count == 10; // Adjust if you change the number of items per page

                // Return the view with search results
                return View("Index", results);
            }

            [HttpGet]
        [Route("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            SetUser();
            var collection = dataHelper.find(id);
           
            return View(collection);
        }

        [HttpPost]
        [Route("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, VoreView.AuthorPostView collection)
        {
            try
            {
                dataHelper.delete(id);
                var filepath = "~/img" + collection.PostImageUrl;
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task SetUser()
        {
            result =  authorizationService.AuthorizeAsync(User, "Admin");
            UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}

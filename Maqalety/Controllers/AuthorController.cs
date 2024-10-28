using Maqalety.Code;
using Maqalety.VoreView;
using MaqaletyCore;
using MaqaletyData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace Maqalety.Controllers
{
    [Route("Author")]

    public class AuthorController : Controller
    {
        private readonly IDataHelper<Author> dataHelper;
        private readonly IWebHostEnvironment webHost;
        private readonly Code.FilesHelper filesHelper;

        public IAuthorizationService AuthorizationService { get; }

        public AuthorController(IDataHelper<Author> dataHelper,
            IAuthorizationService authorizationService,
            IWebHostEnvironment webHost)
        {
            this.dataHelper=dataHelper;
            AuthorizationService = authorizationService;
            this.webHost = webHost;
            filesHelper = new Code.FilesHelper(this.webHost);
        }
        // GET: AuthorController
        [HttpGet]
        [Authorize("Admin")]

        [Route("Index")]
        public ActionResult Index(int? id, int page = 1, bool isPrevious = false)
        {
            const int PAGEItem = 10;
            var allData = dataHelper.GetAllData();
                

            if (isPrevious && page > 1)
            {
                page--;
            }

            var paginatedData = allData.Skip((page - 1) * PAGEItem)
                .Take(PAGEItem).ToList();

            ViewBag.Page = page;
            ViewBag.HasPrevious = page > 1;
            ViewBag.HasNext = paginatedData.Count == PAGEItem
                && (page * PAGEItem) < allData.Count;

            return View(paginatedData);
        }
        public ActionResult Details(int id)
        {
            return View();
        }


        [HttpGet]
        [Route("Create")]
        public ActionResult Create()
        {
            return View();
        }

        [Route("Create")]

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        [Authorize]

        [Route("Edit/{id}")]
        public ActionResult Edit(int id)
        {
            var collection = dataHelper.find(id);
            VoreView.AuthorModelView authorveiw = new VoreView.AuthorModelView
            {
                Id = collection.Id,
                FullName = collection.FullName,
                Bio = collection.Bio,
                Instgram = collection.Instgram,
                FaceBook = collection.FaceBook,
                UserName = collection.UserName,
                UserId = collection.UserId,
                Twiter = collection.Twiter,
              
            };

            return View(authorveiw);
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [Route("Edit/{id}")]

        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, VoreView.AuthorModelView collection)
        {
            try
            {

                var author = new Author
                {


                    Id = collection.Id,
                    FullName = collection.FullName,
                    Bio = collection.Bio,
                    Instgram = collection.Instgram,
                    FaceBook = collection.FaceBook,
                    UserName = collection.UserName,
                    UserId = collection.UserId,
                    Twiter = collection.Twiter,
                    PictureImageUrl = filesHelper
                           .UploadFile(collection.PictureImageUrl,"img")


                };

                dataHelper.Update(author, id);
                var result = AuthorizationService.AuthorizeAsync(User, "Admin");
                if (result.Result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));


                }
                else
                {

                    return Redirect("/AdminIndex");
                }
            }
            catch(Exception EX)
            {
                return View();
            }
        }
        [Authorize("Admin")]
        [HttpGet]
        [Route("Search")]
        public ActionResult Search(string searchItem)
        {
            // Handle null or empty search input
            if (searchItem==null)
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
        [Authorize("Admin")]

        [Route("Delete/{id}")]
        // GET: AuthorController/Delete/5
        public ActionResult Delete(int id)
        {
            var collection = dataHelper.find(id);

            VoreView.AuthorModelView authorveiw = new VoreView.AuthorModelView
            {
                Id = collection.Id,
                FullName = collection.FullName,
                Bio = collection.Bio,
                Instgram = collection.Instgram,
                FaceBook = collection.FaceBook,
                UserName = collection.UserName,
                UserId = collection.UserId,
                Twiter = collection.Twiter,

            };

            return View(authorveiw);
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [Route("Delete/{id}")]
        [Authorize("Admin")]


        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Author collection)
        {
            try
            {
                dataHelper.delete(id);
                var filepath = "~/img" + collection.PictureImageUrl;
                if(System.IO.File.Exists(filepath))
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
    }
}

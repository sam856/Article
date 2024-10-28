using MaqaletyCore;
using MaqaletyData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Maqalety.Controllers
{
    [Route("Category")]
    public class CategoryController : Controller
    {
        private readonly IDataHelper<Category> _dataHelper;

        public CategoryController(IDataHelper<Category> dataHelper)
        {
            this._dataHelper = dataHelper;
        }
        [HttpGet]
        [Route("")]
        [Route("Index")]
        public ActionResult Index(int? id, int page = 1, bool isPrevious = false)
        {
            const int PAGEItem = 10; // Number of items per page

            // Fetch the data
            var allData = _dataHelper.GetAllData().OrderBy(x => x.Id).ToList();

            // If "Previous" is clicked, reduce the page number
            if (isPrevious && page > 1)
            {
                page--;
            }

            var paginatedData = allData.Skip((page - 1) * PAGEItem).Take(PAGEItem).ToList();

            ViewBag.Page = page;
            ViewBag.HasPrevious = page > 1; // Will be `false` if `page <= 1`
            ViewBag.HasNext = paginatedData.Count == PAGEItem && (page * PAGEItem) < allData.Count; // Will be `false` if there are no more items

            return View(paginatedData);
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

            var results = _dataHelper.Search(searchItem);

            ViewBag.Page = 1; // Reset to the first page for new search results
            ViewBag.HasPrevious = false; // No previous page on first page
            ViewBag.HasNext = results.Count == 10; // Adjust if you change the number of items per page

            // Return the view with search results
            return View("Index", results);
        }

        [HttpGet]
        [Route("Create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Create")]
        public ActionResult Create(Category collection)
        {
            try
            {
                int result = _dataHelper.Add(collection);
                if (result == 1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public ActionResult Edit(int id)
        {
            return View(_dataHelper.find(id));
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public ActionResult Edit(int id, Category collection)
        {
            try
            {
                int result = _dataHelper.Update(collection, id);
                if (result == 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            return View(_dataHelper.find(1));
        }

        [HttpPost]
        [Route("Delete/{id}")]
        public ActionResult Delete(int id, Category collection)
        {
            try
            {
                var result = _dataHelper.delete(id);
                if (result == 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}

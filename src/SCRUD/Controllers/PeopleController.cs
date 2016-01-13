/*
	Created By Gene Kochanowsky	
*/
using System.Linq;
using System.Security.Principal;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using SCRUD.Models;

namespace SCRUD.Controllers
{
    public class PeopleController : Controller, IControllerContext
    {
        private SCRUDContext _context;
		private IRepository_Person _db;

		public IPrincipal UserPrincipal { get { return this.User; } }

		public PeopleController(SCRUDContext context)
        {
            _context = context;
			_db = new Repository_Person(context, this);
        }

        // GET: People
        public IActionResult Index()
        {
			return View();
        }

		public ActionResult Search(int id = 0, string formView = null, bool includeResults = true, string funcRefresh = null, string funcSelect = null)
		{
			var dto = new SearchDTO_Person { FormView = formView };

			dto = includeResults ? _db.Search(dto) : null;

			return PartialView(dto);
		}

		[HttpPost]
		public ActionResult Search(SearchDTO_Person search)
		{
			ModelState.Clear();
			return PartialView(search.entireView ? "Search" : "SearchResults", _db.Search(search));
		}

		// GET: People/Details/5
		public IActionResult Details(int? id, string formView = null)
        {
			if (id == null || formView == null)
				return HttpNotFound();

			var dto = _db.Find(id.GetValueOrDefault());
			if (dto == null)
				return HttpNotFound();

			dto.formView = formView;
			return PartialView(dto);
		}

		public IActionResult CreateOrEdit(int? id = null, string formView = null, string funcRefresh = null)
		{
			Person dto;
			if (id.HasValue)
			{
				dto = _db.Find(id.GetValueOrDefault());
				if (dto == null) return HttpNotFound();
				dto.formView = formView;
				dto.funcRefresh = funcRefresh;
			}
			else
				dto = _db.New(formView, null);

			return PartialView(dto);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CreateOrEdit(Person dto)
		{
			if (ModelState.IsValid && _db.CreateOrEdit(dto, this.ActionContext))
				return Content("SUCCESS");

			return PartialView(dto);
		}

		// GET: People/Delete/5
		[ActionName("Delete")]
        public IActionResult Delete(int id, string formView)
        {
            Person person = _db.Find(id);
            if (person == null)
                return HttpNotFound();
			person.formView = formView;
            return PartialView(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string formView)
        {
			if (_db.Delete(id))
				return Content("SUCCESS");

			var dto = _db.Find(id);
			dto.formView = formView;
			return PartialView("Delete", dto);
		}
    }
}

/*
	Created By Gene Kochanowsky	

	All I ask is that you say who you stole this from.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using SCRUD.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

namespace SCRUD.ViewComponents
{
//	[Authorize]
//	[Authorize(Policy = "Testing")]
//	[Authorize(Roles = "role-i-am-in")]
	public class SearchPersonViewComponent : ViewComponent, IControllerContext
	{
		private SCRUDContext _dbContext;
		private IRepository_Person rep;

		public IPrincipal UserPrincipal { get { return this.User; } }

		public SearchPersonViewComponent(SCRUDContext dbContext)
		{
			_dbContext = dbContext;
			this.rep = new Repository_Person(dbContext, controllerContext: this);
		}

		public IViewComponentResult Invoke(string formView)
		{
			var dto = new SearchDTO_Person() { FormView = formView };
			return View(rep.Search(dto));
		}
	}
}

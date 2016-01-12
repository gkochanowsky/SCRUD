﻿/*
	Created By Gene Kochanowsky	
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using SCRUD.Models;
using Microsoft.AspNet.Mvc;

namespace SCRUD.ViewComponents
{
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

		public IViewComponentResult Invoke()
		{
			var dto = new SearchDTO_Person();
			return View(rep.Search(dto));
		}
	}
}

/*
	Created By Gene Kochanowsky	
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;

namespace SCRUD.Models
{
	public class Repository_Person : IRepository_Person
	{
		SCRUDContext _db;
		IControllerContext _controllerContext;

		private string User { get { return Utilities.BaseUserName(_controllerContext.UserPrincipal.Identity); } }

		public Repository_Person(SCRUDContext db, IControllerContext controllerContext)
		{
			_db = db;
			_controllerContext = controllerContext;
		}

		public IQueryable<Person> All
		{
			get
			{
				return _db.Person.Include(r => r.Gender);
			}
		}

		public Person New(string formView, string funcRefresh)
		{
			return new Person
			{
				formView = formView,
				funcRefresh = funcRefresh
			};
		}

		public Person Find(int id)
		{
			return All.FirstOrDefault(r => r.PersonId == id);
		}

		public SearchDTO_Person Search(SearchDTO_Person search)
		{
			try
			{
				search.FirstName = string.IsNullOrWhiteSpace(search.FirstName) ? null : search.FirstName;
				search.LastName = string.IsNullOrWhiteSpace(search.LastName) ? null : search.LastName;
				search.DoB_From = search.DoB_From.GetValueOrDefault() == default(DateTime) ? null : search.DoB_From;
				search.DoB_To = search.DoB_To.GetValueOrDefault() == default(DateTime) ? null : search.DoB_To;
				search.GenderID = search.GenderID.GetValueOrDefault() == default(int) ? null : search.GenderID;

				var result = from r in All
							 where (search.FirstName == null || r.FirstName.Contains(search.FirstName))
								&& (search.LastName == null || r.LastName.Contains(search.LastName))
								&& (search.DoB_From == null || search.DoB_From <= r.DoB )
								&& (search.DoB_To == null || r.DoB <= search.DoB_To)
								&& (search.GenderID == null || r.GenderID == search.GenderID)
							 select r;

				search.recCount = result.Count();

				if (search.Orderings.ContainsKey(search.sort))
				// Apply sort order
				{
					var applyOrdering = search.Orderings[search.sort];
					result = applyOrdering(result, search.sortDir == "ASC");
				}
				else
				{
					_controllerContext.ModelState.AddModelError(string.Empty, $"column sort for {search.sort} not implemented");
				}

				// Take page of data
				var recs = result.Skip(search.skip).Take(search.recsPerPage);
				search.results = recs.ToList();
			}
			catch (Exception ex)
			{
				Utilities.LogException(_controllerContext, ex);
				search.results = new List<Person>();
			}

			return search;
		}

		public bool CreateOrEdit(Person dto, ActionContext ac = null)
		{
			try
			{
				if (dto.PersonId == 0)
					_db.Person.Add(dto);
				else
					_db.Update(dto);

				return _db.SaveChanges() > 0;
			}
			catch (Exception ex)
			{
				return Utilities.LogException(_controllerContext, ex);
			}
		}

		public bool Delete(int id)
		{
			try
			{
				Person dto = Find(id);
				_db.Person.Remove(dto);
				return _db.SaveChanges() > 0;
			}
			catch (Exception ex)
			{
				return Utilities.LogException(_controllerContext, ex);
			}
		}
	}

	public interface IRepository_Person
	{
		IQueryable<Person> All { get; }
		Person New(string formView, string funcRefresh);
		Person Find(int id);
		bool CreateOrEdit(Person dto, ActionContext ac);
		bool Delete(int id);
		SearchDTO_Person Search(SearchDTO_Person dto);
	}
}

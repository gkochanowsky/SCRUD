/*
	Created By Gene Kochanowsky	
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Rendering;

namespace SCRUD.Models
{
    public class SelectLists
    {
		private SCRUDContext _context;

		public SelectLists(SCRUDContext context)
		{
			_context = context;
		}
		public SelectList Gender(int? id = null)
		{
			return new SelectList(_context.Gender.OrderBy(g => g.Code), "GenderId", "Code", id);
		}
    }
}

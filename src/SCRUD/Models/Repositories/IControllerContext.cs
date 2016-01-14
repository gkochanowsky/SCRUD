/*
	Created By Gene Kochanowsky	

	All I ask is that you say who you stole this from.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace SCRUD.Models
{
    public interface IControllerContext
    {
		ModelStateDictionary ModelState { get; }

		IPrincipal UserPrincipal { get; } 
	}
}

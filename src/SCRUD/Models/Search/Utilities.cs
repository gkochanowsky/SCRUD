/*
	Created By Gene Kochanowsky	
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace SCRUD.Models
{
	public class Utilities
	{
		/// <summary>
		/// Return the name of the user without domain information.
		/// </summary>
		public static string BaseUserName(IIdentity identity)
		{
			return identity == null || string.IsNullOrWhiteSpace(identity.Name) ? "Unknown" : identity.Name.Split('\\').Last().Trim();
		}

		public static bool LogException(IControllerContext err, Exception ex, string loc = null)
		{
			string errMsg = (string.IsNullOrWhiteSpace(loc) ? "" : "Location: " + loc + " - ") + ((ex == null) ? "" : ex.Message + ((ex.InnerException == null) ? "" : " - " + ex.InnerException.Message));

			var msgs = ex.Message.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
			msgs.ForEach(m => { if (!m.Contains("See the inner exception for details")) err.ModelState.AddModelError(string.Empty, m); });

			if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
			{
				var exmsgs = ex.InnerException.Message.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
				exmsgs.ForEach(m => { err.ModelState.AddModelError(string.Empty, m); });
			}

			return false;
		}
	}
}

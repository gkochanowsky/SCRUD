/*
	Created By Gene Kochanowsky	
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;

namespace SCRUD.Helpers
{
	public static class SearchSortResultsHelper
	{
		public static HtmlString SearchSortResult<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string formID)
		{
			var tag = $@"<a class=""link-default"" onclick=""return SortSearchResults('{htmlHelper.DisplayNameFor(expression)}', '{formID}');"">{htmlHelper.DisplayNameFor(expression)}</a>";
			return new HtmlString(tag);
		}
	}
}

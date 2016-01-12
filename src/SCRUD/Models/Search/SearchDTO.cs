/*
	Created By Gene Kochanowsky	
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SCRUD.Models
{
	public class SearchDTO
	{
		/// <summary>
		/// Default records per page
		/// </summary>
		private const int defaultRecsPerPage = 10;
		private const int defaultPage = 1;
		/// <summary>
		/// Default number of pages in a page group.
		/// </summary>
		private const int defaultPagesPerGroup = 10;

		public readonly string _base_name;

		public SearchDTO(string baseName)
		{
			if (string.IsNullOrWhiteSpace(baseName)) throw new Exception("Search requires baseName for element ids");
			_base_name = baseName.ToLower().Replace("_", "-");
			Page = 1;
			sortDir = "ASC";
		}

		#region pagination
		private int _recCount = 0;
		/// <summary>
		/// Number of records in search results
		/// </summary>
		/// <remarks>
		/// When this value is set the last page in the search results is computed based on current settings.
		/// </remarks>
		public int recCount { get { return _recCount; } set { _recCount = value; } }

		private int _recsPerPage = defaultRecsPerPage;
		/// <summary>
		/// Setting for number of records per page. 
		/// </summary>
		/// <remarks>
		/// Default value of records per page is set by const defaultRecsPerPage.
		/// </remarks>
		public int recsPerPage { get { return _recsPerPage; } set { _recsPerPage = value; } }

		private int _pagesPerGroup = defaultPagesPerGroup;
		/// <summary>
		/// Number of pages shown in selection. (rounded to next odd number)
		/// </summary>
		/// <remarks>
		/// Always rounded up to next odd number.
		/// </remarks>
		public int PagesPerGroup { get { return _pagesPerGroup; } set { _pagesPerGroup = value; } }

		public int PageFirst { get { return 1; } }
		public int PagePrev { get { return Math.Max(1, _page - 1); } }
		public int PageNext { get { return Math.Min(_page + 1, PageLast); } }

		/// <summary>
		/// Return the page starting the next group
		/// </summary>
		public int GroupPageNext { get { return Math.Min(PageLast, _page + PagesPerGroup); } }

		/// <summary>
		/// Return the page starting the next group
		/// </summary>
		public int GroupPagePrev { get { return Math.Max(1, _page - PagesPerGroup); } }

		public int GroupCount { get { return (PageLast / _pagesPerGroup) + (PageLast % _pagesPerGroup > 0 ? 1 : 0); } }

		/// <summary>                                                                                    
		/// Return the current group
		/// </summary>
		public int GroupCurr { get { return (_page / _pagesPerGroup) + (_page % _pagesPerGroup > 0 ? 1 : 0); } }

		/// <summary>
		/// Last page in collection.
		/// </summary>
		/// <remarks>
		/// Last page should be recomputed whenever recCount of recsPerPage changes.
		/// </remarks>
		public int PageLast { get { return (_recCount / _recsPerPage) + (_recCount % _recsPerPage > 0 ? 1 : 0); } }

		/// <summary>
		/// Current page
		/// </summary>
		/// <remarks>
		/// Page will always be in range of 1 to last page.
		/// </remarks>
		public int Page { get; set; }
		private int _page { get { return Math.Max(1, Math.Min(Page, PageLast)); } }

		/// <summary>
		/// starting page in page range
		/// </summary>
		/// <remarks>
		/// Based on current page and last page and size of page range.
		/// </remarks>
		public int PageRangeStart { get { return Math.Max(1, (GroupCurr - 1) * PagesPerGroup + 1); } }

		/// <summary>
		/// ending page in page range
		/// </summary>
		public int PageRangeEnd { get { return Math.Min(PageLast, GroupCurr * PagesPerGroup); } }

		public int skip { get { return (Page - 1) * recsPerPage; } }

		#endregion

		#region sorting
		/// <summary>
		/// Name of column to sort.
		/// </summary>
		public string sort { get; set; }

		/// <summary>
		/// Sort direction [ASC,DESC]
		/// </summary>
		public string sortDir { get; set; }

		/// <summary>
		/// Generic function for sorting grid view column.
		/// </summary>
		public static Func<IQueryable<T>, bool, IOrderedQueryable<T>> CreateOrderingFunc<T, TKey>(Expression<Func<T, TKey>> keySelector)
		{
			return (source, ascending) => ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
		}

		#endregion

		#region Views

		/// <summary>
		/// Indicates that returned view is search terms and results as oppposed to results only.
		/// </summary>
		public bool entireView { get; set; }

		/// <summary>
		/// Seed name to use in nameing results, dialog and functions.
		/// </summary>
		public string seedName { get { return (string.IsNullOrWhiteSpace(FormView) ? "search-" : FormView + "-") + _base_name.ToLower().Replace("_", "-"); } }

		/// <summary>
		/// ID of search form.
		/// </summary>
		public string FormID { get { return seedName + "-frm"; } }

		/// <summary>
		/// optional ID of div containing search form.
		/// </summary>
		public string FormView { get; set; }

		/// <summary>
		/// ID of div for search results.
		/// </summary>
		public string ResultView { get { return seedName + "-res"; } }

		/// <summary>
		/// ID of div for dialog
		/// </summary>
		public string DialogView { get { return seedName + "-dlg"; } }

		/// <summary>
		/// Base to use in function names
		/// </summary>
		public string funcBase { get { return seedName.Replace("-", "_"); } }

		/// <summary>
		/// Generated refresh results function name
		/// </summary>
		public string funcRefresh { get { return "Refresh_" + funcBase + "_res"; } }

		/// <summary>
		/// Generated open dialog function name
		/// </summary>
		public string funcOpen { get { return "Open_" + funcBase + "_dlg"; } }

		#endregion
	}
}

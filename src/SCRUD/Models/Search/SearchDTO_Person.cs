/*
	Created By Gene Kochanowsky	

	All I ask is that you say who you stole this from.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUD.Models
{
    public class SearchDTO_Person : SearchDTO
    {
		public SearchDTO_Person() : base("person")
		{
			sortDir = "ASC";
			sort = Person.LastName_Display;
			recsPerPage = 10;
		}

		public IEnumerable<Person> results { get; set; }

		/// <summary>
		/// Field ordering functions used for paginated column sorting.
		/// </summary>
		public readonly IDictionary<string, Func<IQueryable<Person>, bool, IOrderedQueryable<Person>>>
		Orderings = new Dictionary<string, Func<IQueryable<Person>, bool, IOrderedQueryable<Person>>>
		{
			{Person.FirstName_Display, SearchDTO.CreateOrderingFunc<Person,System.String>(p=>p.FirstName)},
			{Person.LastName_Display, SearchDTO.CreateOrderingFunc<Person,System.String>(p=>p.LastName)},
			{Person.DoB_Display, SearchDTO.CreateOrderingFunc<Person, System.DateTime>(p=>p.DoB)},
			{Person.GenderID_Display, SearchDTO.CreateOrderingFunc<Person,System.String>(p=>p.Gender.Code)}
		};

		[Display(Name = "First Name")]
		public string FirstName { get; set; }
		[Display(Name = "Last Name")]
		public string LastName { get; set; }
		[Display(Name = "From DoB"), DataType(DataType.Date)]
		public DateTime? DoB_From { get; set; }
		[Display(Name = "To DoB"), DataType(DataType.Date)]
		public DateTime? DoB_To { get; set; }
		[Display(Name = "Gender")]
		public int? GenderID { get; set; }
	}
}

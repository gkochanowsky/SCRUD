/*
	Created By Gene Kochanowsky	
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUD.Models
{
    public class Person
    {
		public const string FirstName_Display = "First Name";
		public const string LastName_Display = "Last Name";
		public const string DoB_Display = "Birth Date";
		public const string GenderID_Display = "Gender";

		public int PersonId { get; set; }
		[Display(Name = Person.FirstName_Display)]
		public string FirstName { get; set; }
		[Display(Name = Person.LastName_Display)]
		public string LastName { get; set; }
		[Display(Name = Person.DoB_Display), DataType(DataType.Date)]
		public DateTime DoB { get; set; }
		[Display(Name = Person.GenderID_Display)]
		public int GenderID { get; set; }

		public virtual Gender Gender { get; set; }

		[NotMapped]
		public string formView { get; set; }
		[NotMapped]
		public string funcRefresh { get; set; }
	}
}

/*
	Created By Gene Kochanowsky	
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUD.Models
{
    public class Gender
    {
		public int GenderId { get; set; }
		public string Code { get; set; }
		public string GenderDesc { get; set; }

		public virtual ICollection<Person> People { get; set; }
	}
}

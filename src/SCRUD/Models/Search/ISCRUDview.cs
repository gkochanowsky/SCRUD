using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUD.Models
{
    public interface ISCRUDview
    {
		string formView { get; set; }
		string funcRefresh { get; set; }
		bool isModal { get; set; }
	}
}

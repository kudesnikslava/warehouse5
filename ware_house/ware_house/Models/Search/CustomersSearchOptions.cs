using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary.Models.Search;

namespace ware_house.Models.Search
{
    public class CustomersSearchOptions : BaseSearchOptions
	{

		public string FirstName { get; set; }


		public string LastName { get; set; }


		public byte Age { get; set; }


		public DateTime? CreationDate { get; set; }
	}
}
